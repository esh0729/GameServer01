using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 동기 객체를 생성 및 관리하는 클래스
	/// </summary>
	public class SFSyncFactory
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nType;
		private Dictionary<object, SFSync> m_syncs;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public SFSyncFactory(int nType)
		{
			m_nType = nType;
			m_syncs = new Dictionary<object, SFSync>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public int type
		{
			get { return m_nType; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기 객체 생성 및 호출 함수
		/// </summary>
		/// <param name="id">영웅 ID</param>
		/// <returns>영웅 타입의 해당 id를 가지고 있는 동기 객체</returns>
		public SFSync GetOrCreateHeroSync(object id)
		{
			SFSync? sync;

			if (!m_syncs.TryGetValue(id, out sync))
			{
				sync = new SFSync(id);
				m_syncs.Add(id, sync);
			}

			return sync;
		}
	}
}
