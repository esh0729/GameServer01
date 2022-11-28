using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// (추상 클래스)네트워크 통신 시 필요한 함수의 구현부 클래스
	/// </summary>
	public abstract class SFPeerImpl : IPeer
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private SFApplicationBase m_application;
		private UserToken m_token;

		private string m_sIpAddress;
		private int m_nPort;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token">피어 초기화 정보 객체</param>
		public SFPeerImpl(SFPeerInit peerInit)
		{
			if (peerInit == null)
				throw new ArgumentNullException("peerInit");

			m_application = peerInit.application;
			m_token = peerInit.token;
			m_token.SetPeer(this);

			m_sIpAddress = m_token.ipAddress;
			m_nPort = m_token.port;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public string ipAddress
		{
			get { return m_sIpAddress; }
		}

		public int port
		{
			get { return m_nPort; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		//
		// Receive
		//

		/// <summary>
		/// (내부)데이터 수신시 호출되는 함수
		/// </summary>
		/// <param name="packet">수신 패킷 데이터</param>
		void IPeer.OnReceive(Packet packet)
		{
			try
			{
				// 수신 데이터의 경우 작업 요청 타입 이외 작업은 처리하지 않음
				MessageType messageType = (MessageType)packet.PopByte();
				if (messageType != MessageType.OperationRequest)
					return;

				SFOperationRequest request = SFOperationRequest.CreateOperationRequest(packet);

				// 요청 타입에 따른 분류 처리
				switch (request.type)
				{
					// 응답이 필요한 명령 타입
					case RequestType.Command: OnCommand(request); break;
					// 응답이 필요없는 이벤트 타입
					case RequestType.Event: Console.WriteLine("OnEvent"); break;
				}
			}
			catch (Exception ex)
			{
				SFLogUtil.Error(GetType(), ex);
			}
		}

		/// <summary>
		/// 클라이언트 요청이 명령 타입일 경우 호출되는 함수
		/// </summary>
		/// <param name="request">클라이언트 요청</param>
		private void OnCommand(SFOperationRequest request)
		{
			int nName = (int)request.parameters[(byte)CommandParameter.Name];

			// 핸들러 ID를 통하여 해당 핸들러 Type 호출
			Type? handlerType = GetCommandHandler(nName);
			if (handlerType == null)
				throw new Exception(String.Format("해당 핸들러가 존재하지 않습니다. nName = {0}", nName));

			// Type으로 인스턴스 생성 및 초기화
			SFHandler handler = (SFHandler)Activator.CreateInstance(handlerType)!;
			handler.Initialize(this, request);

			// 핸들러 실행
			handler.Run();
		}

		/// <summary>
		/// (추상 함수)클라이언트 명령 핸들러 타입 호출 함수
		/// </summary>
		/// <param name="nName">클라이언트 명령 타입</param>
		/// <returns>해당 클라이언트 명령 핸들러 타입</returns>
		protected abstract Type? GetCommandHandler(int nName);

		//
		// Send
		//

		/// <summary>
		/// (내부)패킷 데이터 송신 함수
		/// </summary>
		/// <param name="packet">송신 패킷 데이터</param>
		void IPeer.Send(Packet packet)
		{
			m_token.Send(packet);
		}

		/// <summary>
		/// 클라이언트 응답 송신 요청 함수
		/// </summary>
		/// <param name="response">클라이언트 요청 응답 객체</param>
		public void SendResponse(SFOperationResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			((IPeer)this).Send(response.packet);
		}

		/// <summary>
		/// 서버 이벤트 송신 요청 함수
		/// </summary>
		/// <param name="eventData">서버 이벤트 객체</param>
		public void SendEvent(SFEventData eventData)
		{
			if (eventData == null)
				throw new ArgumentNullException("eventData");

			((IPeer)this).Send(eventData.packet);
		}

		//
		//
		//

		/// <summary>
		/// 연결 해제 함수
		/// </summary>
		public void Disconnect()
		{
			m_token.Disconnect();
		}

		/// <summary>
		/// (추상 함수)연결 해제 시 호출되는 함수
		/// </summary>
		public abstract void OnDisconnect();
	}
}
