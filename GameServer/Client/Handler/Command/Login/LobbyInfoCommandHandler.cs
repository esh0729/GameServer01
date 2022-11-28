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
	/// 로비 정보 클라이언트 명령 처리 클래스
	/// </summary>
	public class LobbyInfoCommandHandler : LoginRequiredCommandHandler<LobbyInfoCommandBody,LobbyInfoResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private List<PDLobbyHero> m_heroes;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public LobbyInfoCommandHandler()
		{
			m_heroes = new List<PDLobbyHero>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnLoginRequiredCommandHandle()
		{
			//
			// 상태 검사
			//

			if (m_myAccount!.isHeroLoggedIn)
				throw new CommandHandleException(kResult_Error, "현재 상태에서 사용할 수 없는 명령입니다.");

			//
			//
			//

			SFSyncWork syncWork = SyncWorkUtil.CreateUserSyncWork(m_myAccount.userId);
			RunnableStandaloneWork(Process, syncWork);
		}

		/// <summary>
		/// 계정에 생성된 영웅 조회 비동기 작업 함수
		/// </summary>
		private void Process()
		{
			SqlConnection? conn = null;

			try
			{
				// GameDB 연결 객체 생성
				conn = Util.OpenGameDBConnection();

				// 영웅 목록 조회
				DataRowCollection drcHeroes = GameDBDoc.Heroes(conn, null, m_myAccount!.id);

				foreach (DataRow drHero in drcHeroes)
				{
					PDLobbyHero hero = new PDLobbyHero();
					hero.heroId = DBUtil.ToGuid(drHero["heroId"]);
					hero.name = Convert.ToString(drHero["name"]);
					hero.characterId = Convert.ToInt32(drHero["characterId"]);

					m_heroes.Add(hero);
				}

				//
				//
				//

				// 데이터베이스 연결 닫기
				Util.Close(ref conn);
			}
			finally
			{
				// 중간에 에러가 발생하여 데이터베이스 종료 처리가 되지 않았을 경우 연결 닫기
				if (conn != null)
					Util.Close(ref conn);
			}
		}

		/// <summary>
		/// 비동기 작업 완료 이후 호출되는 함수
		/// </summary>
		protected override void OnWorkSuccess()
		{
			base.OnWorkSuccess();

			// 작업 완료 처리 함수 호출
			ProcessCompleted();
		}

		/// <summary>
		/// 응답 송신 함수
		/// </summary>
		private void ProcessCompleted()
		{
			LobbyInfoResponseBody resBody = new LobbyInfoResponseBody();
			resBody.heroes = m_heroes.ToArray();

			// 응답 송신
			SendResponseOK(resBody);
		}
	}
}
