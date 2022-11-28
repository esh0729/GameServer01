using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// (추상 클래스)클라이언트 명령 데이터 관리 클래스
	/// </summary>
	public abstract class CommandBody : Body
	{
	}

	/// <summary>
	/// (추상 클래스)클라이언트 명령 응답 데이터 관리 클래스
	/// </summary>
	public abstract class ResponseBody : Body
	{
	}
}
