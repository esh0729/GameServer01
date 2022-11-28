using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 송수신 데이터를 관리하는 클래스
	/// </summary>
	public class Buffer
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private byte[] m_buffer;

		private int m_nPosition;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">버퍼</param>
		/// <param name="nPosition">버퍼 현재 위치</param>
		public Buffer(byte[] buffer, int nPosition)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			m_buffer = buffer;

			m_nPosition = nPosition;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public byte[] buffer
		{
			get { return m_buffer; }
		}

		public int position
		{
			get { return m_nPosition; }
			set { m_nPosition = value; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		//
		// Read
		//

		/// <summary>
		/// 버퍼에서 byte 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 byte 타입 데이터 반환</returns>
		public byte PopByte()
		{
			return m_buffer[m_nPosition++];
		}

		/// <summary>
		/// 버퍼에서 short 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 short 타입 데이터 반환</returns>
		public short PopInt16()
		{
			return (short)(m_buffer[m_nPosition++] | (m_buffer[m_nPosition++] << 8));
		}

		/// <summary>
		/// 버퍼에서 int 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 int 타입 데이터 반환</returns>
		public int PopInt32()
		{
			return m_buffer[m_nPosition++] | (m_buffer[m_nPosition++] << 8) | (m_buffer[m_nPosition++] << 16) | (m_buffer[m_nPosition++] << 24);
		}

		/// <summary>
		/// 버퍼에서 long 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 long 타입 데이터 반환</returns>
		public long PopInt64()
		{
			return (long)(m_buffer[m_nPosition++] | (m_buffer[m_nPosition++] << 8) | (m_buffer[m_nPosition++] << 16) | (m_buffer[m_nPosition++] << 24) |
					(m_buffer[m_nPosition++] << 32) | (m_buffer[m_nPosition++] << 40) | (m_buffer[m_nPosition++] << 48) | (m_buffer[m_nPosition++] << 56));
		}

		/// <summary>
		/// 버퍼에서 byte 타입의 데이터를 꺼내 0이 아닐 경우 true, 0일 경우 false를 반환하는 함수
		/// </summary>
		/// <returns>bool 타입의 데이터 반환</returns>
		public bool PopBoolean()
		{
			return PopByte() != 0 ? true : false;
		}

		/// <summary>
		/// 버퍼에서 float 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 float 타입 데이터 반환</returns>
		public float PopSingle()
		{
			float fValue = BitConverter.ToSingle(m_buffer, m_nPosition);
			m_nPosition += sizeof(float);

			return fValue;
		}

		/// <summary>
		/// 버퍼에서 특정 길이의 string 형태의 데이터를 꺼내오는 함수
		/// </summary>
		/// <returns>현재 위치의 string 타입 데이터 반환</returns>
		public string? PopString()
		{
			if (!PopBoolean())
				return null;

			int nLength = PopInt16();

			string sValue = Encoding.UTF8.GetString(m_buffer, m_nPosition, nLength);
			m_nPosition += nLength;

			return sValue;
		}

		//
		// Write
		//

		/// <summary>
		/// byte 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">byte 타입 데이터</param>
		public void Push(byte value)
		{
			m_buffer[m_nPosition++] = value;
		}

		/// <summary>
		/// short 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">short 타입 데이터</param>
		public void Push(short value)
		{
			m_buffer[m_nPosition++] = (byte)value;
			m_buffer[m_nPosition++] = (byte)(value >> 8);
		}

		/// <summary>
		/// int 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">int 타입 데이터</param>
		public void Push(int value)
		{
			m_buffer[m_nPosition++] = (byte)value;
			m_buffer[m_nPosition++] = (byte)(value >> 8);
			m_buffer[m_nPosition++] = (byte)(value >> 16);
			m_buffer[m_nPosition++] = (byte)(value >> 24);
		}

		/// <summary>
		/// long 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">long 타입 데이터</param>
		public void Push(long value)
		{
			m_buffer[m_nPosition++] = (byte)value;
			m_buffer[m_nPosition++] = (byte)(value >> 8);
			m_buffer[m_nPosition++] = (byte)(value >> 16);
			m_buffer[m_nPosition++] = (byte)(value >> 24);
			m_buffer[m_nPosition++] = (byte)(value >> 32);
			m_buffer[m_nPosition++] = (byte)(value >> 40);
			m_buffer[m_nPosition++] = (byte)(value >> 48);
			m_buffer[m_nPosition++] = (byte)(value >> 56);
		}

		/// <summary>
		/// bool 형태의 데이터를 저장하는 함수
		/// </summary>
		/// <param name="value">bool 타입 데이터</param>
		public void Push(bool value)
		{
			Push((byte)(value ? 1 : 0));
		}

		/// <summary>
		/// float 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">float 타입 데이터</param>
		public void Push(float value)
		{
			// float 데이터 byte[]로 변환
			byte[] temp = BitConverter.GetBytes(value);
			// 변환한 byte[]을 버퍼의 현재 위치에서 부터 복사
			temp.CopyTo(m_buffer, m_nPosition);
			// 현재위치 변환한 byte[]의 길이만큼 증가
			m_nPosition += temp.Length;
		}

		/// <summary>
		/// string 형태의 데이터를 버퍼의 현재 위치에 저장하는 함수
		/// </summary>
		/// <param name="value">string 타입 데이터</param>
		public void Push(string? value)
		{
			if (value == null)
			{
				Push(false);
				return;
			}

			Push(true);

			// string 데이터를 UTF8 인코딩으로 byte[] 변환
			byte[] temp = Encoding.UTF8.GetBytes(value);
			// byte[] 의 길이 저장
			Push((short)temp.Length);

			// 변환한 byte[]을 버퍼의 현재 위치에서 부터 복사
			temp.CopyTo(m_buffer, m_nPosition);
			// 현재위치 변환한 byte[]의 길이만큼 증가
			m_nPosition += temp.Length;
		}
	}
}
