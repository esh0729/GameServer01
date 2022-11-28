using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServerFramework;
using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 서버 이벤트 객체 생성 및 송신 처리 클래스
	/// </summary>
	public static class ServerEvent
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 서버 이벤트 송신 요청 함수(개인)
		/// </summary>
		/// <param name="name">서버 이벤트 타입</param>
		/// <param name="body">송신 데이터 저장 객체</param>
		/// <param name="clientPeer">클라이언트 피어</param>
		private static void Send(ServerEventName name, ServerEventBody? body, ClientPeer clientPeer)
		{
			// 서버 이벤트 객체 생성
			SFEventData eventData = SFEventData.CreateEventData((int)name);

			// 직렬화 처리
			if (body != null)
			{
				int nPosition = body.SerializeRaw(eventData.packetBuffer, eventData.packetPosition);
				eventData.packetPosition = nPosition;
			}

			// 송신 요청
			clientPeer.SendEvent(eventData);
		}

		/// <summary>
		/// 서버 이벤트 송신 요청 함수(다수)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="body"></param>
		/// <param name="clientPeers"></param>
		private static void Send(ServerEventName name, ServerEventBody? body, IEnumerable<ClientPeer> clientPeers)
		{
			// 서버 이벤트 객체 생성
			SFEventData eventData = SFEventData.CreateEventData((int)name);

			// 직렬화 처리
			if (body != null)
			{
				int nPosition = body.SerializeRaw(eventData.packetBuffer, eventData.packetPosition);
				eventData.packetPosition = nPosition;
			}

			// 송신 요청
			foreach (ClientPeer clientPeer in clientPeers)
			{
				clientPeer.SendEvent(eventData);
			}
		}

		//
		// 계정
		//

		/// <summary>
		/// 계정 로그인 중복 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeer">클라이언트 피어</param>
		public static void SendLoginDuplicatedEvent(ClientPeer clientPeer)
		{
			Send(ServerEventName.LoginDuplicated, null, clientPeer);
		}

		//
		// 영웅
		//

		/// <summary>
		/// 다른 영웅 입장 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="hero">영웅 정보 객체</param>
		public static void SendHeroEnterEvent(IEnumerable<ClientPeer> clientPeers, PDHero hero)
		{
			SEBHeroEnterEventBody body = new SEBHeroEnterEventBody();
			body.hero = hero;

			Send(ServerEventName.HeroEnter, body, clientPeers);
		}

		/// <summary>
		/// 다른 영웅 퇴장 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="heroId">영웅 ID</param>
		public static void SendHeroExitEvent(IEnumerable<ClientPeer> clientPeers, Guid heroId)
		{
			SEBHeroExitEventBody body = new SEBHeroExitEventBody();
			body.heroId = heroId;

			Send(ServerEventName.HeroExit, body, clientPeers);
		}

		/// <summary>
		/// 다른 영웅 이동 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="heroId">영웅 ID</param>
		/// <param name="position">위치</param>
		/// <param name="fYRotation">방향</param>
		public static void SendHeroMoveEvent(IEnumerable<ClientPeer> clientPeers, Guid heroId, PDVector3 position, float fYRotation)
		{
			SEBHeroMoveEventBody body = new SEBHeroMoveEventBody();
			body.heroId = heroId;
			body.position = position;
			body.yRotation = fYRotation;

			Send(ServerEventName.HeroMove, body, clientPeers);
		}

		/// <summary>
		/// 다른 영웅 행동 시작 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="heroId">영웅 ID</param>
		/// <param name="nActionId">행동 ID</param>
		/// <param name="position">위치</param>
		/// <param name="fYRotation">방향</param>
		public static void SendHeroActionStartedEvent(IEnumerable<ClientPeer> clientPeers, Guid heroId, int nActionId, PDVector3 position, float fYRotation)
		{
			SEBHeroActionStartedEventBody body = new SEBHeroActionStartedEventBody();
			body.heroId = heroId;
			body.actionId = nActionId;
			body.position = position;
			body.yRotation = fYRotation;

			Send(ServerEventName.HeroActionStarted, body, clientPeers);
		}

		//
		// 관심영역
		//

		/// <summary>
		/// 관심 영역 변경 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeer">클라이언트 피어</param>
		/// <param name="addedHeroes">추가 된 영웅 객체 배열</param>
		/// <param name="removedHeroes">삭제 된 영웅 ID 배열</param>
		public static void SendInterestedAreaChangedEvent(ClientPeer clientPeer, PDHero[] addedHeroes, Guid[] removedHeroes)
		{
			SEBInterestedAreaChangedEventBody body = new SEBInterestedAreaChangedEventBody();
			body.addedHeroes = addedHeroes;
			body.removedHeroes = removedHeroes;

			Send(ServerEventName.InterestedAreaChanged, body, clientPeer);
		}

		/// <summary>
		/// 다른 영웅 관심 영역 입장 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="hero">영웅 정보 객체</param>
		public static void SendHeroInterestedAreaEnterEvent(IEnumerable<ClientPeer> clientPeers, PDHero hero)
		{
			SEBHeroInterestedAreaEnterEventBody body = new SEBHeroInterestedAreaEnterEventBody();
			body.hero = hero;

			Send(ServerEventName.HeroInterestedAreaEnter, body, clientPeers);
		}

		/// <summary>
		/// 다른 영웅 관심 영역 퇴장 서버 이벤트 송신 함수
		/// </summary>
		/// <param name="clientPeers">클라이언트 피어 열거자</param>
		/// <param name="heroId">영웅 ID</param>
		public static void SendHeroInterestedAreaExitEvent(IEnumerable<ClientPeer> clientPeers, Guid heroId)
		{
			SEBHeroInterestedAreaExitEventBody body = new SEBHeroInterestedAreaExitEventBody();
			body.heroid = heroId;

			Send(ServerEventName.HeroInterestedAreaExit, body, clientPeers);
		}
	}
}
