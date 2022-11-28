using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 특정 작업을 실행하는 함수의 구현을 필요로 하는 클래스의 인터페이스
	/// </summary>
	public interface ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 특정 작업을 실행하는 함수
		/// </summary>
		void Run();
	}
}
