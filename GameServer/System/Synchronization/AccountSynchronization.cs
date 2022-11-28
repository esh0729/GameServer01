using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 계정과 동기 처리가 필요한 함수를 동기적으로 실행시키는 클래스
	/// </summary>
	public class AccountSynchronization : ClientPeerSynchronization
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Account m_account;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public AccountSynchronization(Account account, bool bGlobalLockRequired, ISFWork work)
			: base(account.clientPeer, bGlobalLockRequired, work)
		{
			m_account = account;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기 작업 시작 함수
		/// </summary>
		protected override void ProcessSynchronization()
		{
			Hero? hero = m_account.currentHero;
			if (hero != null)
			{
				HeroSynchronization heroSynchronization = new HeroSynchronization(hero, m_bGlobalLockRequired, m_work);
				heroSynchronization.ProcessSynchronization();
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
	}
}
