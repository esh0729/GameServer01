using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using ClientCommon;
using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 계정 로그인 클라이언트 명령 처리 클래스
	/// </summary>
	public class LoginCommandHandler : CommandHandler<LoginCommandBody,LoginResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private AccessToken? m_accessToken;

		private Guid m_accountId;
		private Account? m_account;
		private DataRow? m_drAccount;

		private DateTimeOffset m_currentTime;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public LoginCommandHandler()
		{
			m_accessToken = null;

			m_accountId = Guid.Empty;
			m_account = null;
			m_drAccount = null;

			m_currentTime = DateTimeOffset.MinValue;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		// 계정 객체를 생성하여 Cache에 등록하기 때문에 Cache lock 처리 프로퍼티 true로 오버라이딩
		protected override bool globalLockRequired
		{
			get { return true; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnCommandHandle()
		{
			m_currentTime = DateTimeUtil.currentTime;
			
			//
			// 유효성 검사
			//

			if (m_body == null)
				throw new CommandHandleException(kResult_Error, "body가 null 입니다.");

			string? sAccessToken = m_body.accessToken;
			if (string.IsNullOrEmpty(sAccessToken))
				throw new CommandHandleException(kResult_Error, "액세스 토큰이 존재하지 않습니다.");

			m_accessToken = new AccessToken();
			if (!m_accessToken.Parse(sAccessToken))
				throw new CommandHandleException(kResult_Error, "액세스 토큰이 존재하지 않습니다.");

			if (!m_accessToken.Verify())
				throw new CommandHandleException(kResult_Error, "액세스 토큰이 유효하지 않습니다.");

			//
			//
			//

			SFSyncWork syncWork = SyncWorkUtil.CreateUserSyncWork(m_accessToken.userId);
			RunnableStandaloneWork(Process, syncWork);
		}

		/// <summary>
		/// 계정 조회 비동기 작업 처리 함수
		/// </summary>
		private void Process()
		{
			// User, Game DB에 대한 연결 객체 및 트랜잭션 객체
			SqlConnection? userConn = null;
			SqlConnection? gameConn = null;
			SqlTransaction? gameTrans = null;

			try
			{
				// UserDB 작업
				{
					// UserDB 연결 객체 생성
					userConn = Util.OpenUserDBConnection();

					// 사용자 조회
					DataRow? drUser = UserDBDoc.User(userConn, null!, m_accessToken!.userId);

					// 사용자 유효성 검사
					if (drUser == null)
						throw new CommandHandleException(kResult_Error, "사용자 정보가 존재하지 않습니다.");

					Guid userId = DBUtil.ToGuid(drUser["userId"]);
					string? sAccessSecret = Convert.ToString(drUser["accessSecret"]);

					if (m_accessToken.accessSecret != sAccessSecret)
						throw new CommandHandleException(kResult_Error, "사용자 정보가 유효하지 않습니다.");
				}

				//
				// GameDB 작업
				//

				{
					// GameDB 연결 객체 및 트랜잭션 객체 생성
					gameConn = Util.OpenGameDBConnection();
					gameTrans = gameConn.BeginTransaction();

					// 계정 유효성 검사
					m_drAccount = GameDBDoc.Account(gameConn, gameTrans, m_accessToken.userId);

					if (m_drAccount == null)
					{
						// 계정 등록
						m_accountId = Guid.NewGuid();

						if (GameDBDocEx.AddAccount(gameConn, gameTrans, m_accountId, m_accessToken.userId, m_currentTime) != 0)
							throw new CommandHandleException(kResult_Error, "계정 등록에 실패 했습니다.");
					}
				}

				//
				//
				//

				// 모든 조회 및 등록 작업이 끝났을 경우 트랜잭션 커밋 처리후 데이터베이스 연결 닫기
				Util.Close(ref userConn);

				Util.Commit(ref gameTrans);
				Util.Close(ref gameConn);
			}
			finally
			{
				// 중간에 에러가 발생하여 데이터베이스 커밋 및 종료 처리가 되지 않았을 경우

				// UserDB는 단순 조회 처리만 하기때문에 연결 종료 처리
				if (userConn != null)
					Util.Close(ref userConn);

				// GameDB에 대해서 데이터 추가 작업이 있기 때문에 작업에 대한 롤백 이후 연결 종료 처리
				if (gameConn != null)
				{
					if (gameTrans != null)
						Util.Rollback(ref gameTrans);

					Util.Close(ref gameConn);
				}
			}
		}

		/// <summary>
		/// 비동기 작업 완료 이후 호출되는 함수
		/// </summary>
		protected override void OnWorkSuccess()
		{
			base.OnWorkSuccess();

			CheckDuplicated();
		}

		/// <summary>
		/// 계정 다중 접속 체크 함수
		/// </summary>
		private void CheckDuplicated()
		{
			// 데이터베이스에 계정 정보가 있는 상태일 경우 계정ID 호출
			if (m_drAccount != null)
				m_accountId = DBUtil.ToGuid(m_drAccount["accountId"]);

			// 계정ID로 현재 접속중인 계정이 있는지 체크
			Account? loginedAccount = Cache.instance.GetAccount(m_accountId);

			// 이미 접속한 계정이 있을 경우 기존에 접속 되어있는 계정으로 중복접속 이벤트 송신 후 해당 계정 로그아웃 처리
			if (loginedAccount != null)
			{
				// 현재 접속중인 계정에게 중복 로그인 서버 이벤트 송신
				ServerEvent.SendLoginDuplicatedEvent(loginedAccount.clientPeer);

				// 로그아웃 처리
				loginedAccount.Logout();
			}

			ProcessCompleted();
		}

		/// <summary>
		/// 계정 로그인 완료 이후 데이터 저장 및 클라이언트 응답 송신을 처리하는 함수
		/// </summary>
		private void ProcessCompleted()
		{
			// 기존에 생성된 계정이 있을경우 해당 데이터로 계정 객체 생성
			if (m_drAccount != null)
			{
				m_account = new Account(clientPeer);
				m_account.Initialize(m_drAccount);
			}
			// 기존에 생성된 계정이 없을 경우 새로 계정 데이터 생성후 객체 생성
			else
			{
				m_account = new Account(clientPeer);
				m_account.Initialize(m_accountId, m_accessToken!.userId, m_currentTime);
			}

			// Cache에 계정 객체를 저장
			Cache.instance.AddAccount(m_account);

			// 클라이언트 피어에 계정 객체 등록
			clientPeer.account = m_account;

			// 로그인지 저장되는 데이터 DB 저장
			SaveToDB_User();

			// 응답 송신
			SendResponseOK(null);
		}

		/// <summary>
		/// UserDB 수정 사항 처리 함수
		/// </summary>
		private void SaveToDB_User()
		{
			SFSqlWork dbWork = SqlWorkUtil.CreateUserDBWork(m_account!.userId);

			// 사용자 수정(로그인)
			dbWork.AddCommand(UserDBDoc.CSC_UpdateUser_Login(m_account.userId, Server.instance.currentGameServer.id, clientPeer.ipAddress));

			dbWork.Schedule();
		}
	}
}
