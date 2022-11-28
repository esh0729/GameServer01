using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// 클라이언트 요청을 처리할때 사용되는 클래스
	/// </summary>
	public class SFOperationRequest : ISFMessage
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private RequestType m_type;
		private Packet m_packet;
		private Dictionary<byte, object> m_parameters;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">요청 타입</param>
		/// <param name="packet">수신 패킷</param>
		private SFOperationRequest(Packet packet)
		{
			m_type = default(RequestType);
			m_packet = packet;
			m_parameters = new Dictionary<byte, object>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public MessageType messageType
		{
			get { return MessageType.OperationRequest; }
		}

		public RequestType type
		{
			get { return m_type; }
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
		}

		public Dictionary<byte,object> parameters
		{
			get { return m_parameters; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		private void Initialize()
		{
			m_type = (RequestType)m_packet.PopByte();

			switch (m_type)
			{
				case RequestType.Command:
					{
						m_parameters[(byte)CommandParameter.Name] = m_packet.PopInt32();
						m_parameters[(byte)CommandParameter.Id] = m_packet.PopInt64();
					}
					break;

				case RequestType.Event: break;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 클라이언트 요청 객체 생성 함수
		/// </summary>
		/// <param name="packet">수신 패킷</param>
		/// <returns>클라이언트 요청 처리를 위한 객체 반환</returns>
		public static SFOperationRequest CreateOperationRequest(Packet packet)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			SFOperationRequest request = new SFOperationRequest(packet);
			request.Initialize();

			return request;
		}
	}
}
