using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 스케줄 전용 작업자
	/// </summary>
	public class SFScheduleWorker
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private SFWorker m_worker;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		private SFScheduleWorker()
		{
			m_worker = new SFWorker();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 작업자 시작 함수
		/// </summary>
		public void Start()
		{
			m_worker.Start();
		}

		/// <summary>
		/// 작업자 작업 추가 함수
		/// </summary>
		/// <param name="schedule">스케줄 작업 객체</param>
		public void AddWork(SFSchedule schedule)
		{
			if (schedule == null)
				throw new ArgumentNullException("schedule");

			m_worker.Add(schedule);
		}

		/// <summary>
		/// 작업자 종료 함수
		/// </summary>
		public void Stop()
		{
			m_worker.Stop();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static readonly SFScheduleWorker s_instance;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructors

		/// <summary>
		/// 
		/// </summary>
		static SFScheduleWorker()
		{
			s_instance = new SFScheduleWorker();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static SFScheduleWorker instnace
		{
			get { return s_instance; }
		}
	}
}
