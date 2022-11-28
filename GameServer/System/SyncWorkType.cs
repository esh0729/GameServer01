using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 동기 작업 타입
	/// </summary>
	public enum SyncWorkType : int
	{
		// 사용자
		User = 1,
		// 영웅
		Hero
	}
}
