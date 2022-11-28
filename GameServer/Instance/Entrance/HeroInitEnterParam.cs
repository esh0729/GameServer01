using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 영웅 초기 입장 데이터 관리 클래스
	/// </summary>
	public class HeroInitEnterParam : EntranceParam
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		private Continent m_continent;
		private Vector3 m_position;
		private float m_fYRotation;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="continent">입장 대륙 데이터</param>
		/// <param name="position">입장 위치</param>
		/// <param name="fYRotation">입장 방향</param>
		public HeroInitEnterParam(Continent continent, Vector3 position, float fYRotation)
		{
			if (continent == null)
				throw new ArgumentNullException("continent");

			m_continent = continent;
			m_position = position;
			m_fYRotation = fYRotation;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public Continent continent
		{
			get { return m_continent; }
		}

		public Vector3 position
		{
			get { return m_position; }
		}

		public float yRotation
		{
			get { return m_fYRotation; }
		}
	}
}
