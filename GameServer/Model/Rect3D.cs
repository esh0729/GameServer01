using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 3차원 육면체의 정보를 가지고 있는 구조체
	/// </summary>
	public struct Rect3D
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public float x;
		public float y;
		public float z;
		public float xSize;
		public float ySize;
		public float zSize;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">x 좌표</param>
		/// <param name="y">y 좌표</param>
		/// <param name="z">z 좌표</param>
		/// <param name="xSize">x축 길이</param>
		/// <param name="ySize">y축 길이</param>
		/// <param name="zSize">z축 길이</param>
		public Rect3D(float x, float y, float z, float xSize, float ySize, float zSize)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.xSize = xSize;
			this.ySize = ySize;
			this.zSize = zSize;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 육면체 내부에 포함되는 위치인지 확인하는 함수
		/// </summary>
		/// <param name="position">확인 할 좌표</param>
		/// <returns>내부에 포함될 경우 true, 포함되지 않을 경우 false 반환</returns>
		public bool Contains(Vector3 position)
		{
			return x <= position.x && x + xSize > position.x
				&& y <= position.y && y + ySize > position.y
				&& z <= position.z && z + zSize > position.z;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		public static readonly Rect3D zero = new Rect3D(0, 0, 0, 0, 0, 0);
	}
}
