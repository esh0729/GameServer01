using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 영웅 로그아웃 클라이언트 명령 처리 클래스
	/// </summary>
	public class HeroLogoutCommandHandler : InGameCommandHandler<HeroLogoutCommandBody,HeroLogoutResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		protected override bool globalLockRequired
		{
			get { return true; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnInGameCommandHandle()
		{
			// 영웅 로그아웃 함수 호출
			m_myHero!.Logout();

			// 응답 송신
			SendResponseOK(null);
		}
	}
}
