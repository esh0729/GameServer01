using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 특정 시점에 실행을 위한 매개변수를 가지고 있지 않는 대리자를 처리하는 클래스
	/// </summary>
	public class SFAction : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Action m_action;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action">매개변수를 가지고 있지 않는 대리자</param>
		public SFAction(Action action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			m_action = action;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 대리자 실행 함수
		/// </summary>
		public void Run()
		{
			m_action();
		}
	}

	/// <summary>
	/// 특정 시점에 실행을 위한 매개변수를 1개 가지고 있는 대리자를 처리하는 클래스
	/// </summary>
	public class SFAction<T1> : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Action<T1?> m_action;
		private T1? m_arg1;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action">매개변수를 1개 가지고 있는 대리자</param>
		/// <param name="arg1">대리자의 매개변수</param>
		public SFAction(Action<T1?> action, T1? arg1)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			m_action = action;
			m_arg1 = arg1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 대리자 실행 함수
		/// </summary>
		public void Run()
		{
			m_action(m_arg1);
		}
	}

	/// <summary>
	/// 특정 시점에 실행을 위한 매개변수를 2개 가지고 있는 대리자를 처리하는 클래스
	/// </summary>
	public class SFAction<T1,T2> : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Action<T1?,T2?> m_action;
		private T1? m_arg1;
		private T2? m_arg2;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action">매개변수를 2개 가지고 있는 대리자</param>
		/// <param name="arg1">대리자의 첫번째 매개변수</param>
		/// <param name="arg2">대리자의 두번째 매개변수</param>
		public SFAction(Action<T1?,T2?> action, T1? arg1, T2? arg2)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			m_action = action;
			m_arg1 = arg1;
			m_arg2 = arg2;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 대리자 실행 함수
		/// </summary>
		public void Run()
		{
			m_action(m_arg1, m_arg2);
		}
	}
}
