using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 영웅과 동기 처리가 필요한 함수를 동기적으로 실행시키는 클래스
	/// </summary>
	public class HeroSynchronization : AccountSynchronization
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Hero m_hero;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public HeroSynchronization(Hero hero, bool bGlobalLockRequired, ISFWork work)
			: base(hero.account, bGlobalLockRequired, work)
		{
			if (hero == null)
				throw new ArgumentNullException("hero");

			m_hero = hero;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 동기 작업 진행 함수
		/// </summary>
		protected override void ProcessSynchronization()
		{
			if (m_bGlobalLockRequired)
			{
				lock (Cache.instance.syncObject)
				{
					PhysicalPlace? place = m_hero.currentPlace;
					if (place != null)
					{
						lock (place.syncObject)
						{
							RunWork();
						}
					}
					else
						RunWork();
				}
			}
			else
			{
				PhysicalPlace? place = m_hero.currentPlace;
				if (place != null)
				{
					lock (place.syncObject)
					{
						if (place.GetHero(m_hero.id) != null)
						{
							RunWork();
							return;
						}
					}
				}

				RunWork();
			}
		}
	}
}
