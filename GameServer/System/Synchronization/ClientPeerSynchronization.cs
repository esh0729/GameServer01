using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 클라이언트 피어와 동기 처리가 필요한 함수를 동기적으로 실행시키는 클래스
	/// </summary>
	public class ClientPeerSynchronization : ISynchronization
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private ClientPeer m_clientPeer;

		protected bool m_bGlobalLockRequired;
		protected ISFWork m_work;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clientPeer">클라이언트 피어</param>
		/// <param name="work">실행할 작업</param>
		public ClientPeerSynchronization(ClientPeer clientPeer, bool bGlobalLockRequired, ISFWork work)
		{
			if (clientPeer == null)
				throw new ArgumentNullException("clientPeer");

			if (work == null)
				throw new ArgumentNullException("work");

			m_clientPeer = clientPeer;

			m_bGlobalLockRequired = bGlobalLockRequired;
			m_work = work;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기 작업 시작 함수
		/// </summary>
		public void Start()
		{
			m_clientPeer.synchronizer.Synchronization(this);
		}

		/// <summary>
		/// 내부 로직 실행 함수
		/// </summary>
		void ISFWork.Run()
		{
			ProcessSynchronization();
		}

		/// <summary>
		/// 동기 작업 진행 함수
		/// </summary>
		protected virtual void ProcessSynchronization()
		{
			Account? account = m_clientPeer.account;
			if (account != null)
			{
				AccountSynchronization accountSynchronization = new AccountSynchronization(account, m_bGlobalLockRequired, m_work);
				accountSynchronization.ProcessSynchronization();
			}
			else
			{
				if (m_bGlobalLockRequired)
				{
					lock (Cache.instance.syncObject)
					{
						RunWork();
					}
				}
				else
					RunWork();
			}
		}

		/// <summary>
		/// 작업 실행 함수
		/// </summary>
		protected void RunWork()
		{
			lock (m_clientPeer.syncObject)
			{
				m_work.Run();
			}
		}
	}
}
