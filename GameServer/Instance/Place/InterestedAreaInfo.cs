using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 관심 영역 정보 클래스
	/// </summary>
	public class InterestedAreaInfo
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private HashSet<Sector> m_notChangedSectors;
		private HashSet<Sector> m_addedSectors;
		private HashSet<Sector> m_removedSectors;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public InterestedAreaInfo()
		{
			m_notChangedSectors = new HashSet<Sector>();
			m_addedSectors = new HashSet<Sector>();
			m_removedSectors = new HashSet<Sector>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public HashSet<Sector> notChangedSectors
		{
			get { return m_notChangedSectors; }
		}

		public HashSet<Sector> addedSectors
		{
			get { return m_addedSectors; }
		}

		public HashSet<Sector> removedSectors
		{
			get { return m_removedSectors; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 변경되지 않은 섹터 저장 함수
		/// </summary>
		/// <param name="sector">변경되지 않은 섹터 객체</param>
		public void AddNotChangedSector(Sector sector)
		{
			if (sector == null)
				throw new ArgumentNullException("sector");

			m_notChangedSectors.Add(sector);
		}

		/// <summary>
		/// 추가 된 섹터 저장 함수
		/// </summary>
		/// <param name="sector">추가 된 섹터 객체</param>
		public void AddAddedSector(Sector sector)
		{
			if (sector == null)
				throw new ArgumentNullException("sector");

			m_addedSectors.Add(sector);
		}

		/// <summary>
		/// 삭제 된 섹터 저장 함수
		/// </summary>
		/// <param name="sector">삭제 된 섹터 객체</param>
		public void AddRemovedSector(Sector sector)
		{
			if (sector == null)
				throw new ArgumentNullException("sector");

			m_removedSectors.Add(sector);
		}

		/// <summary>
		/// 매개변수로 전달 된 섹터가 변경되지 않은 섹터에 존재하는지 확인하는 함수
		/// </summary>
		/// <param name="sector">확인 할 섹터 객체</param>
		/// <returns>변경되지 않은 섹터에 존재할 경우 true, 존재하지 않을 경우 false 반환</returns>
		public bool ContainsNotChangedSector(Sector sector)
		{
			if (sector == null)
				return false;

			return m_notChangedSectors.Contains(sector);
		}

		//
		//
		//

		/// <summary>
		/// 추가 된 섹터의 영웅 목록 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 추가 된 섹터의 영웅 리스트</returns>
		public List<Hero> GetAddedSectorHeroes(Guid heroIdToExclude)
		{
			List<Hero> addedHeroes = new List<Hero>();

			foreach (Sector sector in m_addedSectors)
			{
				sector.GetHeroes(addedHeroes, heroIdToExclude);
			}

			return addedHeroes;
		}

		//
		//
		//

		/// <summary>
		/// 삭제 된 섹터의 영웅들의 ID 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 삭제 된 섹터의 영웅 ID 리스트</returns>
		public List<Guid> GetRemovedSectorHeroIds(Guid heroIdToExclude)
		{
			List<Guid> heroIds = new List<Guid>();

			foreach (Sector sector in m_removedSectors)
			{
				sector.GetHeroIds(heroIds, heroIdToExclude);
			}

			return heroIds;
		}

		//
		//
		//

		/// <summary>
		/// 변경되지 않은 섹터의 영웅들의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 변경되지 않은 섹터의 영웅들의 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetNotChangedSectorClientPeers(Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Sector sector in m_notChangedSectors)
			{
				sector.GetClientPeers(clientPeers, heroIdToExclude);
			}

			return clientPeers;
		}

		/// <summary>
		/// 추가 된 섹터의 영웅들의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 추가 된 섹터의 영웅들의 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetAddedSectorClientPeers(Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Sector sector in m_addedSectors)
			{
				sector.GetClientPeers(clientPeers, heroIdToExclude);
			}

			return clientPeers;
		}

		/// <summary>
		/// 삭제 된 섹터의 영웅들의 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 삭제 된 섹터의 영웅들의 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetRemovedSectorClientPeers(Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Sector sector in m_removedSectors)
			{
				sector.GetClientPeers(clientPeers, heroIdToExclude);
			}

			return clientPeers;
		}
	}
}
