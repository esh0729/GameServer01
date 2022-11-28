using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// 서버 이벤트 데이터를 송신할 때 사용되는 클래스
	/// </summary>
	public class SFEventData : ISFMessage
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nName;
		private Packet m_packet;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nName">서버 이벤트 타입</param>
		/// <param name="packet">송신 패킷</param>
		private SFEventData(int nName, Packet packet)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			m_nName = nName;
			m_packet = packet;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public MessageType messageType
		{
			get { return MessageType.EventData; }
		}

		public Packet packet
		{
			get { return m_packet; }
		}

		public byte[] packetBuffer
		{
			get { return m_packet.buffer; }
		}

		public int packetPosition
		{
			get { return m_packet.position; }
			set { m_packet.position = value; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		private void Initialize()
		{
			m_packet.Push((byte)messageType);
			m_packet.Push(m_nName);
		}

		/// <summary>
		/// byte 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">byte 타입 데이터</param>
		public void Push(byte value)
		{
			m_packet.Push(value);
		}

		/// <summary>
		/// int 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">int 타입 데이터</param>
		public void Push(int value)
		{
			m_packet.Push(value);
		}

		/// <summary>
		/// long 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">long 타입 데이터</param>
		public void Push(long value)
		{
			m_packet.Push(value);
		}

		/// <summary>
		/// bool 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">bool 타입 데이터</param>
		public void Push(bool value)
		{
			m_packet.Push(value);
		}

		/// <summary>
		/// float 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">float 타입 데이터</param>
		public void Push(float value)
		{
			m_packet.Push(value);
		}

		/// <summary>
		/// string 타입의 데이터를 송신 버퍼에 저장하는 함수
		/// </summary>
		/// <param name="value">string 타입 데이터</param>
		public void Push(string? value)
		{
			m_packet.Push(value);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 서버 이벤트 객체 생성 함수
		/// </summary>
		/// <param name="nName">서버 이벤트 타입</param>
		/// <returns>서버 이벤트 처리를 위한 객체 반환</returns>
		public static SFEventData CreateEventData(int nName)
		{
			SFEventData eventData = new SFEventData(nName, Packet.Create());
			eventData.Initialize();

			return eventData;
		}
	}
}
