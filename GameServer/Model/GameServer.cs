using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

using ServerFramework;

namespace GameServer
{
	public class GameServer
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nId;

		private string? m_sDBPath;
		private Regex? m_heroNameRegex;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public GameServer()
		{
			m_nId = 0;

			m_sDBPath = null;
			m_heroNameRegex = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public int id
		{
			get { return m_nId; }
		}

		public string dbPath
		{
			get { return m_sDBPath!; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		public void Set(DataRow dr)
		{
			if (dr == null)
				throw new ArgumentNullException("dr");

			m_nId = Convert.ToInt32(dr["gameServerId"]);

			m_sDBPath = Convert.ToString(dr["dbPath"]);

			string? sHeroNameRegex = Convert.ToString(dr["heroNameRegex"]);
			if (sHeroNameRegex != null)
				m_heroNameRegex = new Regex(sHeroNameRegex);
			else
				SFLogUtil.Warn(GetType(), "영웅이름정규표현식이 존재하지 않습니다.");
		}

		public bool IsMatchHeroNameRegex(string sName)
		{
			return m_heroNameRegex!.IsMatch(sName);
		}
	}
}
