using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 버퍼에 데이터를 저장하는 기능을 제공하는 클래스
	/// </summary>
	public class BufferWriter
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Buffer m_buffer;
		private byte[] m_temp;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">송신 버퍼</param>
		public BufferWriter(Buffer buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			m_buffer = buffer;
			m_temp = new byte[8];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 버퍼에 bool 타입 데이터 저장 함수
		/// </summary>
		/// <param name="value">boolean 타입 데이터</param>
		public void Write(bool value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 byte 타입 데이터 저장 함수
		/// </summary>
		/// <param name="value">byte 타입 데이터</param>
		public void Write(byte value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 short 타입 데이터 저장 함수
		/// </summary>
		/// <param name="value">short 타입 데이터</param>
		public void Write(short value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 int 타입의 데이터 저장 함수
		/// </summary>
		/// <param name="value">int 타입 데이터</param>
		public void Write(int value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 long 타입의 데이터 저장 함수
		/// </summary>
		/// <param name="value">long 타입 데이터</param>
		public void Write(long value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 float 타입의 데이터 저장 함수
		/// </summary>
		/// <param name="value">float 타입 데이터</param>
		public void Write(float value)
		{
			m_buffer.Push(value);
		}

		/// <summary>
		/// 버퍼에 string 타입의 데이터 저장 함수
		/// </summary>
		/// <param name="value">string 타입 데이터</param>
		public void Write(string? value)
		{
			m_buffer.Push(value);
		}
	}
}
