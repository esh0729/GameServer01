using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 서버 이벤트(클라이언트 요청 없이 데이터 송신) 타입
	/// </summary>
	public enum ServerEventName : int
	{
		None = 0,

		//
		// 계정
		//

		// 계정 로그인 중복
		LoginDuplicated,

		//
		// 영웅
		//

		// 다른 영웅 입장
		HeroEnter,
		// 다른 영웅 퇴장
		HeroExit,
		// 다른 영웅 이동
		HeroMove,
		// 다른 영웅 행동 시작
		HeroActionStarted,

		//
		// 관심영역
		//

		// 관심 영역 변경
		InterestedAreaChanged,
		// 다른 영웅 관심 영역 입장
		HeroInterestedAreaEnter,
		// 다른 영웅 관심 영역 퇴장
		HeroInterestedAreaExit
	}
}
