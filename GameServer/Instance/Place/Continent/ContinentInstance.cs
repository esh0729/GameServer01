using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 대륙 장소 내부 정보를 관리하는 클래스
	/// </summary>
	public class ContinentInstance : PhysicalPlace
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Continent m_continent;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public ContinentInstance(Continent continent)
		{
			if (continent == null)
				throw new ArgumentNullException("continent");

			m_continent = continent;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public override PlaceType type
		{
			get { return PlaceType.Continent; }
		}

		public override Location location
		{
			get { return m_continent; }
		}

		public override Rect3D rect
		{
			get { return m_continent.rect; }
		}

		public Continent continent
		{
			get { return m_continent; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			InitPhysicalPlace();
		}

		//
		// 영웅
		//

		/// <summary>
		/// 영웅 퇴장 완료 시 호출되는 함수
		/// </summary>
		/// <param name="hero"></param>
		/// <param name="bIsLogout"></param>
		/// <param name="entranceParam"></param>
		protected override void OnHeroExit(Hero hero, bool bIsLogout, EntranceParam? entranceParam)
		{
			base.OnHeroExit(hero, bIsLogout, entranceParam);

			if (!bIsLogout)
				hero.SetPreviousContinent();
		}
	}
}
