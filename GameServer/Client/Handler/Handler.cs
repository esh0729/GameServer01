using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)클라이언트 요청에 대한 작업을 실행하는 클래스
	/// </summary>
	public abstract class Handler : SFHandler
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public ClientPeer clientPeer
		{
			get { return (ClientPeer)m_peer!; }
		}

		// Cache의 동기 실행이 필요할 경우 override 하여 true로 변경 필요
		protected virtual bool globalLockRequired
		{
			get { return false; }
		}

		protected virtual bool isValid
		{
			get { return !clientPeer.disposed; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public functions

		/// <summary>
		/// 핸들러 작업 실행 함수
		/// </summary>
		public override void Run()
		{
			ClientPeerSynchronization synchronization = new ClientPeerSynchronization(clientPeer, globalLockRequired, new SFAction(OnHandle));
			synchronization.Start();
		}

		/// <summary>
		/// (추상 함수)핸들러 작업 처리 함수
		/// </summary>
		protected abstract void OnHandle();
	}
}
