using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 이벤트 신호 처리 클래스
	/// </summary>
	public class SFSync
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private object m_id;

		private AutoResetEvent m_progressSignal;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		///
		/// </summary>
		/// <param name="id">특정 타입의 ID</param>
		public SFSync(object id)
		{
			if (id == null)
				throw new ArgumentNullException("id");

			m_id = id;

			m_progressSignal = new AutoResetEvent(true);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 진행 대기 처리 함수
		/// </summary>
		public void Waiting()
		{
			m_progressSignal.WaitOne();
		}

		/// <summary>
		/// 진행 신호 받음 처리 함수
		/// </summary>
		public void Set()
		{
			m_progressSignal.Set();
		}
	}
}
