using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 동기 작업 클래스에 대한 추가 기능을 제공하는 클래스
	/// </summary>
	public static class SyncWorkUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 동기 작업 클래스 관련 초기화 함수
		/// </summary>
		public static void Initialize()
		{
			SFSyncFactoryManager.instance.AddSyncFactory(new SFSyncFactory((int)SyncWorkType.User));
			SFSyncFactoryManager.instance.AddSyncFactory(new SFSyncFactory((int)SyncWorkType.Hero));
		}

		/// <summary>
		/// 사용자 동기 작업 처리 클래스 생성 함수
		/// </summary>
		/// <param name="userId">사용자 ID</param>
		/// <returns>사용자 동기 작업 처리 객체 반환</returns>
		public static SFSyncWork CreateUserSyncWork(Guid userId)
		{
			SFSyncWork syncWork = new SFSyncWork((int)SyncWorkType.User, userId);
			syncWork.Initialize();

			return syncWork;
		}

		/// <summary>
		/// 영웅 동기 작업 처리 클래스 생성 함수
		/// </summary>
		/// <param name="heroId">영웅 ID</param>
		/// <returns>영웅 동기 작업 처리 객체 반환</returns>
		public static SFSyncWork CreateHeroSyncWork(Guid heroId)
		{
			SFSyncWork syncWork = new SFSyncWork((int)SyncWorkType.Hero, heroId);
			syncWork.Initialize();

			return syncWork;
		}
	}
}
