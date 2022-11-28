using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// (추상 클래스)네트워크 서비스 통신 객체를 관리하는 메인 클래스
	/// </summary>
	public abstract class SFApplicationBase
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private NetworkService m_service;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nMaxConnections">최대 접속자 수</param>
		/// <param name="nBufferSize">입출력버퍼의 크기</param>
		public SFApplicationBase(int nMaxConnections, int nBufferSize)
		{
			if (nMaxConnections <= 0)
				throw new ArgumentOutOfRangeException("nMaxConnections");

			if (nBufferSize <= 0)
				throw new ArgumentOutOfRangeException("nBufferSize");

			// 로그 작업자 초기화는 모든 작업중 가장 먼저 처리
			SFLogUtil.Initialize();

			// 네트워크 서버스 생성 및 초기화 처리
			m_service = new NetworkService(nMaxConnections, nBufferSize, 1);
			m_service.Initialize();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 서버 네트워크 서비스 시작 함수
		/// </summary>
		/// <param name="sHost">서버 주소</param>
		/// <param name="nPort">서버 포트</param>
		/// <param name="nBacklog">연결요청대기큐 크기</param>
		public virtual void Start(string sHost, int nPort, int nBacklog)
		{
			m_service.onCreatedSession += OnCreatedSession;
			m_service.Listen(sHost, nPort, nBacklog);
		}

		/// <summary>
		/// 클라이언트 연결 완료시 호출되는 함수
		/// </summary>
		/// <param name="token">연결된 클라이언트 사용자 토큰</param>
		private void OnCreatedSession(UserToken token)
		{
			CreatePeer(new SFPeerInit(this, token));
		}

		/// <summary>
		/// 서버 네트워크 서비스 종료 함수
		/// </summary>
		public void Stop()
		{
			m_service.Close();

			OnTearDown();

			// 로그 작업자 종료는 서버 종료 작업 가장 마지막에 처리
			SFLogUtil.Stop();
		}

		/// <summary>
		/// (추상 함수)서버 네트워크 서비스 종료 완료시 호출되는 함수
		/// </summary>
		protected abstract void OnTearDown();

		//
		// Peer
		//

		/// <summary>
		/// (추상 함수)피어 생성 함수
		/// </summary>
		/// <returns>생성된 피어 객체</returns>
		protected abstract SFPeerImpl CreatePeer(SFPeerInit peerInit);
	}
}
