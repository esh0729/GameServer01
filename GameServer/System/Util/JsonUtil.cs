using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LitJson;

namespace GameServer
{
	/// <summary>
	/// Json 관련 추가 기능을 제공하는 클래스
	/// </summary>
	public static class JsonUtil
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member functions

		/// <summary>
		/// 오브젝트 타입의 JsonData 객체를 생성하는 함수
		/// </summary>
		/// <returns>오브젝트 타입의 JsonData 객체 반환</returns>
		public static JsonData CreateObject()
		{
			JsonData jsonData = new JsonData();
			jsonData.SetJsonType(JsonType.Object);

			return jsonData;
		}

		/// <summary>
		/// 배열 타입의 JsonData 객체를 생성하는 함수
		/// </summary>
		/// <returns>배열 타입의 JsonData 객체 반환</returns>
		public static JsonData CreateArray()
		{
			JsonData jsonData = new JsonData();
			jsonData.SetJsonType(JsonType.Array);

			return jsonData;
		}
	}
}
