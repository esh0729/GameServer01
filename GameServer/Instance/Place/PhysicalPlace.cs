using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)물리적으로 존재하는 장소에 대한 정보를 관리하는 클래스
	/// </summary>
	public abstract class PhysicalPlace : Place
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected Sector[,]? m_sectors;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public abstract Location location { get; }
		public abstract Rect3D rect { get; }

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected void InitPhysicalPlace()
		{
			float fSectorCellSize = Resource.instance.sectorCellSize;

			float fWidth = rect.xSize - rect.x;
			float fHeight = rect.zSize - rect.z;

			int nRowCount = (int)(fHeight / fSectorCellSize) + 1;
			int nColCount = (int)(fWidth / fSectorCellSize) + 1;

			m_sectors = new Sector[nRowCount, nColCount];
			for (int nRow = 0; nRow < nRowCount; nRow++)
			{
				for (int nCol = 0; nCol < nColCount; nCol++)
				{
					Sector sector = new Sector(this, nRow, nCol, new Vector3(rect.x + (nRow * fSectorCellSize), 0, rect.z + (nRow * fSectorCellSize)));

					m_sectors[nRow, nCol] = sector;
				}
			}

			//
			//
			//

			InitPlace();
		}

		/// <summary>
		/// 현재 장소 관리 영역 내에 존재하는지 확인하는 함수
		/// </summary>
		/// <param name="position">위치 정보</param>
		/// <returns>관리 역역 내에 존재 할 경우 true, 존재하지 않을 경우 false 반환</returns>
		public bool ContainsPosition(Vector3 position)
		{
			return rect.Contains(position);
		}

		//
		// 섹터
		//

		/// <summary>
		/// 섹터 호출 함수
		/// </summary>
		/// <param name="position">위치 정보</param>
		/// <returns>해당 섹터 객체 또는 null 반환</returns>
		protected Sector? GetSector(Vector3 position)
		{
			float fSectorCellSize = Resource.instance.sectorCellSize;

			float fX = position.x - rect.x;
			float fZ = position.z - rect.z;

			int nRow = (int)(fZ / fSectorCellSize);
			int nCol = (int)(fX / fSectorCellSize);

			return GetSector(nRow, nCol);
		}

		/// <summary>
		/// 섹터 호출 함수
		/// </summary>
		/// <param name="nRow">행 번호</param>
		/// <param name="nCol">열 번호</param>
		/// <returns>해당 섹터 객체 또는 null 반환</returns>
		protected Sector? GetSector(int nRow, int nCol)
		{
			if (nRow < 0 || nRow >= m_sectors!.GetLength(0))
				return null;

			if (nCol < 0 || nCol >= m_sectors.GetLength(1))
				return null;

			return m_sectors[nRow, nCol];
		}

		/// <summary>
		/// 관심 영역 섹터 호출 함수
		/// </summary>
		/// <param name="standardSector">기준 섹터 객체</param>
		/// <returns>기준 섹터의 관심 영역 섹터 리스트</returns>
		protected List<Sector> GetInterestedSector(Sector standardSector)
		{
			List<Sector> sectors = new List<Sector>();

			for (int nRow = standardSector.row - 1; nRow <= standardSector.row + 1; nRow++)
			{
				for (int nCol = standardSector.col - 1; nCol <= standardSector.col + 1; nCol++)
				{
					Sector? sector = GetSector(nRow, nCol);

					if (sector == null)
						continue;

					sectors.Add(sector);
				}
			}

			return sectors;
		}

		/// <summary>
		/// 관심 영역 섹터에 존재하는 영웅 호출 함수
		/// </summary>
		/// <param name="standardSector">기준 섹터 객체</param>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 관심 영역 섹터의 영웅 리스트</returns>
		public List<Hero> GetInterestedHeroes(Sector standardSector, Guid heroIdToExclude)
		{
			List<Hero> heroes = new List<Hero>();

			foreach (Sector sector in GetInterestedSector(standardSector))
			{
				foreach (Hero hero in sector.GetHeroes(heroIdToExclude))
				{
					heroes.Add(hero);
				}
			}

			return heroes;
		}

		/// <summary>
		/// 관심 영역 섹터에 존재하는 영웅 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="standardSector">기준 섹터 객체</param>
		/// <param name="heroIdToExclude">제외 할 영웅 ID</param>
		/// <returns>해당 영웅을 제외한 관심 영역 섹터의 영웅 클라이언트 피어 리스트</returns>
		public List<ClientPeer> GetInterestedClientPeers(Sector standardSector, Guid heroIdToExclude)
		{
			List<ClientPeer> clientPeers = new List<ClientPeer>();

			foreach (Sector sector in GetInterestedSector(standardSector))
			{
				sector.GetClientPeers(clientPeers, heroIdToExclude);
			}

			return clientPeers;
		}

		//
		// 영웅
		//

		/// <summary>
		/// 영웅 입장 처리 시 호출되는 함수(장소에 영웅이 추가 전에 호출됨)
		/// </summary>
		/// <param name="hero">입장 영웅 객체</param>
		protected override void OnHeroEntering(Hero hero)
		{
			base.OnHeroEntering(hero);

			// 영웅의 장소 설정
			hero.SetCurrentPlace(this);
			// 영웅의 위치 및 방향 설정
			hero.SetSector(GetSector(hero.position));
		}

		/// <summary>
		/// 영웅 입장 완료 시 호출되는 함수(장소에 영웅이 추가 이후 호출됨)
		/// </summary>
		/// <param name="hero">입장 영웅 객체</param>
		protected override void OnHeroEnter(Hero hero)
		{
			base.OnHeroEnter(hero);

			// 다른 영웅 입장 이벤트 송신
			ServerEvent.SendHeroEnterEvent(GetInterestedClientPeers(hero.sector!, hero.id), hero.ToPDHero());
		}

		/// <summary>
		/// 영웅 퇴장 완료 시 호출되는 함수
		/// </summary>
		/// <param name="hero">퇴장 영웅 객체</param>
		/// <param name="bIsLogout">로그아웃 여부</param>
		/// <param name="entranceParam">다음 입장 장소 정보</param>
		protected override void OnHeroExit(Hero hero, bool bIsLogout, EntranceParam? entranceParam)
		{
			base.OnHeroExit(hero, bIsLogout, entranceParam);

			// 이동 종료
			hero.EndMove();
			// 행동 종료
			hero.EndAction();

			// 다른 영웅 퇴장 이벤트 송신
			ServerEvent.SendHeroExitEvent(GetInterestedClientPeers(hero.sector!, hero.id), hero.id);

			// 영웅 장소 null로 설정
			hero.SetCurrentPlace(null);
			// 영웅 섹터 null로 설정
			hero.SetSector(null);
			// 입장 장소 정보 설정
			hero.SetEntranceParam(entranceParam);
		}

		/// <summary>
		/// 영웅 이동 처리 함수
		/// </summary>
		/// <param name="hero">이동 할 영웅 객체</param>
		/// <param name="position">좌표</param>
		/// <param name="fYRotation">방향</param>
		/// <param name="bSendInterestedChangedEvent">관심 영역 변경 이벤트 송신 여부</param>
		/// <returns>관심 영역 정보 객체</returns>
		public InterestedAreaInfo MoveHero(Hero hero, Vector3 position, float fYRotation, bool bSendInterestedChangedEvent)
		{
			if (hero == null)
				throw new ArgumentNullException("hero");

			InterestedAreaInfo info = ChangeHeroPosition(hero, position, fYRotation, bSendInterestedChangedEvent);

			// 영웅 이동 이후 처리
			hero.OnMove();

			// 영웅 이동 서버 이벤트 송신
			ServerEvent.SendHeroMoveEvent(info.GetNotChangedSectorClientPeers(hero.id), hero.id, position, fYRotation);

			//
			//
			//

			return info;
		}

		/// <summary>
		/// 영웅 좌표 변경 처리 함수
		/// </summary>
		/// <param name="hero">좌표 변경 할 영웅 객체</param>
		/// <param name="position">좌표</param>
		/// <param name="fYRotation">방향</param>
		/// <param name="bSendInterestedChangedEvent">관심 영역 변경 이벤트 송신 여부</param>
		/// <returns>관심 영역 정보 객체</returns>
		public InterestedAreaInfo ChangeHeroPosition(Hero hero, Vector3 position, float fYRotation, bool bSendInterestedChangedEvent)
		{
			// 현재 섹터
			Sector oldSector = hero.sector!;
			// 이동 이후 섹터
			Sector newSector = GetSector(position)!;

			// 관심 역역 정보 호출
			InterestedAreaInfo info = GetInterestedAreaInfo(oldSector, newSector);

			// 영웅 이동 처리
			hero.SetPosition(position, fYRotation);

			if (oldSector != newSector)
			{
				hero.SetSector(newSector);

				// 관심 영역 변경 이벤트 송신
				if (bSendInterestedChangedEvent)
				{
					ServerEvent.SendInterestedAreaChangedEvent(hero.clientPeer,
						Hero.ToPDHeroes(info.GetAddedSectorHeroes(hero.id)).ToArray(), info.GetRemovedSectorHeroIds(hero.id).ToArray());
				}

				// 추가 된 관심 영역 영웅들에게 영웅 관심 영역 입장 이벤트 송신
				ServerEvent.SendHeroInterestedAreaEnterEvent(info.GetAddedSectorClientPeers(hero.id), hero.ToPDHero());

				// 삭제 된 관심 영역 영웅들에게 영웅 관심 영역 퇴장 이벤트 송신
				ServerEvent.SendHeroInterestedAreaExitEvent(info.GetRemovedSectorClientPeers(hero.id), hero.id);
			}

			//
			//
			//

			return info;
		}

		/// <summary>
		/// 관심 영역 정보 처리 함수
		/// </summary>
		/// <param name="oldSector">이전 섹터 객체</param>
		/// <param name="newSector">이후 섹터 객체</param>
		/// <returns>관심 영역 정보 객체</returns>
		private InterestedAreaInfo GetInterestedAreaInfo(Sector oldSector, Sector newSector)
		{
			InterestedAreaInfo info = new InterestedAreaInfo();

			// 이전 섹터와 이후 섹터가 같을 경우 모든 관심 영역 섹터를 변경되지 않는 섹터 처리
			if (oldSector == newSector)
			{
				foreach (Sector sector in GetInterestedSector(oldSector))
				{
					info.AddNotChangedSector(sector);
				}
			}
			else
			{
				List<Sector> oldSectorInterestedSectors = GetInterestedSector(oldSector);
				List<Sector> newSectorInterestedSectors = GetInterestedSector(newSector);

				//
				// NotChangedSector
				//

				// 이전 관심 영역 섹터와 이후 관심 영역 섹터 비교하여 변경되지 않은 섹터 처리
				foreach (Sector oldSectorInterestedSector in oldSectorInterestedSectors)
				{
					foreach (Sector newSectorInterestedSector in newSectorInterestedSectors)
					{
						if (oldSectorInterestedSector == newSectorInterestedSector)
						{
							info.AddNotChangedSector(oldSectorInterestedSector);
							break;
						}
					}
				}

				//
				// AddedSector
				//

				// 이후 관심 영역 섹터 검사 하여 추가 된 섹터 처리
				foreach (Sector newSectorInterestedSector in newSectorInterestedSectors)
				{
					if (info.ContainsNotChangedSector(newSectorInterestedSector))
						continue;

					info.AddAddedSector(newSectorInterestedSector);
				}

				//
				// RemovedSector
				//

				// 이전 관심 영역 섹터 검사 하여 삭제 된 섹터 처리
				foreach (Sector oldSectorInterestedSector in oldSectorInterestedSectors)
				{
					if (info.ContainsNotChangedSector(oldSectorInterestedSector))
						continue;

					info.AddRemovedSector(oldSectorInterestedSector);
				}
			}

			return info;
		}
	}
}
