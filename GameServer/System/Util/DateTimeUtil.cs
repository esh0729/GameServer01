using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 시각 관련 추가 기능을 제공하는 클래스
	/// </summary>
	public static class DateTimeUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public static DateTimeOffset currentTime
		{
			get { return DateTimeOffset.Now; }
		}
	}
}
