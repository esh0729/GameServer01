using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 물리적으로 존재하는 장소의 정보를 분할하여 관리하는 클래스
	/// </summary>
	public class Sector
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private PhysicalPlace m_place;
		private int m_nRow;
		private int m_nCol;
		private Vector3 m_position;

		//
		// 영웅
		//

		private Dictionary<Guid, Hero> m_heroes;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="place">물리적으로 존재하는 장소</param>
		/// <param name="nRow">행 번호</param>
		/// <param name="nCol">열 번호</param>
		/// <param name="position">위치 정보</param>
		public Sector(PhysicalPlace place, int nRow, int nCol, Vector3 position)
		{
			if (place == null)
				throw new ArgumentNullException("place");

			m_place = place;
			m_nRow = nRow;
			m_nCol = nCol;
			m_position = position;

			//
			// 영웅
			//

			m_heroes = new Dictionary<Guid, Hero>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public PhysicalPlace place
		{
			get { return m_place; }
		}

		public int row
		{
			get { return m_nRow; }
		}

		public int col
		{
			get { return m_nCol; }
		}

		public Vector3 position
		{
			get { return m_position; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		//
		// 영웅
		//

		/// <summary>
		/// 영웅 객체 저장 함수
		/// </summary>
		/// <param name="hero">저장 할 영웅 객체</param>
		public void AddHero(Hero hero)
		{
			if (hero == null)
				throw new ArgumentNullException("hero");

			m_heroes.Add(hero.id, hero);
		}

		/// <summary>
		/// 영웅 객체 호출 함수
		/// </summary>
		/// <param name="heroId">호출 할 영웅 ID</param>
		/// <returns>해당하는 영웅 객체 또는 null</returns>
		public Hero? GetHero(Guid heroId)
		{
			Hero? value;

			return m_heroes.TryGetValue(heroId, out value) ? value : null;
		}

		/// <summary>
		/// 영웅 객체 삭제 함수
		/// </summary>
		/// <param name="heroId">삭제 할 영웅 ID</param>
		public void RemoveHero(Guid heroId)
		{
			m_heroes.Remove(heroId);
		}

		/// <summary>
		/// 섹터에 존재하는 영웅 목록 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 섹터의 영웅 리스트</returns>
		public List<Hero> GetHeroes(Guid heroIdToExclude)
		{
			List<Hero> heroes = new List<Hero>();

			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				heroes.Add(hero);
			}

			return heroes;
		}

		/// <summary>
		/// 섹터에 존재하는 영웅 목록 호출 함수
		/// </summary>
		/// <param name="heroes">해당 영웅을 제외한 섹터의 영웅을 저장 할 리스트</param>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		public void GetHeroes(List<Hero> heroes, Guid heroIdToExclude)
		{
			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				heroes.Add(hero);
			}
		}

		/// <summary>
		/// 섹터에 존재하는 영웅들의 ID 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 섹터의 영웅 ID 리스트</returns>
		public List<Guid> GetHeroIds(Guid heroIdToExclude)
		{
			List<Guid> heroIds = new List<Guid>();

			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				heroIds.Add(hero.id);
			}

			return heroIds;
		}

		/// <summary>
		/// 섹터에 존재하는 영웅들의 ID 호출 함수
		/// </summary>
		/// <param name="heroIds">해당 영웅을 제외한 섹터의 영웅 ID를 저장 할 리스트</param>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		public void GetHeroIds(List<Guid> heroIds, Guid heroIdToExclude)
		{
			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				heroIds.Add(hero.id);
			}
		}

		/// <summary>
		/// 섹터에 존재하는 영웅들의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 섹터의 영웅의 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetClientPeers(Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				clientPeers.Add(hero.clientPeer);
			}

			return clientPeers;
		}

		/// <summary>
		/// 섹터에 존재하는 영웅들의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="clientPeers">해당 영웅을 제외한 섹터의 영웅의 클라이언트 피어를 저장할 리스트</param>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		public void GetClientPeers(List<ClientPeer> clientPeers, Guid heroIdToExclude)
		{
			foreach (Hero hero in m_heroes.Values)
			{
				if (hero.id == heroIdToExclude)
					continue;

				clientPeers.Add(hero.clientPeer);
			}
		}
	}
}
