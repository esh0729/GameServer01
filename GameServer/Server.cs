using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using ServerFramework;

namespace GameServer
{
	/// <summary>
	/// 서버의 초기화 및 시작, 종료, 게임 동작에 필요한 시스템을 제공하는 서버 메인 클래스
	/// </summary>
	public class Server : SFApplicationBase
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private SFWorker m_standaloneWorker;

		private CommandHandlerFactory m_commandHandlerFactory;

		private Dictionary<int, GameServer> m_gameServers;

		private Dictionary<Guid, ClientPeer> m_clientPeers;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nMaxConnections">최대 접속자 수</param>
		/// <param name="nBufferSize">입출력버퍼의 크기</param>
		public Server(int nMaxConnections, int nBufferSize)
			: base(nMaxConnections, nBufferSize)
		{
			m_standaloneWorker = new SFWorker();

			m_commandHandlerFactory = new CommandHandlerFactory();

			m_gameServers = new Dictionary<int, GameServer>();

			m_clientPeers = new Dictionary<Guid, ClientPeer>();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public GameServer currentGameServer
		{
			get { return GetGameServer(SystemConfig.gameServerId)!; }
		}

		public CommandHandlerFactory commandHandlerFactory
		{
			get { return m_commandHandlerFactory; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 서버 네트워크 서비스 시작 함수
		/// </summary>
		/// <param name="sHost"></param>
		/// <param name="nPort"></param>
		/// <param name="nBacklog"></param>
		public override void Start(string sHost, int nPort, int nBacklog)
		{
			// 서버 네트워크 서비스 시작전 초기화 작업 처리
			Initialize();

			base.Start(sHost, nPort, nBacklog);

			//
			//
			//

			SFLogUtil.System(GetType(), "GameServer Started.");
		}

		/// <summary>
		/// 초기화 함수
		/// </summary>
		private void Initialize()
		{
			SFLogUtil.System(GetType(), "Initialize Started.");

			// 독립 작업자 시작
			m_standaloneWorker.Start();

			// 클라이언트 명령 핸들러 타입 컬렉션 객체 초기화
			m_commandHandlerFactory.Initialize();

			// 동기 작업 클래스 관련 초기화 
			SyncWorkUtil.Initialize();

			//
			//
			//

			SqlConnection? conn = null;

			try
			{
				conn = Util.OpenUserDBConnection();

				// 시스템관련 데이터 초기화
				InitializeSystemData(conn);

				// 리소스 데이터 초기화
				Resource.instance.Initialize(conn);
			}
			catch (Exception ex)
			{
				SFLogUtil.Error(GetType(), ex);
			}
			finally
			{
				Util.Close(ref conn);
			}

			//
			// 
			//

			lock (Cache.instance.syncObject)
			{
				// Cache 초기화
				Cache.instance.Initialize();
			}

			SFLogUtil.System(GetType(), "Initialize Completed.");
		}

		/// <summary>
		/// 시스템 관련 데이터 초기화 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		private void InitializeSystemData(SqlConnection conn)
		{
			SFLogUtil.System(GetType(), "SystemData Initialize Started.");

			//
			//
			//

			// 게임서버 데이터 데이터베이스에서 로드
			foreach (DataRow dr in UserDBDoc.GameServers(conn, null))
			{
				GameServer gameServer = new GameServer();
				gameServer.Set(dr);

				m_gameServers.Add(gameServer.id, gameServer);
			}

			//
			//
			//

			SFLogUtil.System(GetType(), "SystemData Initialize Completed.");
		}

		/// <summary>
		/// 서버 종료 완료시 호출되는 함수
		/// </summary>
		protected override void OnTearDown()
		{
			SFLogUtil.System(GetType(), "OnTearDown Started!");

			// Cache 리소스 해제 처리
			lock (Cache.instance.syncObject)
			{
				Cache.instance.Dispose();
			}

			// 비동기 독립 작업자 종료 처리
			m_standaloneWorker.Stop();

			SFLogUtil.System(GetType(), "OnTearDown Finished!");
		}

		/// <summary>
		/// 게임서버 데이터 호출 함수
		/// </summary>
		/// <param name="nId">게임서버 ID</param>
		/// <returns>요청 게임서버 또는 null 반환</returns>
		public GameServer? GetGameServer(int nId)
		{
			GameServer? value;

			return m_gameServers.TryGetValue(nId, out value) ? value : null;
		}

		//
		// 피어
		//

		/// <summary>
		/// 피어 생성 함수
		/// </summary>
		/// <param name="peerInit"></param>
		/// <returns>생성된 피어 객체 반환</returns>
		protected override SFPeerImpl CreatePeer(SFPeerInit peerInit)
		{
			ClientPeer clientPeer = new ClientPeer(peerInit);

			lock (Cache.instance.syncObject)
			{
				m_clientPeers.Add(clientPeer.id, clientPeer);
			}

			Console.WriteLine(String.Format("Connected IP - {0}, PORT - {1}", clientPeer.ipAddress, clientPeer.port));

			return clientPeer;
		}

		/// <summary>
		/// 클라이언트 피어 호출 함수
		/// </summary>
		/// <param name="id">클라이언트 피어 ID</param>
		/// <returns>ID에 대응하는 클라이언트 피어 또는 null 반환</returns>
		public ClientPeer? GetClientPeer(Guid id)
		{
			ClientPeer? value;

			return m_clientPeers.TryGetValue(id, out value) ? value : null;
		}

		/// <summary>
		/// 클라이언트 피어 삭제 함수
		/// </summary>
		/// <param name="id">클라이언트 피어 ID</param>
		public void RemoveClientPeer(Guid id)
		{
			m_clientPeers.Remove(id);
		}

		//
		//
		//

		/// <summary>
		/// 비동기 독립 작업 큐잉 함수
		/// </summary>
		/// <param name="work">비동기 독립 작업이 필요한 작업</param>
		public void AddStandaloneWork(ISFWork work)
		{
			m_standaloneWorker.Add(work);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static Server? s_instance;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static Server instance
		{
			get { return s_instance!; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 서버 생성 함수
		/// </summary>
		/// <param name="nMaxConnection">최대 접속자 수</param>
		/// <param name="nBufferSize">입출력버퍼 크기</param>
		/// <returns>생성된 서버</returns>
		public static Server CreateServer(int nMaxConnection, int nBufferSize)
		{
			s_instance = new Server(nMaxConnection, nBufferSize);

			return s_instance;
		}
	}
}
