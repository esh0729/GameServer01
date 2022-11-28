using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 버퍼 데이터를 읽어 역직렬화 하는 기능을 제공하는 클래스
	/// </summary>
	public class BufferReader
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Buffer m_buffer;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">수신 버퍼</param>
		public BufferReader(Buffer buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			m_buffer = buffer;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 버퍼에서 byte 형태의 데이터를 꺼내와서 0이 아닐 경우 true, 0일 경우 false를 반환 하는 함수
		/// </summary>
		/// <returns>boolean 타입 데이터 반환</returns>
		public bool ReadBoolean()
		{
			return m_buffer.PopByte() != 0;
		}

		/// <summary>
		/// 버퍼에서 byte 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>byte 타입 데이터 반환</returns>
		public byte ReadByte()
		{
			return m_buffer.PopByte();
		}

		/// <summary>
		/// 버퍼에서 short 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>short 타입 데이터 반환</returns>
		public short ReadInt16()
		{
			return m_buffer.PopInt16();
		}

		/// <summary>
		/// 버퍼에서 int 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>int 타입 데이터 반환</returns>
		public int ReadInt32()
		{
			return m_buffer.PopInt32();
		}

		/// <summary>
		/// 버퍼에서 long 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>long 타입 데이터 반환</returns>
		public long ReadInt64()
		{
			return m_buffer.PopInt64();
		}

		/// <summary>
		/// 버퍼에서 float 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>float 타입 데이터 반환</returns>
		public float ReadSingle()
		{
			return m_buffer.PopSingle();
		}

		/// <summary>
		/// 버퍼에서 특정 길이의 string 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>string 타입 데이터 반환</returns>
		public string? ReadString()
		{
			return m_buffer.PopString();
		}
	}
}
