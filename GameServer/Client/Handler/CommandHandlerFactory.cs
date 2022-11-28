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
	/// 클라이언트 명령 핸들러 타입을 관리하는 클래스
	/// </summary>
	public class CommandHandlerFactory : SFHandlerFactory
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 핸들러 타입을 저장하는 함수
		/// </summary>
		/// <param name="name">클라이언트 명령 이름</param>
		private void AddCommandHandler<T>(CommandName name) where T : SFHandler
		{
			AddHandler<T>((int)name);
		}

		/// <summary>
		/// 클라이언트 명령 핸들러 타입을 호출하는 함수
		/// </summary>
		/// <param name="nName">클라이언트 명령 타입</param>
		/// <returns>클라이언트 명령 이름에 대응하는 타입 또는 null 반환</returns>
		public Type? GetCommandHandler(int nName)
		{
			return GetHandler(nName);
		}

		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected override void InitializeInteral()
		{
			//
			// 계정
			//

			AddCommandHandler<LoginCommandHandler>(CommandName.Login);
			AddCommandHandler<LobbyInfoCommandHandler>(CommandName.LobbyInfo);

			//
			// 영웅
			//

			AddCommandHandler<HeroCreateCommandHandler>(CommandName.HeroCreate);
			AddCommandHandler<HeroLoginCommandHandler>(CommandName.HeroLogin);
			AddCommandHandler<HeroLogoutCommandHandler>(CommandName.HeroLogout);
			AddCommandHandler<HeroInitEnterCommandHandler>(CommandName.HeroInitEnter);

			AddCommandHandler<HeroMoveStartCommandHandler>(CommandName.HeroMoveStart);
			AddCommandHandler<HeroMoveCommandHandler>(CommandName.HeroMove);
			AddCommandHandler<HeroMoveEndCommandHandler>(CommandName.HeroMoveEnd);

			//
			// 행동
			//

			AddCommandHandler<ActionCommandHandler>(CommandName.Action);
		}
	}
}
