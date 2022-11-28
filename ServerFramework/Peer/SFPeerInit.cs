using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TAPSocket;

namespace ServerFramework
{
	/// <summary>
	/// 피어 초기화를 위한 클래스
	/// </summary>
	public class SFPeerInit
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private SFApplicationBase m_application;
		private UserToken m_token;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="application">서버 메인 객체</param>
		/// <param name="token">사용자 토큰</param>
		public SFPeerInit(SFApplicationBase application, UserToken token)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (token == null)
				throw new ArgumentNullException("token");

			m_application = application;
			m_token = token;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public SFApplicationBase application
		{
			get { return m_application; }
		}

		public UserToken token
		{
			get { return m_token; }
		}
	}
}
