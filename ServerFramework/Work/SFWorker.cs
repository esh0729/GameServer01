using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
	/// <summary>
	/// 특정 작업을 큐잉하여 순서대로 실행 시키는 클래스
	/// </summary>
	public class SFWorker
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private object m_syncObject;

		private Task? m_task;

		private Queue<ISFWork> m_works;

		private ManualResetEvent m_progressSignal;
		private ManualResetEvent m_endSignal;

		private bool m_bRunning;
		private bool m_bDisposed;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public SFWorker()
		{
			m_syncObject = new object();

			m_task = null;

			m_works = new Queue<ISFWork>();

			m_progressSignal = new ManualResetEvent(false);
			m_endSignal = new ManualResetEvent(false);

			m_bRunning = false;
			m_bDisposed = false;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public bool resting
		{
			get
			{
				lock (m_syncObject)
				{
					return m_works.Count <= 0;
				}
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 작업자 실행 함수
		/// </summary>
		public void Start()
		{
			m_bRunning = true;

			m_progressSignal.Reset();
			m_endSignal.Set();

			// 작업을 처리하는 함수 별도의 스레드에서 실행
			// 작업자 스레드의 경우 프로그램이 종료될때까지 실행 되니 TaskCreationOptions.LongRunning 옵션 추가 하여 별도의 스레드 생성(ThreadPool 방지)
			m_task = new Task(Running, TaskCreationOptions.LongRunning);
			m_task.Start();
		}

		/// <summary>
		/// 작업자 중지 함수
		/// 큐잉 중인 작업 모두 처리 이후 리소스 해제
		/// </summary>
		public void Stop()
		{
			lock (m_syncObject)
			{
				// 이미 작동이 중단되었을 경우 리턴
				if (!m_bRunning)
					return;

				m_bRunning = false;
			}

			Dispose();
		}

		/// <summary>
		/// 작업 추가 함수
		/// </summary>
		/// <param name="work">작업 객체</param>
		public void Add(ISFWork work)
		{
			lock (m_syncObject)
			{
				// 동작중이지 않을 경우 리턴
				if (!m_bRunning)
					return;

				m_works.Enqueue(work);

				// 작업이 없던 상태에서 작업이 들어온 경우 작업 진행 신호 받음 설정
				if (m_works.Count == 1)
				{
					// 작업이 진행될 수 있도록 작업 신호 설정
					m_progressSignal.Set();
					// 모든 작업이 끝날때까지 종료 신호 차단 설정
					m_endSignal.Reset();
				}
			}
		}

		/// <summary>
		/// 반복문을 돌며 상태에 따라 작업 실행 함수를 호출하는 함수
		/// 별도의 스레드에서 진행
		/// </summary>
		private void Running()
		{
			while (!m_bDisposed)
			{
				// 작업큐에 작업이 큐잉될때까지 대기
				m_progressSignal.WaitOne();

				// 작업 실행
				RunWork();
			}
		}

		/// <summary>
		/// 작업 실행 함수
		/// </summary>
		private void RunWork()
		{
			try
			{
				// 작업큐에서 첫번재 작업 호출 후 실행
				ISFWork work;
				lock (m_syncObject)
				{
					work = m_works.Peek();
				}
				work.Run();
			}
			catch
			{

			}

			try
			{
				lock (m_syncObject)
				{
					// 작업큐의 첫번째 작업 삭제
					m_works.Dequeue();

					// 작업큐가 비었을 경우
					if (m_works.Count <= 0)
					{
						// 작업 진행 신호 차단 설정
						m_progressSignal.Reset();
						// 리소스 해제 처리가 될수 있도록 종료 신호 설정
						m_endSignal.Set();
					}
				}
			}
			catch
			{

			}
		}

		/// <summary>
		/// 리소스 해제 함수
		/// </summary>
		private void Dispose()
		{
			// 모든 작업이 끝나 종료 신호 설정 될때까지 대기
			m_endSignal.WaitOne();

			m_bDisposed = true;
			// 작업 스레드에서 while문 조건이 만족되어 대기 중일 경우 반복문이 종료 될수 있도록 작업 진행 신호 설정
			m_progressSignal.Set();

			// 작업 스레드가 종료 될때까지 대기
			m_task!.Wait();

			//
			//
			//

			m_task = null;
			m_progressSignal.Dispose();
			m_endSignal.Dispose();
		}
	}
}
