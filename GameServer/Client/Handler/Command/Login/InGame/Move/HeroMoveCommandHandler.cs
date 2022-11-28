﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 영웅 이동 클라이언트 명령 처리 클래스
	/// </summary>
	public class HeroMoveCommandHandler : InGameCommandHandler<HeroMoveCommandBody, HeroMoveResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnInGameCommandHandle()
		{
			//
			// 영웅 상태 검사
			//

			PhysicalPlace? currentPlace = m_myHero!.currentPlace;
			if (currentPlace == null)
				throw new CommandHandleException(kResult_Error, "현재 장소에서 사용할 수 없는 명령입니다.");

			//
			// 유효성 검사
			//

			if (m_body == null)
				throw new CommandHandleException(kResult_Error, "body가 null입니다.");

			// 유효성 검사
			Guid placeInstanceId = m_body.placeInstanceId;
			Vector3 position = m_body.position;
			float fYRotation = m_body.yRotation;

			if (placeInstanceId == Guid.Empty)
				throw new CommandHandleException(kResult_Error, "장소ID가 유효하지 않습니다.");

			if (placeInstanceId != currentPlace.instanceId)
				throw new CommandHandleException(kResult_Error, "현재 장소에서 사용하지 않은 명령입니다.");

			if (!currentPlace.ContainsPosition(position))
				throw new CommandHandleException(kResult_Error, "이동위치가 유효하지 않습니다. position = " + position);

			if (!m_myHero.moving)
				throw new CommandHandleException(kResult_Error, "영웅이 이동중이 아닙니다.");

			// 이동
			InterestedAreaInfo info = currentPlace.MoveHero(m_myHero, position, fYRotation, false);

			// 응답객체 생성
			HeroMoveResponseBody resBody = new HeroMoveResponseBody();
			resBody.addedHeroes = Hero.ToPDHeroes(info.GetAddedSectorHeroes(m_myHero.id)).ToArray();
			resBody.removedHeroes = info.GetRemovedSectorHeroIds(m_myHero.id).ToArray();

			// 응답 송신
			SendResponseOK(resBody);
		}
	}
}
