using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 클라이언트 요청 타입
	/// </summary>
	public enum RequestType : byte
	{
		// 응답이 필요한 명령 타입
		Command = 1,
		// 응답이 필요없는 이벤트 타입
		Event
	}
}
