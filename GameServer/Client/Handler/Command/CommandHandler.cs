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
	/// (추상 클래스)클라이언트 명령에 대한 작업을 실행하는 클래스
	/// </summary>
	public abstract class CommandHandler<T1, T2> : Handler
		where T1 : CommandBody where T2 : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constants

		// 클라이언트 작업 요청에 대한 결과 상수
		public const short kResult_OK		= 0;
		public const short kResult_Error	= 1;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private CommandName m_name;
		private long m_lnCommandId;
		protected T1? m_body;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public CommandHandler()
		{
			m_name = CommandName.None;
			m_lnCommandId = 0;
			m_body = null;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions
		
		/// <summary>
		/// 초기화 함수
		/// </summary>
		/// <param name="request">클라이언트 요청</param>
		protected override void InitializeInternal(SFOperationRequest request)
		{
			m_name = (CommandName)request.parameters[(byte)CommandParameter.Name];
			m_lnCommandId = (long)request.parameters[(byte)CommandParameter.Id];
			m_body = (T1)Activator.CreateInstance(typeof(T1))!;

			m_body.DeserializeRaw(request.packetBuffer, request.packetPosition);
		}

		/// <summary>
		/// 핸들러 작업 시작 함수
		/// </summary>
		protected override void OnHandle()
		{
			try
			{
				OnCommandHandle();
			}
			catch (CommandHandleException ex)
			{
				ErrorLog(ex);

				SendResponse(ex.result, ex.Message);
			}
			catch (Exception ex)
			{
				ErrorLog(ex);

				SendResponse(kResult_Error, "Command processing error");
			}
		}

		/// <summary>
		/// (추상 함수)클라이언트 명령의 세부적인 처리를 위한 함수
		/// </summary>
		protected abstract void OnCommandHandle();
		
		//
		// 비동기 처리
		//

		/// <summary>
		/// 비동기 독립 작업 등록 함수
		/// </summary>
		/// <param name="action"></param>
		protected void RunnableStandaloneWork(Action action, SFSyncWork? syncWork)
		{
			Server.instance.AddStandaloneWork(new SFAction<Action, SFSyncWork?>(RunStandaloneWork!, action, syncWork));
		}

		/// <summary>
		/// 비동기 독립 작업 실행 함수
		/// </summary>
		/// <param name="action">비동기 작업 대리자</param>
		/// <param name="syncWork">동기 작업 처리 객체</param>
		private void RunStandaloneWork(Action action, SFSyncWork? syncWork)
		{
			try
			{
				// 비동기 작업에 대한 동기 작업이 있을 동기 작업 시작
				if (syncWork != null)
					syncWork.Run();

				// 유효성 검사
				lock (clientPeer.syncObject)
				{
					if (!isValid)
						return;
				}

				// 비동기 작업
				action();
			}
			catch (Exception ex)
			{
				FinishWork(ex);

				return;
			}
			finally
			{
				// 동기 작업 종료
				if (syncWork != null)
					syncWork.End();
			}

			//
			//
			//

			// 비동기 작업 완료 이후 처리할 작업
			try
			{
				FinishWork(null);
			}
			catch (CommandHandleException ex)
			{
				ErrorLog(ex);

				SendResponse(ex.result, ex.Message);
			}
			catch (Exception ex)
			{
				ErrorLog(ex);

				SendResponse(kResult_Error, "Command processing error");
			}
		}

		/// <summary>
		/// 비동기 작업 처리 이후 처리할 작업을 진행하는 함수
		/// </summary>
		/// <param name="ex">발생 에러 객체</param>
		protected void FinishWork(Exception? ex)
		{
			if (ex == null)
			{
				ClientPeerSynchronization synchronization = new ClientPeerSynchronization(clientPeer, globalLockRequired, new SFAction(OnWorkSuccess));
				synchronization.Start();
			}
			else
			{
				CommandHandleException? commandEx = ex as CommandHandleException;
				if (commandEx != null)
				{
					ErrorLog(commandEx);

					SendResponse(commandEx.result, commandEx.Message);
				}
				else
				{
					ErrorLog(ex);

					SendResponse(kResult_Error, "Command processing error.");
				}

				OnWorkFail(ex);
			}
		}

		/// <summary>
		/// (가상 함수)비동기 작업 처리 완료 이후 처리할 작업을 처리하는 함수
		/// </summary>
		protected virtual void OnWorkSuccess()
		{

		}

		/// <summary>
		/// (가상 함수)비동기 작업 처리 실패 이후 처리할 작업을 처리하는 함수
		/// </summary>
		/// <param name="ex">발생 에러 객체</param>
		protected virtual void OnWorkFail(Exception ex)
		{

		}

		//
		// 응답
		//

		/// <summary>
		/// 클라이언트 명령 응답 송신 요청 함수
		/// </summary>
		/// <param name="response"></param>
		private void SendResponse(SFOperationResponse response)
		{
			m_peer!.SendResponse(response);
		}

		/// <summary>
		/// 클라이언트 명령 실패 응답 송신 요청 함수
		/// </summary>
		/// <param name="nResult">결과 코드</param>
		/// <param name="sDebugMessage">에러 메세지</param>
		protected void SendResponse(short nResult, string sDebugMessage)
		{
			SFOperationResponse response = SFOperationResponse.CreateOperationResponse((int)m_name, m_lnCommandId, nResult, sDebugMessage);

			SendResponse(response);
		}

		/// <summary>
		/// 클라이언트 명령에 대한 응답을 생성하여 송신 요청하는 함수
		/// </summary>
		/// <param name="nResult">결과 코드</param>
		/// <param name="responseBody">클라이언트에게 송신 할 응답 데이터</param>
		protected void SendResponse(short nResult, T2? responseBody)
		{
			SFOperationResponse response = SFOperationResponse.CreateOperationResponse((int)m_name, m_lnCommandId, nResult, null);

			if (responseBody != null)
			{
				int nPosition = responseBody.SerializeRaw(response.packetBuffer, response.packetPosition);
				response.packetPosition = nPosition;
			}

			SendResponse(response);
		}

		/// <summary>
		/// 클라이언트 명령에 대한 성공 응답 데이터를 송신 요청하는 함수
		/// </summary>
		/// <param name="responseBody">클라이언트에게 송신 할 응답 데이터</param>
		protected void SendResponseOK(T2? responseBody)
		{
			SendResponse(kResult_OK, responseBody);
		}

		//
		// 로그
		//

		/// <summary>
		/// 핸들러 내에서 발생한 에러 로깅 요청 함수
		/// </summary>
		/// <param name="ex">에러 객체</param>
		protected void ErrorLog(Exception ex)
		{
			ErrorLog(ex.Message, true, ex.StackTrace);
		}

		/// <summary>
		/// 핸들러 내에서 발생한 명령 에러 로깅 요청 함수
		/// </summary>
		/// <param name="ex">명령 에러 객체</param>
		protected void ErrorLog(CommandHandleException ex)
		{
			ErrorLog(string.Format("Error {0}, {1}", ex.result, ex.Message), true, ex.StackTrace);
		}

		/// <summary>
		/// 핸들러 내에서 발생한 에러 메세지와 에러 경로 로깅 요청 함수
		/// </summary>
		/// <param name="sMessage">에러 메세지</param>
		/// <param name="bLoggingTrace">에러 경로 저장 여부</param>
		/// <param name="sStackTrace">에러 발생 경로</param>
		protected void ErrorLog(string sMessage, bool bLoggingTrace, string? sStackTrace)
		{
			StringBuilder sb = new StringBuilder();

			ErrorFrom(sb);

			SFLogUtil.Error(GetType(), sb, sMessage, bLoggingTrace, sStackTrace);
		}

		/// <summary>
		/// 에러가 발생한 클라이언트에 대한 정보를 로그에 추가 하는 함수(하위 핸들러에서 발생하는 에러는 오버라이딩하여 추가 처리 필요)
		/// </summary>
		/// <param name="sb">에러 메세지를 저장하는 객체</param>
		protected virtual void ErrorFrom(StringBuilder sb)
		{
			sb.Append("# PeerId : ");
			sb.Append(clientPeer!.id);
			sb.AppendLine();
		}
	}
}
