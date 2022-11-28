using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ServerFramework
{
	/// <summary>
	/// SQL Server 데이터베이스에 대한 저장프로시저 작업을 처리하는 클래스
	/// </summary>
	public class SFSqlWork : SFSchedule
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private SqlConnection? m_conn;

		private SFSyncWork m_syncWork;
		private List<SqlCommand> m_sqlCommands;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn">SQL Server 데이터 베이스 연결 객체</param>
		public SFSqlWork(SqlConnection conn, SFSyncWork syncWork)
		{
			if (conn == null)
				throw new ArgumentNullException("conn");

			if (syncWork == null)
				throw new ArgumentNullException("syncWork");

			m_conn = conn;

			m_sqlCommands = new List<SqlCommand>();
			m_syncWork = syncWork;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// Sql 작업 추가 함수
		/// </summary>
		/// <param name="sqlCommand">실행 할 Sql 작업</param>
		public void AddCommand(SqlCommand sqlCommand)
		{
			if (sqlCommand == null)
				throw new ArgumentNullException("sqlCommand");

			m_sqlCommands.Add(sqlCommand);
		}

		/// <summary>
		/// 동기 작업을 추가하는 함수
		/// </summary>
		/// <param name="work">동기 작업</param>
		public void AddSyncWork(SFSyncWork work)
		{
			if (work == null)
				throw new ArgumentNullException("work");

			m_syncWork.AddWork(work);
		}

		/// <summary>
		/// Sql 작업 실행 함수
		/// </summary>
		public override void Run()
		{
			// 동기 작업 시작
			m_syncWork.Run();

			//
			//
			//

			SqlTransaction? trans = null;

			try
			{
				// 데이터베이스 연결
				m_conn!.Open();

				// 트랜젝션 시작
				trans = m_conn.BeginTransaction();

				// Sql 작업 컬렉션에 있는 모든 작업 실행
				foreach (SqlCommand sc in m_sqlCommands)
				{
					sc.Connection = m_conn;
					sc.Transaction = trans;
					sc.Parameters.Add("ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

					sc.ExecuteNonQuery();

					int nReturnValue = Convert.ToInt32(sc.Parameters["ReturnValue"].Value);
					if (nReturnValue != 0)
						throw new Exception(String.Format("StoredProcedure - {0}, ReturnValue - {1}", sc.CommandText, nReturnValue));
				}

				// 모든 작업이 완료되었을 경우
				trans.Commit();
				trans = null;

				// 연결을 닫고 연결객체 null 처리
				m_conn.Close();
				m_conn = null;
			}
			catch (Exception ex)
			{
				SFLogUtil.Error(GetType(), ex);

				// 데이터베이스 연결 및 트랜젝션이 null이 아닐경우는 작업도중 에러가 발생했을 경우
				if (m_conn != null)
				{
					if (trans != null)
					{
						// 모든 작업 롤백 처리
						trans.Rollback();
						trans = null;
					}

					// 데이터베이스 연결 닫기
					m_conn.Close();
					m_conn = null;
				}

				throw;
			}
			finally
			{
				// 동기 작업 종료
				m_syncWork.End();
			}
		}
	}
}
