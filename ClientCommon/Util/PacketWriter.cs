using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 버퍼에 데이터를 직렬화 하는 확장 함수를 제공하는 클래스
	/// </summary>
	public class PacketWriter : BufferWriter
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">송신 버퍼</param>
		public PacketWriter(Buffer buffer)
			: base(buffer)
		{

		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// Guid 데이터를 직렬화 하여 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="guid">송신 할 Guid 구조체</param>
		public void Write(Guid guid)
		{
			Write(guid.ToString());
		}

		/// <summary>
		/// Guid 배열 데이터를 직렬화 하여 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="guids"></param>
		public void Write(Guid[]? guids)
		{
			if (guids == null)
			{
				Write(false);
				return;
			}

			Write(true);

			int nLength = guids.Length;
			Write(nLength);

			for (int i = 0; i < nLength; i++)
			{
				Write(guids[i]);
			}
		}

		/// <summary>
		/// 3차원 위치 정보를 직렬화 하여 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="vector3">3차원 위치 정보 구조체</param>
		public void Write(PDVector3 vector3)
		{
			Write(vector3.x);
			Write(vector3.y);
			Write(vector3.z);
		}

		/// <summary>
		/// PacketData 데이터를 직렬화 하여 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="packetData">송신 할 packetData 객체</param>
		public void Write(PacketData? packetData)
		{
			if (packetData == null)
			{
				Write(false);
				return;
			}

			Write(true);

			packetData.SerializeRaw(this);
		}


		/// <summary>
		/// PacketData 배열을 직렬화 하여 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="packetDatas"></param>
		public void Write(PacketData[]? packetDatas)
		{
			if (packetDatas == null)
			{
				Write(false);
				return;
			}

			Write(true);

			int nLength = packetDatas.Length;
			Write(nLength);

			for (int i = 0; i < nLength; i++)
			{
				Write(packetDatas[i]);
			}
		}
	}
}
