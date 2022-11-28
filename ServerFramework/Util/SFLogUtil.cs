using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	// 로깅작업 처리 클래스
	public static class SFLogUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		private const string kTimeStringFormat = "[yyyy'-'MM'-'dd' 'HH':'mm':'ss,fff]";

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static SFWorker m_worker;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructos

		/// <summary>
		/// 
		/// </summary>
		static SFLogUtil()
		{
			m_worker = new SFWorker();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public static void Initialize()
		{
			m_worker.Start();
		}

		/// <summary>
		/// 로깅작업 종료 함수
		/// </summary>
		public static void Stop()
		{
			m_worker.Stop();
		}

        //
        // INFO
        //

        /// <summary>
        /// INFO 로그 작성 함수
        /// </summary>
        /// <param name="type">로그 작성 클래스 타입</param>
        /// <param name="sMessage">로그 메세지</param>
        public static void Info(Type type, string sMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTimeOffset.Now.ToString(kTimeStringFormat));
            sb.Append(" | ");
            sb.Append("INFO");
            sb.Append(" | ");

            if (type != null)
            {
                sb.Append(type.Namespace);
                sb.Append('.');
                sb.Append(type.Name);
                sb.Append(" : ");
            }

            sb.Append(sMessage);

            m_worker.Add(new SFAction<StringBuilder>(WriteLog!, sb));
        }

        //
        // WARN
        //

        /// <summary>
        /// WARN 로그 작성 함수
        /// </summary>
        /// <param name="type">로그 작성 클래스 타입</param>
        /// <param name="sMessage">로그 메세지</param>
        public static void Warn(Type type, string sMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTimeOffset.Now.ToString(kTimeStringFormat));
            sb.Append(" | ");
            sb.Append("WARN");
            sb.Append(" | ");

            if (type != null)
            {
                sb.Append(type.Namespace);
                sb.Append('.');
                sb.Append(type.Name);
                sb.Append(" : ");
            }

            sb.Append(sMessage);

            m_worker.Add(new SFAction<StringBuilder>(WriteLog!, sb));
        }

        //
        // ERROR
        //

        /// <summary>
        /// ERROR 로그 작성 함수
        /// </summary>
        /// <param name="type">로그 작성 클래스 타입</param>
        /// <param name="ex">발생 에러</param>
        public static void Error(Type? type, Exception ex)
        {
            Error(type, ex.Message, true, ex.StackTrace);
        }

        /// <summary>
        /// ERROR 로그 작성 함수
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sMessage"></param>
        public static void Error(Type? type, string sMessage)
        {
            Error(type, sMessage, false, null);
        }

        public static void Error(Type? type, string sMessage, bool bLoggingTrace, string? sStackTrace)
        {
            Error(type, null, sMessage, bLoggingTrace, sStackTrace);
        }

        public static void Error(Type? type, StringBuilder? sb, string sMessage, bool bLoggingTrace, string? sStackTrace)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append(DateTimeOffset.Now.ToString(kTimeStringFormat));
            sb.Append(" | ");
            sb.Append("ERROR");
            sb.Append(" | ");

            if (type != null)
            {
                sb.Append(type.Namespace);
                sb.Append('.');
                sb.Append(type.Name);
                sb.Append(" : ");
            }

            sb.Append(sMessage);

            if (bLoggingTrace)
            {
                sb.AppendLine();
                sb.Append(sStackTrace);
            }

            m_worker.Add(new SFAction<StringBuilder>(WriteLog!, sb));
        }

        public static void System(Type type, string sMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTimeOffset.Now.ToString(kTimeStringFormat));
            sb.Append(" | ");
            sb.Append("SYSTEM");
            sb.Append(" | ");

            if (type != null)
            {
                sb.Append(type.Namespace);
                sb.Append('.');
                sb.Append(type.Name);
                sb.Append(" : ");
            }

            sb.Append(sMessage);

            m_worker.Add(new SFAction<StringBuilder>(WriteLog!, sb));
        }

        //
        //
        //

        private static void WriteLog(StringBuilder sb)
		{
			string sPath = Environment.CurrentDirectory + @"\Error";
			DirectoryInfo di = new DirectoryInfo(sPath);

			if (!di.Exists)
				di.Create();

			string sDate = DateTimeOffset.Now.ToString("yyyy-MM-dd");
			string sFilePath = sPath + @"\" + sDate + ".txt";

			if (!File.Exists(sFilePath))
			{
				using (StreamWriter writer = File.CreateText(sFilePath))
				{
					writer.WriteLine(sb.ToString());
				}
			}
			else
			{
				using (StreamWriter writer = File.AppendText(sFilePath))
				{
					writer.WriteLine(sb.ToString());
				}
			}
		}
	}
}
