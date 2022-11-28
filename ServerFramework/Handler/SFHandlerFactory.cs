using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// (추상 클래스)핸들러 타입을 관리하는 클래스
	/// </summary>
	public abstract class SFHandlerFactory
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Dictionary<int, Type> m_handler;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public SFHandlerFactory()
		{
			m_handler = new Dictionary<int, Type>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			InitializeInteral();
		}

		/// <summary>
		/// 자식 클래스 초기화 함수
		/// </summary>
		protected abstract void InitializeInteral();

		/// <summary>
		/// 핸들러 타입 저장 함수
		/// </summary>
		/// <param name="nId">핸들러 타입 ID</param>
		protected void AddHandler<T>(int nId) where T : SFHandler
		{
			m_handler.Add(nId, typeof(T));
		}

		/// <summary>
		/// 핸들러 타입 호출 함수
		/// </summary>
		/// <param name="nId">핸들러 타입 ID</param>
		/// <returns>해당 핸들러 타입 ID에 대한 핸들러가 존재할 경우 핸들러 Type 반환, 존재하지 않을 경우 null 반환</returns>
		protected Type? GetHandler(int nId)
		{
			Type? value;

			return m_handler.TryGetValue(nId, out value) ? value : null;
		}
	}
}
