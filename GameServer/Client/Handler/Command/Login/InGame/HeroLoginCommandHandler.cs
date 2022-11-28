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
	/// 영웅 로그인 클라이언트 명령 처리 클래스
	/// </summary>
	public class HeroLoginCommandHandler : LoginRequiredCommandHandler<HeroLoginCommandBody,HeroLoginResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private DateTimeOffset m_currentTime;

		private Guid m_heroId;
		private DataRow? m_drHero;
		private Hero? m_hero;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public HeroLoginCommandHandler()
		{
			m_currentTime = DateTimeOffset.MinValue;

			m_heroId = Guid.Empty;
			m_drHero = null;
			m_hero = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnLoginRequiredCommandHandle()
		{
			m_currentTime = DateTimeUtil.currentTime;

			//
			// 유효성 검사
			//

			if (m_body == null)
				throw new CommandHandleException(kResult_Error, "body가 null입니다.");

			m_heroId = m_body.heroId;
			if (m_heroId == Guid.Empty)
				throw new CommandHandleException(kResult_Error, "영웅ID가 유효하지 않습니다.");

			//
			// 계정 상태 검사
			//

			if (m_myAccount!.isHeroLoggedIn)
				throw new CommandHandleException(kResult_Error, "이미 영웅이 로그인 중 입니다.");

			//
			//
			//

			SFSyncWork syncWork = SyncWorkUtil.CreateHeroSyncWork(m_heroId);
			RunnableStandaloneWork(Process, syncWork);
		}

		/// <summary>
		/// 영웅 조회 비동기 처리 함수
		/// </summary>
		private void Process()
		{
			SqlConnection? conn = null;

			try
			{
				// GameDB 연결 객체 생성
				conn = Util.OpenGameDBConnection();

				// 영웅 유효성 검사
				m_drHero = GameDBDoc.Hero(conn, null, m_heroId);
				if (m_drHero == null)
					throw new CommandHandleException(kResult_Error, "영웅이 존재하지 않습니다. m_heroId = " + m_heroId);

				Guid accountId = DBUtil.ToGuid(m_drHero["accountId"]);
				if (accountId != m_myAccount!.id)
					throw new CommandHandleException(kResult_Error, "다른 계정의 영웅입니다. m_heroId = " + m_heroId);

				//
				//
				//

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

			Validate();

			ProcessCompleted();
		}

		/// <summary>
		/// 유효성 확인 함수
		/// </summary>
		private void Validate()
		{
			if (m_myAccount!.isHeroLoggedIn)
				throw new CommandHandleException(kResult_Error, "이미 영웅이 로그인 중 입니다.");
		}

		/// <summary>
		/// 영웅 로그인 완료 이후 데이터 저장 및 클라이언트 응답 송신 처리 함수
		/// </summary>
		private void ProcessCompleted()
		{
			// 계정 객체에 영웅 로그인 처리
			m_hero = new Hero(m_myAccount!);
			m_hero.CompleteLogin(m_currentTime, m_drHero!);
			m_hero.isInitEntered = true;

			// Cache에 영웅 객체 저장
			Cache.instance.AddHero(m_hero);

			// DB 저장
			SaveToDB_Game();

			// 응답객체 생성
			HeroLoginResponseBody resBody = new HeroLoginResponseBody();

			// 영웅 정보
			resBody.heroId = m_hero.id;
			resBody.name = m_hero.name;
			resBody.characterId = m_hero.character.id;

			// 위치 정보
			HeroInitEnterParam param = (HeroInitEnterParam)m_hero.entranceParam!;
			resBody.enterContinentId = param.continent.id;

			// 응답 송신
			SendResponseOK(resBody);
		}

		/// <summary>
		/// GameDB 수정 사항 처리 함수
		/// </summary>
		private void SaveToDB_Game()
		{
			SFSqlWork dbWork = SqlWorkUtil.CreateHeroGameDBWork(m_heroId);

			// 영웅 수정(영웅 로그인)
			dbWork.AddCommand(GameDBDoc.CSC_HeroLogin(m_heroId, m_hero!.lastLoginTime));

			dbWork.Schedule();
		}
	}
}
