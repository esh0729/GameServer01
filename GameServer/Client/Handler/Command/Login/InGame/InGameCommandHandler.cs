using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)영웅 로그인이 필요한 클라이언트 명령을 지원하는 클래스
	/// </summary>
	public abstract class InGameCommandHandler<T1,T2> : LoginRequiredCommandHandler<T1,T2>
		where T1 : CommandBody where T2 : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected Hero? m_myHero;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		public InGameCommandHandler()
		{
			m_myHero = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		protected override bool isValid
		{
			get { return base.isValid && m_myHero!.isLoggedIn; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 영웅 로그인 확인 후 클라이언트 명령 처리 함수 호출 함수
		/// </summary>
		protected override void OnLoginRequiredCommandHandle()
		{
			m_myHero = m_myAccount!.currentHero;

			if (m_myHero == null)
				throw new CommandHandleException(kResult_Error, "영웅 로그인이 필요한 명령입니다.");

			OnInGameCommandHandle();
		}

		/// <summary>
		/// (추상 함수)영웅 로그인이 필요한 클라이언트 명령 작업을 처리하는 함수
		/// </summary>
		protected abstract void OnInGameCommandHandle();

		/// <summary>
		/// 에러가 발생한 클라이언트에 대한 정보를 로그에 추가하는 함수(영웅 정보)
		/// </summary>
		/// <param name="sb">에러 메세지를 저장하는 객체</param>
		protected override void ErrorFrom(StringBuilder sb)
		{
			base.ErrorFrom(sb);

			sb.Append("# HeroId : ");
			sb.Append(m_myHero!.id);
			sb.AppendLine();
		}
	}
}
