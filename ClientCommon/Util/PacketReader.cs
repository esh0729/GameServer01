using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 버퍼 데이터를 읽어 역직렬화 하는 확장 기능을 제공하는 클래스
	/// </summary>
	public class PacketReader : BufferReader
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">수신 버퍼</param>
		public PacketReader(Buffer buffer)
			: base(buffer)
		{

		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 버퍼에서 Guid 데이터를 역직렬화 하는 함수
		/// </summary>
		/// <returns>역직렬화 된 Guid 구조체</returns>
		public Guid ReadGuid()
		{
			return Guid.Parse(ReadString()!);
		}

		/// <summary>
		/// 버퍼에서 Guid 배열 데이털르 역직렬화 하는 함수
		/// </summary>
		/// <returns>역직렬화 된 Guid 배열 또는 null</returns>
		public Guid[]? ReadGuids()
		{
			if (!ReadBoolean())
				return null;

			int nLength = ReadInt32();

			Guid[] guids = new Guid[nLength];
			for (int i = 0; i < nLength; i++)
			{
				guids[i] = ReadGuid();
			}

			return guids;
		}

		/// <summary>
		/// 버퍼에서 3차원 위치 정보를 역직렬화 하는 함수
		/// </summary>
		/// <returns>역직렬화 된 3차원 위치 정보 구조체</returns>
		public PDVector3 ReadPDVector3()
		{
			PDVector3 vector3 = new PDVector3();
			vector3.x = ReadSingle();
			vector3.y = ReadSingle();
			vector3.z = ReadSingle();

			return vector3;
		}

		/// <summary>
		/// 버퍼에서 PacketData 데이터를 역직렬화 하는 함수
		/// </summary>
		/// <returns>역직렬화 된 PacketData 객체 또는 null</returns>
		public T? ReadPacketData<T>() where T : PacketData
		{
			if (!ReadBoolean())
				return null;

			T packetData = Activator.CreateInstance<T>();
			packetData.DeserializeRaw(this);

			return packetData;
		}

		/// <summary>
		/// 버퍼에서 PacketData 배열을 역직렬화 하는 함수
		/// </summary>
		/// <returns>역직렬화 된 PacketData 배열 또는 null</returns>
		public T[]? ReadPacketDatas<T>() where T : PacketData
		{
			if (!ReadBoolean())
				return null;

			int nLength = ReadInt32();
			T[] packetDatas = new T[nLength];

			for (int i = 0; i < nLength; i++)
			{
				packetDatas[i] = ReadPacketData<T>()!;
			}

			return packetDatas;
		}
	}
}
