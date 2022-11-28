using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// (추상 클래스)데이터 송수신에 처리될 데이터의 직렬화, 역직렬화 함수를 지원하는 클래스
	/// </summary>
	public abstract class Body
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화에 필요한 처리를 시작하는 함수
		/// </summary>
		/// <param name="buffer">송신 버퍼</param>
		/// <param name="nOffset">버퍼 현재 위치</param>
		public int SerializeRaw(byte[] sendBuffer, int nPosition)
		{
			Buffer buffer = new Buffer(sendBuffer, nPosition);
			PacketWriter writer = new PacketWriter(buffer);

			Serialize(writer);

			return buffer.position;
		}

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected virtual void Serialize(PacketWriter writer)
		{
		}

		/// <summary>
		/// 역직렬화에 필요한 처리를 시작하는 함수
		/// </summary>
		/// <param name="buffer">수신 버퍼</param>
		/// <param name="nPosition">버퍼 현재 위치</param>
		public void DeserializeRaw(byte[] receivebuffer, int nPosition)
		{
			Buffer buffer = new Buffer(receivebuffer, nPosition);
			PacketReader reader = new PacketReader(buffer);

			Deserialize(reader);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected virtual void Deserialize(PacketReader reader)
		{
		}
	}
}
