﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using ServerFramework;
using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 영웅 생성 클라이언트 명령 처리 클래스
	/// </summary>
	public class HeroCreateCommandHandler : LoginRequiredCommandHandler<HeroCreateCommandBody,HeroCreateResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		public const short kResult_ExistHeroName = 101;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private DateTimeOffset m_currentTime;

		private Guid m_heroId;
		private string? m_sName;
		private Character? m_character;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public HeroCreateCommandHandler()
		{
			m_heroId = Guid.Empty;
			m_sName = null;
			m_character = null;
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
			// 상태 검사
			//

			if (m_myAccount!.isHeroLoggedIn)
				throw new CommandHandleException(kResult_Error, "현재 상태에서 사용할 수 없는 명령입니다.");

			//
			// 유효성 검사
			//

			if (m_body == null)
				throw new CommandHandleException(kResult_Error, "body가 null입니다.");

			m_sName = m_body.name;
			if (m_sName == null)
				throw new CommandHandleException(kResult_Error, "영웅 이름이 존재하지 않습니다.");

			if (!Server.instance.currentGameServer.IsMatchHeroNameRegex(m_sName))
				throw new CommandHandleException(kResult_Error, "영웅 이름이 유효하지 않습니다.");

			int nCharacterId = m_body.characterId;
			if (nCharacterId <= 0)
				throw new CommandHandleException(kResult_Error, "캐릭터ID가 유효하지 않습니다. nCharacterId = " + nCharacterId);

			m_character = Resource.instance.GetCharacter(nCharacterId);
			if (m_character == null)
				throw new CommandHandleException(kResult_Error, "캐릭터가 존재하지 않습니다.");

			//
			//
			//

			SFSyncWork syncWork = SyncWorkUtil.CreateUserSyncWork(m_myAccount.userId);
			RunnableStandaloneWork(Process, syncWork);
		}

		/// <summary>
		/// 영웅 생성 비동기 작업 함수
		/// </summary>
		private void Process()
		{
			// User DB에 대한 연결 객체 및 트랜잭션 객체
			SqlConnection? userConn = null;
			SqlTransaction? userTrans = null;

			// Game DB에 대한 연결 객체 및 트랜잭션 객체
			SqlConnection? gameConn = null;
			SqlTransaction? gameTrans = null;

			try
			{
				// UserDB 연결 객체 생성 및 트랜잭션 시작
				userConn = Util.OpenUserDBConnection();
				userTrans = userConn.BeginTransaction();

				/*
				// 영웅 이름 검사
				if (UserDBDoc.HeroName(userConn, userTrans, m_sName!) != null)
					throw new CommandHandleException(kResult_ExistHeroName, "이미 존재하는 영웅이름입니다. m_sName = " + m_sName);
				*/


				// GameDB 연결 객체 생성 및 트랜잭션 시작
				gameConn = Util.OpenGameDBConnection();
				gameTrans = gameConn.BeginTransaction();

				// 영웅 생성 제한수 검사
				if (GameDBDoc.HeroCount(gameConn, gameTrans, m_myAccount!.id) >= Resource.instance.heroCreationLimitCount)
					throw new CommandHandleException(kResult_Error, "영웅생성 제한수를 초과하였습니다.");

				// 영웅 등록 작업(UserDB)
				m_heroId = Guid.NewGuid();
				if (UserDBDocEx.AddUserHero(userConn, userTrans, m_myAccount.userId, m_heroId, m_sName!, m_character!.id) != 0)
					throw new CommandHandleException(kResult_Error, "사용자영웅 등록 실패.");

				/*
				// 영웅 이름 등록 작업(UserDB)
				if (UserDBDocEx.AddHeroName(userConn, userTrans, m_sName!, m_heroId) != 0)
					throw new CommandHandleException(kResult_Error, "영웅이름 등록 실패.");
				*/

				// 영웅 등록 작업(GameDB)
				if (GameDBDocEx.AddHero(gameConn, gameTrans, m_myAccount.id, m_heroId, m_sName!, m_character.id, m_currentTime) != 0)
					throw new CommandHandleException(kResult_Error, "영웅 등록 실패.");

				//
				//
				//

				// 모든 조회 및 등록 작업이 끝났을 경우 트랜잭션 커밋 처리후 데이터베이스 연결 닫기
				Util.Commit(ref userTrans);
				Util.Close(ref userConn);

				Util.Commit(ref gameTrans);
				Util.Close(ref gameConn);
			}
			finally
			{
				// 중간에 에러가 발생하여 데이터베이스 커밋 및 종료 처리가 되지 않았을 경우

				// UserDB 롤백 처리 및 연결 닫기
				if (userConn != null)
				{
					if (userTrans != null)
						Util.Rollback(ref userTrans);

					Util.Close(ref userConn);
				}

				// GameDB 롤백 처리 및 연결 닫기
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

			ProcessCompleted();
		}

		/// <summary>
		/// 응답 송신 처리 함수
		/// </summary>
		private void ProcessCompleted()
		{
			// 응답 송신
			SendResponseOK(null);
		}
	}
}
