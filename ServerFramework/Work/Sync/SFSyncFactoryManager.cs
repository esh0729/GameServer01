using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// SFSyncFactory 객체 관리 클래스
	/// </summary>
	public class SFSyncFactoryManager
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Dictionary<int, SFSyncFactory> m_factories;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		private SFSyncFactoryManager()
		{
			m_factories = new Dictionary<int, SFSyncFactory>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기 객체 관리 객체 추가 함수
		/// </summary>
		/// <param name="factory">추가할 동기 객체 관리 객체</param>
		public void AddSyncFactory(SFSyncFactory factory)
		{
			if (factory == null)
				throw new ArgumentNullException("factory");

			m_factories.Add(factory.type, factory);
		}

		/// <summary>
		/// 동기 객체 관리 객체 호출 함수
		/// </summary>
		/// <param name="nType">동기 타입</param>
		/// <returns>해당하는 동기 객체 관리 객체 또는 null 반환</returns>
		public SFSyncFactory? GetSyncFactory(int nType)
		{
			SFSyncFactory? value;

			return m_factories.TryGetValue(nType, out value) ? value : null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static SFSyncFactoryManager s_instance;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructors

		/// <summary>
		/// 
		/// </summary>
		static SFSyncFactoryManager()
		{
			s_instance = new SFSyncFactoryManager();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static SFSyncFactoryManager instance
		{
			get { return s_instance; }
		}
	}
}
