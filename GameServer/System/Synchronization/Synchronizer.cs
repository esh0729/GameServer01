using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// ISynchronization 객체의 동기 처리 클래스
	/// </summary>
	public class Synchronizer
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private object m_syncObject;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public Synchronizer()
		{
			m_syncObject = new object();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기화 처리 함수
		/// </summary>
		/// <param name="synchronization">동기 처리 객체</param>
		public void Synchronization(ISynchronization synchronization)
		{
			lock (m_syncObject)
			{
				synchronization.Run();
			}
		}
	}
}
