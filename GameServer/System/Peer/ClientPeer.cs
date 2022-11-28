using ServerFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 인게임 내부의 사용자 정보를 관리하는 클래스
	/// </summary>
	public class ClientPeer : PeerImpl
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private object m_syncObject;
		private Guid m_id;

		private Synchronizer m_synchronizer;

		private Account? m_account;

		private bool m_bDisposed;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="peerInit">피어 초기화 정보 객체</param>
		public ClientPeer(SFPeerInit peerInit)
			: base(peerInit)
		{
			m_syncObject = new object();
			m_id = Guid.NewGuid();

			m_synchronizer = new Synchronizer();

			m_account = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public object syncObject
		{
			get { return m_syncObject; }
		}

		public Guid id
		{
			get { return m_id; }
		}

		public Synchronizer synchronizer
		{
			get { return m_synchronizer; }
		}

		public Account? account
		{
			get { return m_account; }
			set { m_account = value; }
		}

		public bool disposed
		{
			get { return m_bDisposed; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		public override void OnDisconnect()
		{
			ClientPeerSynchronization synchronization = new ClientPeerSynchronization(this, true, new SFAction(ProcessDisconnect));
			synchronization.Start();
		}

		private void ProcessDisconnect()
		{
			Console.WriteLine(String.Format("Disconnected IP - {0}, PORT - {1}", ipAddress, port));

			m_bDisposed = true;

			if (m_account != null)
				m_account.Logout();

			Server.instance.RemoveClientPeer(m_id);
		}
	}
}
