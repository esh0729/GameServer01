using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 사용자 메세지 인터페이스
	/// </summary>
	public interface ISFMessage
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		MessageType messageType { get; }
	}
}
