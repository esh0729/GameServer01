using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)장소에 대한 정보를 관리하는 클래스
	/// </summary>
	public abstract class Place
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		public const short kUpdateTimeTicks = 500;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected Guid m_instanceId;
		protected object m_syncObject;

		//
		// 작업자 및 업데이트 관련
		//

		protected SFWorker m_worker;
		protected Timer? m_timer;

		private DateTimeOffset m_currentUpdateTime;
		private DateTimeOffset m_prevUpdateTime;

		//
		// 영웅
		//

		protected Dictionary<Guid, Hero> m_heroes;

		//
		//
		//

		protected bool m_bDisposed;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public Place()
		{
			m_instanceId = Guid.NewGuid();
			m_syncObject = new object();

			//
			// 작업자 및 업데이트 관련
			//

			m_worker = new SFWorker();
			m_timer = null;

			m_currentUpdateTime = DateTimeOffset.MinValue;
			m_prevUpdateTime = DateTimeOffset.MinValue;

			//
			// 영웅
			//

			m_heroes = new Dictionary<Guid, Hero>();

			//
			//
			//

			m_bDisposed = false;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public Guid instanceId
		{
			get { return m_instanceId; }
		}

		public object syncObject
		{
			get { return m_syncObject; }
		}

		public abstract PlaceType type { get; }

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected void InitPlace()
		{
			m_worker = new SFWorker();
			m_worker.Start();

			m_timer = new Timer(Update, null, kUpdateTimeTicks, kUpdateTimeTicks);
		}

		//
		// 작업자 및 업데이트 관련
		//

		/// <summary>
		/// 장소 작업자 등록 함수
		/// </summary>
		/// <param name="work">등록할 작업</param>
		/// <param name="bGlobalLockRequired">Cache Lock 필요 여부</param>
		public void AddWork(ISFWork work, bool bGlobalLockRequired)
		{
			m_worker.Add(new SFAction<ISFWork, bool>(RunWork!, work, bGlobalLockRequired));
		}

		/// <summary>
		/// 장소 작업자에서 처리할 작업을 동기적으로 처리하는 함수
		/// </summary>
		/// <param name="work">처리할 작업</param>
		/// <param name="bGlobalLockRequired">Cache Lock 필요 여부</param>
		private void RunWork(ISFWork work, bool bGlobalLockRequired)
		{
			if (bGlobalLockRequired)
			{
				lock (Cache.instance.syncObject)
				{
					lock (m_syncObject)
					{
						work.Run();
					}
				}
			}
			else
			{
				lock (m_syncObject)
				{
					work.Run();
				}
			}
		}

		/// <summary>
		/// 업데이트 등록 함수
		/// </summary>
		/// <param name="state">Timer에서 전달하는 매개 변수(사용X)</param>
		private void Update(object? state)
		{
			AddWork(new SFAction(OnUpdate), false);
		}

		/// <summary>
		/// 업데이트 실행 함수
		/// </summary>
		private void OnUpdate()
		{
			// 에러 처리를 위해 업데이트 로직을 처리하는 함수를 따로 분리하여 try - catch 문으로 처리
			try
			{
				OnUpdateInternal();
			}
			catch (Exception ex)
			{
				SFLogUtil.Error(GetType(), ex);
			}
		}

		/// <summary>
		/// 업데이트 처리 함수
		/// </summary>
		private void OnUpdateInternal()
		{
			m_prevUpdateTime = m_currentUpdateTime;
			m_currentUpdateTime = DateTimeUtil.currentTime;

			// 첫 업데이트 함수 호출일 경우 시간 설정을 위해 건너 뜀
			if (m_prevUpdateTime == DateTimeOffset.MinValue)
				return;
		}

		//
		// 영웅
		//

		/// <summary>
		/// 영웅 객체 저장 함수
		/// </summary>
		/// <param name="hero"></param>
		private void AddHero(Hero hero)
		{
			m_heroes.Add(hero.id, hero);
		}

		/// <summary>
		/// 영웅 객체 호출 함수
		/// </summary>
		/// <param name="heroId">호출 할 영웅 ID</param>
		/// <returns>호출된 영웅 객체 또는 null</returns>
		public Hero? GetHero(Guid heroId)
		{
			Hero? value;

			return m_heroes.TryGetValue(heroId, out value) ? value : null;
		}

		/// <summary>
		/// 영웅 객체 삭제 함수
		/// </summary>
		/// <param name="heroId">삭제 할 영웅 ID</param>
		/// <returns>영웅 삭제 성공 시 true, 실패 시 false 반환</returns>
		private bool RemoveHero(Guid heroId)
		{
			return m_heroes.Remove(heroId);
		}

		/// <summary>
		/// 영웅 입장 함수
		/// </summary>
		/// <param name="hero">입장 영웅 객체</param>
		public void Enter(Hero hero)
		{
			if (hero == null)
				throw new ArgumentNullException("hero");

			OnHeroEntering(hero);

			AddHero(hero);

			OnHeroEnter(hero);
		}

		/// <summary>
		/// 영웅 입장 처리 시 호출되는 함수(장소에 영웅이 추가 전에 호출됨)
		/// </summary>
		protected virtual void OnHeroEntering(Hero hero)
		{

		}

		/// <summary>
		/// 영웅 입장 완료 시 호출되는 함수(장소에 영웅이 추가 이후 호출됨)
		/// </summary>
		/// <param name="hero">입장한 영웅 객체</param>
		protected virtual void OnHeroEnter(Hero hero)
		{

		}

		/// <summary>
		/// 영웅 퇴장 함수
		/// </summary>
		/// <param name="hero">퇴장 영웅 객체</param>
		/// <param name="bIsLogout">로그아웃 여부</param>
		/// <param name="entranceParam">다음 입장 장소 정보</param>
		public void Exit(Hero hero, bool bIsLogout, EntranceParam? entranceParam)
		{
			if (hero == null)
				throw new ArgumentNullException("hero");

			if (!RemoveHero(hero.id))
				return;

			OnHeroExit(hero, bIsLogout, entranceParam);
		}

		/// <summary>
		/// 영웅 퇴장 완료 시 호출되는 함수
		/// </summary>
		/// <param name="hero">퇴장 영웅 객체</param>
		/// <param name="bIsLogout">로그아웃 여부</param>
		/// <param name="entranceParam">다음 입장 장소 정보</param>
		protected virtual void OnHeroExit(Hero hero, bool bIsLogout, EntranceParam? entranceParam)
		{
		}

		/// <summary>
		/// 장소에 있는 모든 영웅의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 영웅들의 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetClientPeers(Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				clientPeers.Add(hero.clientPeer);
			}

			return clientPeers;
		}

		//
		//
		//

		/// <summary>
		/// 리소스 해제 함수
		/// </summary>
		public void Dispose()
		{
			if (m_bDisposed)
				return;

			m_bDisposed = true;

			OnDisposed();
		}

		/// <summary>
		/// 리소스 해제 완료 시 호출 함수
		/// </summary>
		protected virtual void OnDisposed()
		{
			m_timer!.Change(Timeout.Infinite, Timeout.Infinite);
			m_timer.Dispose();
			m_timer = null;

			m_worker.Stop();

			Cache.instance.RemovePlace(this);
		}
	}
}
