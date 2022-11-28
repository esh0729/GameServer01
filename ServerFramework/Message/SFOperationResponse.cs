using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// 클라이언트의 요청에 대한 응답 데이터를 송신할 때 사용되는 클래스
	/// </summary>
	public class SFOperationResponse : ISFMessage
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private int m_nName;
		private long m_lnCommandId;
		private short m_nReturnCode;
		private string? m_sDebugMessage;
		private Packet m_packet;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nName">클라이언트 명령 타입</param>
		/// <param name="lnCommandId">클라이언트 명령 고유 ID</param>
		/// <param name="nReturnCode">반환 코드</param>
		/// <param name="sDebugMessage">에러 메세지</param>
		/// <param name="packet">송신 패킷</param>
		private SFOperationResponse(int nName, long lnCommandId, short nReturnCode, string? sDebugMessage, Packet packet)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			m_nName = nName;
			m_lnCommandId = lnCommandId;
			m_nReturnCode = nReturnCode;
			m_sDebugMessage = sDebugMessage;
			m_packet = packet;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public MessageType messageType
		{
			get { return MessageType.OperationResponse; }
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
			m_packet.Push(m_lnCommandId);
			m_packet.Push(m_nReturnCode);
			m_packet.Push(m_sDebugMessage);
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
		/// 클라이언트 응답 객체 생성 함수
		/// </summary>
		/// <param name="nName">클라이언트 명령 타입</param>
		/// <param name="lnCommandId">클라이언트 명령 고유 ID</param>
		/// <param name="nReturnCode">반환 코드</param>
		/// <param name="sDebugMessage">에러 메세지</param>
		/// <returns>클라이언트 응답 처리를 위한 객체 반환</returns>
		public static SFOperationResponse CreateOperationResponse(int nName, long lnCommandId, short nReturnCode, string? sDebugMessage)
		{
			SFOperationResponse operationResponse = new SFOperationResponse(nName, lnCommandId, nReturnCode, sDebugMessage, Packet.Create());
			operationResponse.Initialize();

			return operationResponse;
		}
	}
}
