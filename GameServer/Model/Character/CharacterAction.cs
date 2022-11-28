using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GameServer
{
	/// <summary>
	/// 캐릭터 행동 정보 관리 클래스
	/// </summary>
	public class CharacterAction
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public Character m_character;
		private int m_nId;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="character"></param>
		public CharacterAction(Character character)
		{
			m_character = character;
			m_nId = 0;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public Character character
		{
			get { return m_character; }
		}

		public int id
		{
			get { return m_nId; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 내부 설정 함수
		/// </summary>
		/// <param name="dr">데이터 행 객체</param>
		public void Set(DataRow dr)
		{
			if (dr == null)
				throw new ArgumentNullException("dr");

			m_nId = Convert.ToInt32(dr["actionId"]);
		}
	}
}
