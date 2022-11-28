using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)핸들러의 처리에 필요한 함수의 구현부 클래스
	/// </summary>
	public abstract class PeerImpl : SFPeerImpl
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="peerInit">피어 초기화 정보 객체</param>
		public PeerImpl(SFPeerInit peerInit)
			: base(peerInit)
		{

		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 핸들러 타입 호출 함수
		/// </summary>
		/// <param name="nName">클라이언트 명령 타입</param>
		/// <returns>해당 클라이언트 명령 핸들러</returns>
		protected override Type? GetCommandHandler(int nName)
		{
			return Server.instance.commandHandlerFactory.GetCommandHandler(nName);
		}
	}
}
