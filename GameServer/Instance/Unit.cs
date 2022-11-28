using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)영웅 또는 NPC의 기본 데이터를 관리하는 클래스
	/// </summary>
	public abstract class Unit
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected PhysicalPlace? m_currentPlace;
		protected Sector? m_sector;
		protected Vector3 m_position;
		protected float m_fYRotation;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public PhysicalPlace? currentPlace
		{
			get { return m_currentPlace; }
		}

		public Sector? sector
		{
			get { return m_sector; }
		}

		public Vector3 position
		{
			get { return m_position; }
		}

		public float yRotation
		{
			get { return m_fYRotation; }
		}

		public virtual float moveSpeed
		{
			get { return 0f; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 현재 장소 설정 함수
		/// </summary>
		/// <param name="place"></param>
		public void SetCurrentPlace(PhysicalPlace? place)
		{
			m_currentPlace = place;
		}

		/// <summary>
		/// 현재 위치와 방향 설정 함수
		/// </summary>
		/// <param name="position">위치 정보</param>
		/// <param name="fYRotation">방향</param>
		public void SetPosition(Vector3 position, float fYRotation)
		{
			m_position = position;
			m_fYRotation = fYRotation;
		}

		/// <summary>
		/// 섹터 설정 함수
		/// </summary>
		/// <param name="sector">설정 할 섹터 객체</param>
		public void SetSector(Sector? sector)
		{
			Sector? oldSector = m_sector;
			m_sector = sector;

			OnSetSector(oldSector);
		}

		/// <summary>
		/// 섹터 설정 완료 이후 호출 되는 함수
		/// </summary>
		/// <param name="oldSector">이전 섹터 객체</param>
		protected virtual void OnSetSector(Sector? oldSector)
		{

		}
	}
}
