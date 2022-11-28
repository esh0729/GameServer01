using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GameServer
{
	/// <summary>
	/// 캐릭터 정보 관리 클래스
	/// </summary>
	public class Character
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nId;

		private float m_fMoveSpeed;

		//
		// 행동
		//

		private Dictionary<int, CharacterAction> m_actions;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public Character()
		{
			m_nId = 0;

			m_fMoveSpeed = 0f;

			//
			// 행동
			//

			m_actions = new Dictionary<int, CharacterAction>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public int id
		{
			get { return m_nId; }
		}

		public float moveSpeed
		{
			get { return m_fMoveSpeed; }
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

			m_nId = Convert.ToInt32(dr["characterId"]);

			m_fMoveSpeed = Convert.ToSingle(dr["moveSpeed"]);
		}

		//
		// 행동
		//

		/// <summary>
		/// 캐릭터 행동 저장 함수
		/// </summary>
		/// <param name="action">캐릭터 행동 데이터</param>
		public void AddAction(CharacterAction action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			m_actions.Add(action.id, action);
		}

		/// <summary>
		/// 캐릭터 행동 호출 함수
		/// </summary>
		/// <param name="nId">캐릭터 행동 ID</param>
		/// <returns>해당하는 캐릭터 행동 데이터 또는 null</returns>
		public CharacterAction? GetAction(int nId)
		{
			CharacterAction? value;

			return m_actions.TryGetValue(nId, out value) ? value : null;
		}
	}
}
