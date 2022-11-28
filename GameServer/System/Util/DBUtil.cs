using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	/// <summary>
	/// DB 작업에 대한 추가 기능을 제공하는 클래스
	/// </summary>
	public static class DBUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		//
		// Guid
		//

		/// <summary>
		/// 매개변수를 Guid 타입으로 변환시키는 함수
		/// </summary>
		/// <param name="obj">변환 시킬 변수</param>
		/// <returns>변환된 Guid 타입 구조체 반환</returns>
		public static Guid ToGuid(object? obj)
		{
			return ToGuid(obj, Guid.Empty);
		}

		/// <summary>
		/// 매개변수를 Guid 타입으로 변환시키는 함수
		/// </summary>
		/// <param name="obj">변환 시킬 변수</param>
		/// <param name="whenNullValue">전달된 매개 변수가 null일 경우 설정될 값</param>
		/// <returns>변환 된 Guid 구조체 반환</returns>
		public static Guid ToGuid(object? obj, Guid whenNullValue)
		{
			if (obj == null)
				return whenNullValue;

			return obj != DBNull.Value ? (Guid)obj : whenNullValue;
		}

		//
		// DateTimeOffset
		//

		/// <summary>
		/// 매개변수를 DateTimeOffset 타입으로 변환시키는 함수
		/// </summary>
		/// <param name="obj">변환 시킬 변수</param>
		/// <returns>변환된 DateTimeOffset 타입 구조체 반환</returns>
		public static DateTimeOffset ToDateTimeOffset(object? obj)
		{
			return ToDateTimeOffset(obj, DateTimeOffset.MinValue);
		}

		/// <summary>
		/// 매개변수를 DateTimeOffset 타입으로 변환시키는 함수
		/// </summary>
		/// <param name="obj">변환 시킬 변수</param>
		/// <param name="whenNullValue">전달된 매개 변수가 null일 경우 설정될 값</param>
		/// <returns>변환된 DateTimeOffset 타입 구조체 반환</returns>
		public static DateTimeOffset ToDateTimeOffset(object? obj, DateTimeOffset whenNullValue)
		{
			if (obj == null)
				return whenNullValue;

			return obj != DBNull.Value ? (DateTimeOffset)obj : whenNullValue;
		}
	}
}
