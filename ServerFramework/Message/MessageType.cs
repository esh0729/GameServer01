using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 사용자 메세지 타입
	/// </summary>
	public enum MessageType : byte
	{
		// 작업 요청
		OperationRequest,
		// 작업 응답
		OperationResponse,
		// 이벤트 데이터
		EventData,
	}
}
