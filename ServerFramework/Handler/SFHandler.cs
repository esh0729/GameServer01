using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// (추상 클래스)수신된 클라이언트 요청을 처리하기 위한 작업 클래스
	/// </summary>
	public abstract class SFHandler : ISFWork
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected SFPeerImpl? m_peer;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		/// <param name="peer">요청을 송신한 클라이언트 피어</param>
		/// <param name="request">클라이언트 요청</param>
		public void Initialize(SFPeerImpl peer, SFOperationRequest request)
		{
			if (peer == null)
				throw new ArgumentNullException("peer");

			if (request == null)
				throw new ArgumentNullException("request");

			m_peer = peer;

			InitializeInternal(request);
		}

		/// <summary>
		/// (추상 함수)자식 클래스 초기화 함수
		/// </summary>
		/// <param name="request">클라이언트 요청</param>
		protected abstract void InitializeInternal(SFOperationRequest request);

		/// <summary>
		/// (추상 함수)내부 실행 함수
		/// </summary>
		public abstract void Run();
	}
}
