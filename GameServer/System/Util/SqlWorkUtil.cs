using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 데이터베이스 작업에 대한 추가 기능을 제공하는 클래스
	/// </summary>
	public static class SqlWorkUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		public static SFSqlWork CreateUserDBWork(Guid userId)
		{
			return new SFSqlWork(Util.CreateUserDBConnection(), SyncWorkUtil.CreateUserSyncWork(userId));
		}

		public static SFSqlWork CreateHeroGameDBWork(Guid heroId)
		{
			return new SFSqlWork(Util.CreateUserDBConnection(), SyncWorkUtil.CreateHeroSyncWork(heroId));
		}
	}
}
