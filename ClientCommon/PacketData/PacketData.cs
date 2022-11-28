using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// (추상 클래스)클래스 단위의 데이터 송수신에 처리될 데이터의 직렬화, 역직렬화 함수를 지원하는 클래스
	/// </summary>
	public abstract class PacketData
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화에 필요한 처리를 시작하는 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		public void SerializeRaw(PacketWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");

			Serialize(writer);
		}

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="wirter">직렬화 처리 객체</param>
		protected virtual void Serialize(PacketWriter writer)
		{

		}

		/// <summary>
		/// 역직렬화에 필요한 처리를 시작하는 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		public void DeserializeRaw(PacketReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			Deserialize(reader);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected virtual void Deserialize(PacketReader reader)
		{

		}
	}
}
