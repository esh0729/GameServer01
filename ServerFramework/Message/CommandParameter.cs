using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	public enum CommandParameter : byte
	{
		// 명령 및 응답의 타입
		Name = 1,
		// 명령의 고유 ID
		Id
	}
}
