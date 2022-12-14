using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace GameServer
{
	/// <summary>
	/// 시스템 설정
	/// </summary>
	public static class SystemConfig
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static string host
		{
			get { return ConfigurationManager.AppSettings["host"]!; }
		}

		public static int port
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["port"]); }
		}

		public static int maxConnections
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["maxConnections"]); }
		}

		public static int bufferSize
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["bufferSize"]); }
		}

		public static int backlog
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["backlog"]); }
		}

		public static int gameServerId
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["gameServerId"]); }
		}

		public static string userDBConnection
		{
			get { return ConfigurationManager.AppSettings["userDBConnection"]!; }
		}

		public static string accessTokenSecurityKey
		{
			get { return ConfigurationManager.AppSettings["accessTokenSecurityKey"]!; }
		}
	}
}
