using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 3차원 위치 정보를 담고 있는 구조체
	/// </summary>
	public struct Vector3
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public float x;
		public float y;
		public float z;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">x 좌표</param>
		/// <param name="y">y 좌표</param>
		/// <param name="z">z 좌표</param>
		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// string 변환시 출력 값 설정 함수
		/// </summary>
		/// <returns>변환 된 string 타입 반환</returns>
		public override string ToString()
		{
			return String.Format("({0}, {1}, {2})", x, y, z);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		public static readonly Vector3 zero = new Vector3(0, 0, 0);

		public static implicit operator PDVector3(Vector3 vector3) => new PDVector3(vector3.x, vector3.y, vector3.z);
		public static implicit operator Vector3(PDVector3 vector3) => new Vector3(vector3.x, vector3.y, vector3.z);
	}
}
