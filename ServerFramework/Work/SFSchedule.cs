using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// (추상 클래스)스케줄 작업 클래스
	/// </summary>
	public abstract class SFSchedule : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// (추상 함수)특정 작업을 실행하는 함수
		/// </summary>
		public abstract void Run();

		/// <summary>
		/// 특정 작업을 스케줄 작업자에 추가하는 함수
		/// </summary>
		public void Schedule()
		{
			SFScheduleWorker.instnace.AddWork(this);
		}
	}
}
