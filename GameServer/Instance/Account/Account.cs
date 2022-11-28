using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GameServer
{
	/// <summary>
	/// 사용자가 서버에 로그인하여 생성되는 정보를 저장하는 클래스
	/// </summary>
	public class Account
	{
		/// <summary>
		/// 계정 상태
		/// </summary>
		private enum AccountState
		{
			Login,
			Logout
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private ClientPeer m_clientPeer;
		private Guid m_id;

		private Guid m_userId;
		private DateTimeOffset m_regTime;

		private Hero? m_currentHero;

		private AccountState m_state;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clientPeer">클라이언트 피어</param>
		public Account(ClientPeer clientPeer)
		{
			if (clientPeer == null)
				throw new ArgumentNullException("clientPeer");

			m_clientPeer = clientPeer;
			m_id = Guid.Empty;

			m_userId = Guid.Empty;
			m_regTime = DateTimeOffset.MinValue;

			m_currentHero = null;

			m_state = AccountState.Logout;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public ClientPeer clientPeer
		{
			get { return m_clientPeer; }
		}

		public object syncObject
		{
			get { return m_clientPeer.syncObject; }
		}

		public Guid id
		{
			get { return m_id; }
		}

		public Guid userId
		{
			get { return m_userId; }
		}

		public DateTimeOffset regTime
		{
			get { return m_regTime; }
		}

		public Hero? currentHero
		{
			get { return m_currentHero; }
			set { m_currentHero = value; }
		}

		public bool isLoggedIn
		{
			get { return m_state == AccountState.Login; }
		}

		public bool isHeroLoggedIn
		{
			get { return m_currentHero != null; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		/// <summary>
		/// 초기화 함수(신규 사용자)
		/// </summary>
		/// <param name="accountId">계정 ID</param>
		/// <param name="userid">사용자 ID</param>
		/// <param name="time">현재 시각</param>
		public void Initialize(Guid accountId, Guid userid, DateTimeOffset time)
		{
			m_id = accountId;

			m_userId = userid;
			m_regTime = time;

			m_state = AccountState.Login;
		}

		/// <summary>
		/// 초기화 함수(기존 사용자)
		/// </summary>
		/// <param name="dr"></param>
		public void Initialize(DataRow dr)
		{
			if (dr == null)
				throw new ArgumentNullException("dr");

			m_id = DBUtil.ToGuid(dr["accountId"]);

			m_userId = DBUtil.ToGuid(dr["userId"]);
			m_regTime = DBUtil.ToDateTimeOffset(dr["regTime"]);

			m_state = AccountState.Login;
		}

		/// <summary>
		/// 계정 로그아웃 함수
		/// </summary>
		public void Logout()
		{
			if (!isLoggedIn)
				return;

			// 계정 상태 변경
			m_state = AccountState.Logout;

			// 영웅 로그아웃 처리
			if (m_currentHero != null)
				m_currentHero.Logout();

			// 클라이언트 피어 계정 삭제
			m_clientPeer.account = null;

			// Cache에서 계정 정보 삭제
			Cache.instance.RemoveAcount(m_id);
		}
	}
}
