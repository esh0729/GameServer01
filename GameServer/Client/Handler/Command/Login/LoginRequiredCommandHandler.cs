using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// (추상 클래스)계정 로그인이 필요한 클라이언트 명령을 지원하는 클래스
	/// </summary>
	public abstract class LoginRequiredCommandHandler<T1, T2> : CommandHandler<T1, T2>
		where T1 : CommandBody where T2 : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		protected Account? m_myAccount;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public LoginRequiredCommandHandler()
		{
			m_myAccount = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		protected override bool isValid
		{
			get { return base.isValid && m_myAccount!.isLoggedIn; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 계정 로그인 확인 후 클라이언트 명령 처리 함수 호출 함수
		/// </summary>
		protected override void OnCommandHandle()
		{
			m_myAccount = clientPeer.account;

			if (m_myAccount == null)
				throw new CommandHandleException(kResult_Error, "계정 로그인이 필요한 명령입니다.");

			OnLoginRequiredCommandHandle();
		}

		/// <summary>
		/// (추상 함수)계정 로그인이 필요한 클라이언트 명령 작업을 처리하는 함수
		/// </summary>
		protected abstract void OnLoginRequiredCommandHandle();

		/// <summary>
		/// 에러가 발생한 클라이언트에 대한 정보를 로그에 추가하는 함수(계정 정보)
		/// </summary>
		/// <param name="sb">에러 메세지를 저장하는 객체</param>
		protected override void ErrorFrom(StringBuilder sb)
		{
			base.ErrorFrom(sb);

			sb.Append("# AccountId : ");
			sb.Append(m_myAccount!.id);
			sb.AppendLine();
		}
	}
}
