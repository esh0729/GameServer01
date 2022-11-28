using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GameServer
{
	/// <summary>
	/// 기타 작업에 대한 추가 기능을 제공하는 클래스
	/// </summary>
	public static class Util
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		//
		// Sql
		//

		/// <summary>
		/// 데이터베이스 연결 객체 생성 함수
		/// </summary>
		/// <param name="sDBPath">데이터베이스 경로</param>
		/// <returns>생성된 데이터베이스 연결 객체 반환</returns>
		public static SqlConnection CreateDBConnection(string sDBPath)
		{
			return new SqlConnection(sDBPath);
		}

		/// <summary>
		/// UserDB 데이터베이스 연결 객체 생성 함수
		/// </summary>
		/// <returns>생성된 UserDB 데이터베이스 연결 객체 반환</returns>
		public static SqlConnection CreateUserDBConnection()
		{
			return CreateDBConnection(SystemConfig.userDBConnection);
		}

		/// <summary>
		/// GameDB 데이터베이스 연결 객체 생성 함수
		/// </summary>
		/// <returns>생성된 GameDB 데이터베이스 연결 객체 반환</returns>
		public static SqlConnection CreateGameDBConnection()
		{
			return CreateDBConnection(Server.instance.currentGameServer.dbPath);
		}

		/// <summary>
		/// UserDB 데이터베이스 연결 객체 생성 및 연결을 여는 함수
		/// </summary>
		/// <returns>연결이 열린 UserDB 데이터베이스 연결 객체 반환</returns>
		public static SqlConnection OpenUserDBConnection()
		{
			SqlConnection conn = CreateUserDBConnection();
			conn.Open();

			return conn;
		}

		/// <summary>
		/// GameDB 데이터베이스 연결 객체 생성 및 연결을 여는 함수
		/// </summary>
		/// <returns>연결이 열린 GameDB 데이터베이스 연결 객체 반환</returns>
		public static SqlConnection OpenGameDBConnection()
		{
			SqlConnection conn = CreateGameDBConnection();
			conn.Open();

			return conn;
		}

		/// <summary>
		/// 데이터베이스 연결 객체 연결 종료 함수
		/// </summary>
		/// <param name="conn">데이터베이스 연결 객체</param>
		public static void Close(ref SqlConnection? conn)
		{
			if (conn == null)
				throw new ArgumentNullException("conn");

			conn.Close();
			conn = null;
		}

		/// <summary>
		/// 데이터베이스 트랜잭션 변경사항 기록 함수 
		/// </summary>
		/// <param name="trans">데이터베이스 트랜잭션 객체</param>
		public static void Commit(ref SqlTransaction? trans)
		{
			if (trans == null)
				throw new ArgumentNullException("trans");

			trans.Commit();
			trans = null;
		}

		/// <summary>
		/// 데이터베이스 트랜잭션 복구 함수
		/// </summary>
		/// <param name="trans">데이터베이스 트랜잭션 객체</param>
		public static void Rollback(ref SqlTransaction? trans)
		{
			if (trans == null)
				throw new ArgumentNullException("trans");

			trans.Rollback();
			trans = null;
		}
	}
}
