using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 시스템에서 사용되는 리소스 데이터를 호출 및 관리하는 클래스
	/// </summary>
	public class Resource
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		// 시작방향의 타입에 대한 상수
		public const int kStartYRotationType_Fiexed = 1;
		public const int kStartYRotationType_Random = 2;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		//
		// 시작 위치 관련
		//

		private int m_nStartContinentId;
		private Vector3 m_startPosition;
		private float m_fStartRadius;
		private int m_nStartYRotationType;
		private float m_fStartYRotation;

		//
		// 섹터 크기
		//

		private float m_fSectorCellSize;

		//
		// 영웅 생성 제한 수
		//

		private int m_nHeroCreationLimitCount;

		//
		// 캐릭터
		//

		private Dictionary<int, Character> m_characters;

		//
		// 위치
		//

		private Dictionary<int, Location> m_locations;

		//
		// 대륙
		//

		private Dictionary<int, Continent> m_continents;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		private Resource()
		{
			//
			// 시작 위치 관련
			//

			m_nStartContinentId = 0;
			m_startPosition = Vector3.zero;
			m_fStartRadius = 0f;
			m_nStartYRotationType = 0;
			m_fStartYRotation = 0f;

			//
			// 섹터 크기
			//

			m_fSectorCellSize = 0f;

			//
			// 영웅 생성 제한 수
			//

			m_nHeroCreationLimitCount = 0;

			//
			// 캐릭터
			//

			m_characters = new Dictionary<int, Character>();

			//
			// 위치
			//

			m_locations = new Dictionary<int, Location>();

			//
			// 대륙
			//

			m_continents = new Dictionary<int, Continent>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		//
		// 시작 위치 관련
		//
		
		public int startContinentId
		{
			get { return m_nStartContinentId; }
		}

		//
		// 섹터 크기
		//

		public float sectorCellSize
		{
			get { return m_fSectorCellSize; }
		}

		//
		// 영웅 생성 제한 수
		//

		public int heroCreationLimitCount
		{
			get { return m_nHeroCreationLimitCount; }
		}

		//
		// 대륙
		//

		public Dictionary<int,Continent> continents
		{
			get { return m_continents; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		public void Initialize(SqlConnection conn)
		{
			if (conn == null)
				throw new ArgumentNullException("conn");

			SFLogUtil.System(GetType(), "Resource Initialize Started.");

			//
			//
			//

			// 게임 설정 데이터 리소스
			LoadResource_GameConfig(conn);

			// 캐릭터 데이터 리소스
			LoadResource_Character(conn);

			// 장소 데이터 리소스
			LoadResource_Place(conn);

			//
			//
			//

			SFLogUtil.System(GetType(), "Resource Initialize Completed.");
		}

		/// <summary>
		/// 게임 설정 데이터 리소스 설정 함수
		/// </summary>
		/// <param name="conn"></param>
		private void LoadResource_GameConfig(SqlConnection conn)
		{
			DataRow? drGameConfig = UserDBDoc.GameConfig(conn, null);
			if (drGameConfig == null)
			{
				SFLogUtil.Warn(GetType(), "게임설정이 존재하지 않습니다.");
				return;
			}

			//
			// 시작 위치 관련
			//

			m_nStartContinentId = Convert.ToInt32(drGameConfig["startContinentId"]);
			m_startPosition.x = Convert.ToSingle(drGameConfig["startXPosition"]);
			m_startPosition.y = Convert.ToSingle(drGameConfig["startYPosition"]);
			m_startPosition.z = Convert.ToSingle(drGameConfig["startZPosition"]);
			m_fStartRadius = Convert.ToSingle(drGameConfig["startRadius"]);
			m_nStartYRotationType = Convert.ToInt32(drGameConfig["startYRotationType"]);
			if (!IsDefinedStartYRotationType(m_nStartYRotationType))
				SFLogUtil.Warn(GetType(), "시작방향타입이 유효하지 않습니다. m_nStartYRotationType = " + m_nStartYRotationType);

			m_fStartYRotation = Convert.ToSingle(drGameConfig["startYRotation"]);

			//
			// 섹터 크기
			//

			m_fSectorCellSize = Convert.ToSingle(drGameConfig["sectorCellSize"]);

			//
			// 영웅 생성 제한 수
			//

			m_nHeroCreationLimitCount = Convert.ToInt32(drGameConfig["heroCreationLimitCount"]);
		}

		/// <summary>
		/// 캐릭터 데이터 리소스 설정 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		private void LoadResource_Character(SqlConnection conn)
		{
			//
			// 캐릭터 목록
			//

			foreach (DataRow dr in UserDBDoc.Characters(conn, null))
			{
				Character character = new Character();
				character.Set(dr);

				m_characters.Add(character.id, character);
			}

			//
			// 캐릭터 행동 목록
			//

			foreach (DataRow dr in UserDBDoc.CharacterActions(conn, null))
			{
				int nCharacterId = Convert.ToInt32(dr["characterId"]);
				Character? character = GetCharacter(nCharacterId);
				if (character == null)
				{
					SFLogUtil.Warn(GetType(), "[캐릭터행동 목록] 캐릭터가 존재하지 않습니다. nCharacterId = " + nCharacterId);
					continue;
				}

				CharacterAction action = new CharacterAction(character);
				action.Set(dr);

				character.AddAction(action);
			}
		}

		/// <summary>
		/// 장소 데이터 리소스 호출 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		private void LoadResource_Place(SqlConnection conn)
		{
			// 대륙 데이터
			LoadResource_Place_Continent(conn);
		}

		/// <summary>
		/// 대륙 데이터 리소스 호출 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		private void LoadResource_Place_Continent(SqlConnection conn)
		{
			//
			// 대륙 목록
			//

			foreach (DataRow dr in UserDBDoc.Continents(conn, null))
			{
				Continent continent = new Continent();
				continent.Set(dr);

				m_continents.Add(continent.id, continent);

				// 위치 추가
				AddLocation(continent);
			}
		}

		//
		// 시작 위치 관련
		//

		/// <summary>
		/// 랜덤 시작 위치 생성 함수
		/// </summary>
		/// <returns>랜덤하게 생성된 위치 정보 반환</returns>
		public Vector3 SelectStartPosition()
		{
			return new Vector3(
				RandomUtil.NextFloat(m_startPosition.x - m_fStartRadius, m_startPosition.x + m_fStartRadius),
				m_startPosition.y,
				RandomUtil.NextFloat(m_startPosition.z - m_fStartRadius, m_startPosition.z + m_fStartRadius));
		}

		/// <summary>
		/// 타입에 따른 시작 방향 생성 함수
		/// </summary>
		/// <returns>타입에 맞는 랜덤 또는 고정 방향 반환</returns>
		public float SelectStartYRotation()
		{
			return m_nStartYRotationType == kStartYRotationType_Fiexed ? m_fStartYRotation : RandomUtil.NextFloat(m_fStartYRotation);
		}

		//
		// 캐릭터
		//

		/// <summary>
		/// 캐릭터 데이터 호출 함수
		/// </summary>
		/// <param name="nId">캐릭터 ID</param>
		/// <returns>해당 캐릭터 데이터 또는 null 반환</returns>
		public Character? GetCharacter(int nId)
		{
			Character? value;

			return m_characters.TryGetValue(nId, out value) ? value : null;
		}

		//
		// 위치
		//

		/// <summary>
		/// 위치 추가 함수
		/// </summary>
		/// <param name="location">추가 할 위치 객체</param>
		private void AddLocation(Location location)
		{
			m_locations.Add(location.locationId, location);
		}

		/// <summary>
		/// 위치 호출 함수
		/// </summary>
		/// <param name="nLocationId">호출 할 위치 ID</param>
		/// <returns>해당 위치 데이터 또는 null 반환</returns>
		public Location? GetLocation(int nLocationId)
		{
			Location? value;

			return m_locations.TryGetValue(nLocationId, out value) ? value : null;
		}

		//
		// 대륙
		//

		/// <summary>
		/// 대륙 데이터 호출 함수
		/// </summary>
		/// <param name="nId">대륙 ID</param>
		/// <returns>해당 대륙 데이터 또는 null 반환</returns>
		public Continent? GetContinent(int nId)
		{
			Continent? value;

			return m_continents.TryGetValue(nId, out value) ? value : null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static Resource s_instance;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructors

		/// <summary>
		/// 
		/// </summary>
		static Resource()
		{
			s_instance = new Resource();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static Resource instance
		{
			get { return s_instance; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 시작방향타입의 유효성 확인 함수
		/// </summary>
		/// <param name="nType">시작방향타입</param>
		/// <returns>유효할 경우 true, 유효하지 않을경우 false 반환</returns>
		public static bool IsDefinedStartYRotationType(int nType)
		{
			return nType == kStartYRotationType_Fiexed
				|| nType == kStartYRotationType_Random;
		}
	}
}
