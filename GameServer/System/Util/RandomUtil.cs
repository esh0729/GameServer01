using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// 멀티스레드에 안전한 난수 생성 클래스
	/// </summary>
	public static class RandomUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static Random s_random;
		private static object s_syncObject;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static constructors

		/// <summary>
		/// 
		/// </summary>
		static RandomUtil()
		{
			s_random = new Random();
			s_syncObject = new object();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		//
		// int
		//

		/// <summary>
		/// 상한값 보다 작은 임의의 정수를 반환하는 함수
		/// </summary>
		/// <param name="nMaxValue">난수 상한값</param>
		/// <returns>0 이상, 상한값 미만의 int 타입의 임의로 생성된 정수 반환</returns>
		public static int NextInt(int nMaxValue)
		{
			lock (s_syncObject)
			{
				return s_random.Next(nMaxValue);
			}
		}

		/// <summary>
		/// 하한값과 상한값을 가진 임의의 정수를 반환하는 함수
		/// </summary>
		/// <param name="nMinValue">난수 하한값</param>
		/// <param name="nMaxValue">난수 상한값</param>
		/// <returns>하한값 이상, 상한값 미만의 int 타입의 임의로 생성된 정수 반환</returns>
		public static int NextInt(int nMinValue, int nMaxValue)
		{
			lock (s_syncObject)
			{
				return s_random.Next(nMinValue, nMaxValue);
			}
		}

		//
		// float
		//

		/// <summary>
		/// 상한을 가진 임의의 소수를 반환하는 함수
		/// </summary>
		/// <param name="fMaxValue">난수 상한값</param>
		/// <returns>0 이상, 상한값 이하의 float 타입의 임의로 생성된 소수 반환</returns>
		public static float NextFloat(float fMaxValue)
		{
			lock (s_syncObject)
			{
				return (float)(s_random.NextDouble() * fMaxValue);
			}
		}

		/// <summary>
		/// 하한과 상한을 가진 임의의 소수를 반환하는 함수
		/// </summary>
		/// <param name="fMinValue">난수 하한값</param>
		/// <param name="fMaxValue">난수 상한값</param>
		/// <returns>하한 이상, 상한 이하의 float 타입의 임의로 생성된 소수 반환</returns>
		public static float NextFloat(float fMinValue, float fMaxValue)
		{
			lock (s_syncObject)
			{
				return fMaxValue - (float)(s_random.NextDouble() * (fMaxValue - fMinValue));
			}
		}
	}
}
