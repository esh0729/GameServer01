using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 동기 작업 처리 클래스
	/// </summary>
	public class SFSyncWork : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nType;
		private object m_id;

		private SFSync? m_sync;
		private List<SFSyncWork> m_syncWorks;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">동기 작업 타입</param>
		/// <param name="id">동기 작업 타입에 대한 id</param>
		public SFSyncWork(int nType, object id)
		{
			if (id == null)
				throw new ArgumentNullException("id");

			m_nType = nType;
			m_id = id;

			m_sync = null;
			m_syncWorks = new List<SFSyncWork>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			SFSyncFactory? factory = SFSyncFactoryManager.instance.GetSyncFactory(m_nType);
			if (factory == null)
				throw new Exception("Invalid type.");

			m_sync = factory.GetOrCreateHeroSync(m_id);
		}

		/// <summary>
		/// 동기 작업 추가 함수
		/// </summary>
		/// <param name="work">동기 작업</param>
		public void AddWork(SFSyncWork work)
		{
			if (work == null)
				throw new ArgumentNullException("work");

			m_syncWorks.Add(work);
		}

		/// <summary>
		/// 동기 작업 작동 함수
		/// </summary>
		public void Run()
		{
			RunWork();

			foreach (SFSyncWork work in m_syncWorks)
			{
				work.RunWork();
			}
		}

		/// <summary>
		/// 동기 작업 종료 함수
		/// </summary>
		public void End()
		{
			EndWork();

			foreach (SFSyncWork work in m_syncWorks)
			{
				work.EndWork();
			}
		}

		/// <summary>
		/// 진행 대기 요청 함수
		/// </summary>
		private void RunWork()
		{
			m_sync!.Waiting();
		}

		/// <summary>
		/// 진행 신호 요청 함수
		/// </summary>
		private void EndWork()
		{
			m_sync!.Set();
		}
	}
}
