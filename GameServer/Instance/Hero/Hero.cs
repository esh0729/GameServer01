using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using ServerFramework;
using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 인게임 내 정보를 관리 하는 클래스
	/// </summary>
	public class Hero : Unit
	{
		/// <summary>
		/// 영웅 상태
		/// </summary>
		private enum HeroState
		{
			LoggedIn,
			Logout
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		// 업데이트 간격
		public const short kUpdateTimeTicks = 500;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		//
		// 영웅 정보
		//

		private Account m_account;
		private Guid m_id;
		private string? m_sName;
		private Character? m_character;

		private DateTimeOffset m_regTime;

		private HeroState m_state;

		private DateTimeOffset m_lastLoginTime;
		private DateTimeOffset m_lastLogoutTime;

		//
		// 작업자 및 업데이트 관련
		//

		private SFWorker m_worker;
		private Timer? m_timer;

		private DateTimeOffset m_currentUpdateTime;
		private DateTimeOffset m_prevUpdateTime;

		//
		// 마지막 입장 위치
		//

		private Location? m_lastLocation;
		private Vector3 m_lastPosition;
		private float m_fLastYRotation;

		//
		// 이전 입장 대륙
		//

		private Continent? m_previousContinent;
		private Vector3 m_previousPosition;
		private float m_fPreviousYRotation;

		//
		// 장소 입장
		//

		private EntranceParam? m_entranceParam;
		private bool m_bIsInitEntered;

		//
		// 이동
		//

		private bool m_bMoving;

		//
		// 행동
		//

		private CharacterAction? m_currentAction;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// 
		/// </summary>
		/// <param name="account">계정</param>
		public Hero(Account account)
		{
			if (account == null)
				throw new ArgumentNullException("account");

			//
			// 영웅 정보
			//

			m_account = account;
			m_id = Guid.Empty;
			m_sName = null;
			m_character = null;

			m_regTime = DateTimeOffset.MinValue;

			m_state = HeroState.Logout;

			m_lastLoginTime = DateTimeOffset.MinValue;
			m_lastLogoutTime = DateTimeOffset.MinValue;

			//
			// 작업 및 업데이트 관련
			//

			m_worker = new SFWorker();
			m_timer = null;

			m_currentUpdateTime = DateTimeOffset.MinValue;
			m_prevUpdateTime = DateTimeOffset.MinValue;

			//
			// 마지막 입장 위치
			//

			m_lastLocation = null;
			m_lastPosition = Vector3.zero;
			m_fLastYRotation = 0f;

			//
			// 이전 입장 대륙
			//

			m_previousContinent = null;
			m_previousPosition = Vector3.zero;
			m_fPreviousYRotation = 0f;

			//
			// 장소 입장
			//

			m_entranceParam = null;
			m_bIsInitEntered = false;

			//
			// 이동
			//

			m_bMoving = false;

			//
			// 행동
			//

			m_currentAction = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		//
		// 영웅 정보
		//

		public Account account
		{
			get { return m_account; }
		}

		public ClientPeer clientPeer
		{
			get { return m_account.clientPeer; }
		}

		public object syncObject
		{
			get { return m_account.syncObject; }
		}

		public Guid id
		{
			get { return m_id; }
		}

		public string name
		{
			get { return m_sName!; }
		}

		public Character character
		{
			get { return m_character!; }
		}

		public bool isLoggedIn
		{
			get { return m_state == HeroState.LoggedIn; }
		}

		public DateTimeOffset lastLoginTime
		{
			get { return m_lastLoginTime; }
		}

		public DateTimeOffset lastLogoutTime
		{
			get { return m_lastLogoutTime; }
		}

		//
		// 마지막 입장 위치
		//

		public Location? lastLocation
		{
			get { return m_lastLocation; }
		}

		public int lastLocationId
		{
			get { return m_lastLocation != null ? m_lastLocation.locationId : 0; }
		}

		public Vector3 lastPosition
		{
			get { return m_lastPosition; }
		}

		public float lastYRotation
		{
			get { return m_fLastYRotation; }
		}

		//
		// 이전 입장 대륙
		//

		public Continent? previousContinent
		{
			get { return m_previousContinent; }
		}

		public int previousContinentId
		{
			get { return m_previousContinent != null ? m_previousContinent.id : 0; }
		}

		public Vector3 previousPosition
		{
			get { return m_previousPosition; }
		}

		public float previousYRotation
		{
			get { return m_fPreviousYRotation; }
		}

		//
		// 장소 입장
		//

		public EntranceParam? entranceParam
		{
			get { return m_entranceParam; }
		}

		public bool isInitEntered
		{
			get { return m_bIsInitEntered; }
			set { m_bIsInitEntered = value; }
		}

		//
		// 이동
		//

		public bool moving
		{
			get { return m_bMoving; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 영웅 정보 설정 함수
		/// </summary>
		/// <param name="time">로그인 시각</param>
		/// <param name="drHero">영웅 정보가 담긴 데이터 행 객체</param>
		public void CompleteLogin(
			DateTimeOffset time,
			
			DataRow drHero)
		{
			if (drHero == null)
				throw new ArgumentNullException("drHero");

			// 작업자 시작
			m_worker.Start();

			// 영웅 기본 정보 설정
			CompleteLogin_Base(time, drHero);

			//
			//
			//

			// 계정에 로그인 영웅 설정
			m_account.currentHero = this;

			// 업데이트 타이머 설정
			m_timer = new Timer(Update, null, kUpdateTimeTicks, kUpdateTimeTicks);

			// 영웅 상태 로그인 상태로 변경
			m_state = HeroState.LoggedIn;
		}

		/// <summary>
		/// 영웅 기본 정보 설정 함수
		/// </summary>
		/// <param name="time">로그인 시각</param>
		/// <param name="drHero">영웅 정보가 담긴 데이터 행 객체</param>
		private void CompleteLogin_Base(DateTimeOffset time, DataRow drHero)
		{
			//
			// 영웅 기본 정보 설정
			//

			m_id = DBUtil.ToGuid(drHero["heroId"]);
			m_sName = Convert.ToString(drHero["name"]);

			int nCharacterId = Convert.ToInt32(drHero["characterId"]);
			m_character = Resource.instance.GetCharacter(nCharacterId);
			if (m_character == null)
				throw new Exception("캐릭터가 존재하지 않습니다. nCharacterId = " + nCharacterId);

			m_lastLoginTime = time;
			m_lastLogoutTime = DBUtil.ToDateTimeOffset(drHero["lastLogoutTime"]);

			m_regTime = DBUtil.ToDateTimeOffset(drHero["regTime"]);

			int nLastLocationId = Convert.ToInt32(drHero["lastLocationId"]);
			m_lastLocation = Resource.instance.GetContinent(nLastLocationId);
			m_lastPosition.x = Convert.ToSingle(drHero["lastXPosition"]);
			m_lastPosition.y = Convert.ToSingle(drHero["lastYPosition"]);
			m_lastPosition.z = Convert.ToSingle(drHero["lastZPosition"]);
			m_fLastYRotation = Convert.ToSingle(drHero["lastYRotation"]);

			int nPreviousContinentId = Convert.ToInt32(drHero["previousContinentId"]);
			m_previousContinent = Resource.instance.GetContinent(nPreviousContinentId);
			m_previousPosition.x = Convert.ToSingle(drHero["previousXPosition"]);
			m_previousPosition.y = Convert.ToSingle(drHero["previousYPosition"]);
			m_previousPosition.z = Convert.ToSingle(drHero["previousZPosition"]);
			m_fPreviousYRotation = Convert.ToSingle(drHero["previousYRotation"]);

			//
			// 영웅 위치 설정
			//

			Continent? enterContinent = null;
			Vector3 enterPosition = Vector3.zero;
			float fEnterYRotation = 0f;

			// 영웅의 마지막 퇴장 장소에 대한 정보가 있을 경우 해당 위치로 설정
			if (m_lastLocation != null)
			{
				if (m_lastLocation is Continent)
				{
					enterContinent = (Continent)m_lastLocation;
					enterPosition = m_lastPosition;
					fEnterYRotation = m_fLastYRotation;
				}
			}
			
			// 마지막 입장 위치가 존재하지 않거나 대륙이 아닐경우
			if (enterContinent == null)
			{
				// 마지막 대륙 정보가 있을 경우 해당 위치로 설정
				if (m_previousContinent != null)
				{
					enterContinent = m_previousContinent;
					enterPosition = m_previousPosition;
					fEnterYRotation = m_fPreviousYRotation;
				}
				// 마직막 대륙 정보가 없을 경우 게임설정에 있는 시작 위치로 설정
				else
				{
					enterContinent = Resource.instance.GetContinent(Resource.instance.startContinentId);
					enterPosition = Resource.instance.SelectStartPosition();
					fEnterYRotation = Resource.instance.SelectStartYRotation();
				}
			}

			// 영웅 초기입장에 대한 정보 생성
			HeroInitEnterParam param = new HeroInitEnterParam(enterContinent!, enterPosition, fEnterYRotation);
			// 장소 입장 데이터 설정
			SetEntranceParam(param);
		}

		/// <summary>
		/// 영웅 로그아웃 함수
		/// </summary>
		public void Logout()
		{
			// 로그인 상태 검사
			if (!isLoggedIn)
				return;

			m_state = HeroState.Logout;

			// 업데이트 타이머 종료
			m_timer!.Change(Timeout.Infinite, Timeout.Infinite);
			m_timer.Dispose();
			m_timer = null;

			// 로그아웃 시각 설정
			m_lastLogoutTime = DateTimeUtil.currentTime;

			// 마지막 위치 정보 저장
			if (m_bIsInitEntered)
				SetLastLocation();

			// 현재 장소 퇴장
			if (m_currentPlace != null)
				m_currentPlace.Exit(this, true, null);

			// DB 저장
			Logout_SaveToDB();

			//
			//
			//

			// 계정의 현재 영웅 정보 초기화
			m_account.currentHero = null;

			// Cache 에서 영웅 삭제
			Cache.instance.RemoveHero(m_id);

			// 비동기 독립 작업자에 영웅 작업자 종료 함수 큐잉
			Server.instance.AddStandaloneWork(new SFAction(m_worker.Stop));
		}

		/// <summary>
		/// 로그아웃시 영웅 정보를 데이터베이스에 저장하는 함수
		/// </summary>
		private void Logout_SaveToDB()
		{
			SFSqlWork dbWork = SqlWorkUtil.CreateHeroGameDBWork(m_id);

			// 영웅 로그아웃
			dbWork.AddCommand(GameDBDocEx.CSC_HeroLogout(this));

			dbWork.Schedule();
		}

		//
		// 작업자 및 업데이트 관련
		//

		/// <summary>
		/// 영웅 작업자 등록 함수
		/// </summary>
		/// <param name="work">등록할 작업</param>
		/// <param name="bGlobalLockRequired">Cache Lock 필요 여부</param>
		public void AddWork(ISFWork work, bool bGlobalLockRequired)
		{
			if (work == null)
				throw new ArgumentNullException("work");

			m_worker.Add(new SFAction<ISFWork, bool>(RunWork!, work, bGlobalLockRequired));
		}

		/// <summary>
		/// 영웅 작업자에서 처리할 작업을 동기적으로 처리하는 함수
		/// </summary>
		/// <param name="work">처리할 작업</param>
		/// <param name="bGlobalLockRequired">Cache Lock 필요 여부</param>
		private void RunWork(ISFWork work, bool bGlobalLockRequired)
		{
			try
			{
				HeroSynchronization heroSynchronization = new HeroSynchronization(this, bGlobalLockRequired, work);
				heroSynchronization.Start();
			}
			catch (Exception ex)
			{
				SFLogUtil.Error(GetType(), ex);
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
			// 영웅이 로그인 상태가 아닐 경우 리턴
			if (!isLoggedIn)
				return;

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
		// 마지막 입장 위치
		//

		/// <summary>
		/// 마지막 입장 위치 정보 설정 함수
		/// </summary>
		public void SetLastLocation()
		{
			if (m_currentPlace != null)
			{
				m_lastLocation = m_currentPlace.location;
				m_lastPosition = m_position;
				m_fLastYRotation = m_fYRotation;
			}
			// 현재 장소가 없을 경우 초기화
			else
			{
				m_lastLocation = null;
				m_lastPosition = Vector3.zero;
				m_fLastYRotation = 0f;
			}
		}

		//
		// 이전 입장 대륙
		//

		/// <summary>
		/// 이전 입장 대륙 정보 설정
		/// </summary>
		public void SetPreviousContinent()
		{
			// 현재 장소를 대륙인스턴스로 캐스팅
			ContinentInstance? continentInstance = m_currentPlace as ContinentInstance;

			// 현재 장소가 대륙이 아닐경우 리턴
			if (continentInstance == null)
				return;

			// 대륙과 위치 정보를 저장
			m_previousContinent = continentInstance.continent;
			m_previousPosition = m_position;
			m_fPreviousYRotation = m_fYRotation;
		}

		//
		// 장소 입장
		//

		/// <summary>
		/// 장소 입장 데이터 설정 함수
		/// </summary>
		/// <param name="param">장소 입장 데이터 관리 객체</param>
		public void SetEntranceParam(EntranceParam? param)
		{
			m_entranceParam = param;
		}

		//
		// 이동
		//

		/// <summary>
		/// 이동 시작 함수
		/// </summary>
		public void StartMove()
		{
			if (m_bMoving)
				return;

			m_bMoving = true;
		}

		/// <summary>
		/// 이동 종료 함수
		/// </summary>
		public void EndMove()
		{
			if (!m_bMoving)
				return;

			m_bMoving = false;
		}

		/// <summary>
		/// 이동 시 호출되는 함수
		/// </summary>
		public void OnMove()
		{
			// 행동 종료 함수 호출
			EndAction();
		}

		//
		// 행동
		//

		/// <summary>
		/// 행동 시작 함수
		/// </summary>
		/// <param name="action">행동</param>
		public void StartAction(CharacterAction action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			// 현재행동 저장
			m_currentAction = action;

			// 이벤트 송신
			// 관심 영역에 있는 영웅들에게 현재 위치 및 행동 송신
			ServerEvent.SendHeroActionStartedEvent(m_currentPlace!.GetInterestedClientPeers(m_sector!, m_id), m_id, m_currentAction.id, m_position, m_fYRotation);
		}

		/// <summary>
		/// 행동 종료 함수
		/// </summary>
		public void EndAction()
		{
			m_currentAction = null;
		}

		//
		//
		//

		protected override void OnSetSector(Sector? oldSector)
		{
			base.OnSetSector(oldSector);

			// 이전 섹터에서 삭제
			if (oldSector != null)
				oldSector.RemoveHero(m_id);

			// 현재 섹터에 추가
			if (m_sector != null)
				m_sector.AddHero(this);
		}

		//
		//
		//

		/// <summary>
		/// 영웅 정보 패킷데이터화 함수
		/// </summary>
		/// <returns>패킷데이터화 된 영웅 정보 객체</returns>
		public PDHero ToPDHero()
		{
			PDHero instance = new PDHero();
			instance.heroId = m_id;
			instance.name = m_sName;
			instance.characterId = m_character!.id;
			instance.position = m_position;
			instance.yRotation = m_fYRotation;
			instance.actionId = m_currentAction != null ? m_currentAction.id : 0;

			return instance;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 다수의 영웅 정보를 패킷데이터화 하는 함수
		/// </summary>
		/// <param name="heroes">영웅 객체를 저장하고 있는 열거자</param>
		/// <returns>패킷데이터화 된 영웅 정보 리스트</returns>
		public static List<PDHero> ToPDHeroes(IEnumerable<Hero> heroes)
		{
			List<PDHero> results = new List<PDHero>();

			foreach (Hero hero in heroes)
			{
				lock (hero.syncObject)
				{
					results.Add(hero.ToPDHero());
				}
			}

			return results;
		}
	}
}
