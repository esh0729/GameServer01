using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 행동 클라이언트 명령 처리 클래스
	/// </summary>
	public class ActionCommandHandler : InGameCommandHandler<ActionCommandBody, ActionResponseBody>
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

			int nActionId = m_body.actionId;
			Vector3 position = m_body.position;
			float fYRotation = m_body.yRotation;

			if (nActionId == 0)
				throw new CommandHandleException(kResult_Error, "행동ID가 유효하지 않습니다. nActionId = " + nActionId);

			CharacterAction? action = m_myHero.character.GetAction(nActionId);
			if (action == null)
				throw new CommandHandleException(kResult_Error, "행동이 존재하지 않습니다. nActionId = " + nActionId);

			if (!currentPlace.ContainsPosition(position))
				throw new CommandHandleException(kResult_Error, "행동위치가 유효하지 않습니다. position = " + position);

			// 이동 처리
			currentPlace.ChangeHeroPosition(m_myHero, position, fYRotation, true);

			// 이동 중지
			m_myHero.EndMove();

			// 행동 시작
			m_myHero.StartAction(action);

			// 응답 송신
			SendResponseOK(null);
		}
	}
}
