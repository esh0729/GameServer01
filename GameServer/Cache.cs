using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 인스턴스 데이터를 관리하는 클래스
	/// </summary>
	public class Cache
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		public const short kUpdateTimeTicks = 500;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private object m_syncObject;

		private SFWorker m_worker;
		private Timer? m_timer;

		private DateTimeOffset m_currentUpdateTime;
		private DateTimeOffset m_prevUpdateTime;

		//
		// 계정
		//

		private Dictionary<Guid, Account> m_accounts;

		//
		// 영웅
		//

		private Dictionary<Guid, Hero> m_heroes;

		//
		// 장소
		//

		private Dictionary<Guid, Place> m_places;

		//
		// 대륙
		//

		private Dictionary<int, ContinentInstance> m_continentInstances;

		//
		//
		//

		private bool m_bDisposed;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		private Cache()
		{
			m_syncObject = new object();

			m_worker = new SFWorker();
			m_timer = null;

			m_currentUpdateTime = DateTimeOffset.MinValue;
			m_prevUpdateTime = DateTimeOffset.MinValue;

			//
			// 계정
			//

			m_accounts = new Dictionary<Guid, Account>();

			//
			// 영웅
			//

			m_heroes = new Dictionary<Guid, Hero>();

			//
			// 장소
			//

			m_places = new Dictionary<Guid, Place>();

			//
			// 대륙
			//

			m_continentInstances = new Dictionary<int, ContinentInstance>();

			//
			//
			//

			m_bDisposed = false;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public object syncObject
		{
			get { return m_syncObject; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			SFLogUtil.System(GetType(), "Cache Initialize Started.");

			//
			//
			//

			// Cache 작업자 시작
			m_worker.Start();

			// 업데이트 타이머 시작
			m_timer = new Timer(Update, null, kUpdateTimeTicks, kUpdateTimeTicks);

			// 대륙 초기화
			InitContinent();

			//
			//
			//

			SFLogUtil.System(GetType(), "Cache Initialize Completed.");
		}

		/// <summary>
		/// 대륙 초기화 함수
		/// </summary>
		private void InitContinent()
		{
			foreach (Continent continent in Resource.instance.continents.Values)
			{
				ContinentInstance continentInstance = new ContinentInstance(continent);
				
				lock (continentInstance.syncObject)
				{
					continentInstance.Initialize();

					AddPlace(continentInstance);
				}
			}
		}

		/// <summary>
		/// 업데이트 등록 함수
		/// </summary>
		/// <param name="state">Timer에서 전달하는 매개 변수(사용X)</param>
		private void Update(object? state)
		{
			// 작업자에 업데이트 함수 큐잉
			AddWork(new SFAction(OnUpdate));
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

		/// <summary>
		/// Cache 작업 추가 함수
		/// </summary>
		/// <param name="work">Cache 작업자에 큐잉 할 작업</param>
		private void AddWork(ISFWork work)
		{
			m_worker.Add(new SFAction<ISFWork>(RunWork!, work));
		}

		/// <summary>
		/// Cache 작업 실행 함수
		/// </summary>
		/// <param name="work">처리할 작업</param>
		private void RunWork(ISFWork work)
		{
			// 동기화를 위한 Cache의 lock 처리
			lock (m_syncObject)
			{
				work.Run();
			}
		}

		//
		// 계정
		//

		/// <summary>
		/// 계정 객체 저장 함수
		/// </summary>
		/// <param name="account">저장 할 계정 객체</param>
		public void AddAccount(Account account)
		{
			if (account == null)
				throw new ArgumentNullException("account");

			m_accounts.Add(account.id, account);
		}

		/// <summary>
		/// 계정 객체 호출 함수
		/// </summary>
		/// <param name="accountId">호출 할 계정 ID</param>
		/// <returns>해당하는 계정 객체 또는 null</returns>
		public Account? GetAccount(Guid accountId)
		{
			Account? value;

			return m_accounts.TryGetValue(accountId, out value) ? value : null;
		}

		/// <summary>
		///계정 객체 삭제 함수
		/// </summary>
		/// <param name="accountId">삭제 할 계정 ID</param>
		public void RemoveAcount(Guid accountId)
		{
			m_accounts.Remove(accountId);
		}

		//
		// 영웅
		//

		/// <summary>
		/// 영웅 객제 저장 함수
		/// </summary>
		/// <param name="hero">저장 할 영웅 객체</param>
		public void AddHero(Hero hero)
		{
			m_heroes.Add(hero.id, hero);
		}

		/// <summary>
		/// 영웅 객체 호출 함수
		/// </summary>
		/// <param name="heroId">호출 할 영웅 ID</param>
		/// <returns>해당하는 영웅 객체 또는 null</returns>
		public Hero? GetHero(Guid heroId)
		{
			Hero? value;

			return m_heroes.TryGetValue(heroId, out value) ? value : null;
		}

		/// <summary>
		/// 영웅 객체 삭제 함수
		/// </summary>
		/// <param name="heroId">삭제 할 영웅 ID</param>
		public void RemoveHero(Guid heroId)
		{
			m_heroes.Remove(heroId);
		}

		//
		// 장소
		//

		/// <summary>
		/// 장소 객체 저장 함수
		/// </summary>
		/// <param name="place">저장 할 장소 객체</param>
		public void AddPlace(Place place)
		{
			m_places.Add(place.instanceId, place);

			switch (place.type)
			{
				case PlaceType.Continent: AddContinentInstance((ContinentInstance)place); break;
			}
		}

		/// <summary>
		/// 장소 객체 삭제 함수
		/// </summary>
		/// <param name="place">삭제 할 장소 객체</param>
		public void RemovePlace(Place place)
		{
			m_places.Remove(place.instanceId);

			switch (place.type)
			{
				case PlaceType.Continent: RemoveContinentInstance(((ContinentInstance)place).continent.id); break;
			}
		}

		//
		// 대륙
		//

		/// <summary>
		/// 대륙 인스턴스 객체를 컬렉션에 저장하는 함수
		/// </summary>
		/// <param name="continentInstance">대륙 인스턴스 객체</param>
		private void AddContinentInstance(ContinentInstance continentInstance)
		{
			m_continentInstances.Add(continentInstance.continent.id, continentInstance);
		}

		/// <summary>
		/// 대륙 인스턴스 객체 호출 함수
		/// </summary>
		/// <param name="nContinentId">대륙 ID</param>
		/// <returns>해당하는 대륙 인스턴스 객체 또는 null 반환</returns>
		public ContinentInstance? GetContinentInstance(int nContinentId)
		{
			ContinentInstance? value;

			return m_continentInstances.TryGetValue(nContinentId, out value) ? value : null;
		}

		/// <summary>
		/// 대륙 인스턴스 객체를 컬렉션에서 삭제한느 함수
		/// </summary>
		/// <param name="nContinentId">대륙 ID</param>
		private void RemoveContinentInstance(int nContinentId)
		{
			m_continentInstances.Remove(nContinentId);
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

			//
			//
			//

			// 업데이트 타이머 종료 처리
			m_timer!.Change(Timeout.Infinite, Timeout.Infinite);
			m_timer.Dispose();
			m_timer = null;

			// 계정들의 로그아웃 처리
			foreach (Account account in m_accounts.Values.ToArray())
			{
				AccountSynchronization accountSynchronization = new AccountSynchronization(account, false, new SFAction(account.Logout));
				accountSynchronization.Start();
			}

			// 장소 리소스 해제 처리
			foreach (Place place in m_places.Values.ToArray())
			{
				place.Dispose();
			}

			// 작업자 종료 처리
			m_worker.Stop();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static Cache s_instance;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructors

		/// <summary>
		/// 
		/// </summary>
		static Cache()
		{
			s_instance = new Cache();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static Cache instance
		{
			get { return s_instance; }
		}
	}
}
