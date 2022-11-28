using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GameServer
{
	/// <summary>
	/// 대륙 정보 관리 클래스
	/// </summary>
	public class Continent : Location
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nId;

		private Rect3D m_rect;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public Continent()
		{
			m_nId = 0;

			m_rect = Rect3D.zero;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public int id
		{
			get { return m_nId; }
		}

		public Rect3D rect
		{
			get { return m_rect; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 내부 설정 함수
		/// </summary>
		/// <param name="dr">데이터 행 객체</param>
		public override void Set(DataRow dr)
		{
			base.Set(dr);

			m_nId = Convert.ToInt32(dr["continentId"]);

			m_rect.x = Convert.ToSingle(dr["x"]);
			m_rect.y = Convert.ToSingle(dr["y"]);
			m_rect.z = Convert.ToSingle(dr["z"]);
			m_rect.xSize = Convert.ToSingle(dr["xSize"]);
			m_rect.ySize = Convert.ToSingle(dr["ySize"]);
			m_rect.zSize = Convert.ToSingle(dr["zSize"]);
		}

		/// <summary>
		/// 대륙 내부의 위치에 포함되는지 확인하는 함수
		/// </summary>
		/// <param name="position">위치 정보</param>
		/// <returns>포함 될 경우 true, 포함되지 않을경우 false 반환</returns>
		public bool Contains(Vector3 position)
		{
			return m_rect.Contains(position);
		}
	}
}
