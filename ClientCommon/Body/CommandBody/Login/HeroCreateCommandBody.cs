using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 영웅 생성 명령 데이터 관리 클래스
	/// </summary>
	public class HeroCreateCommandBody : CommandBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public string? name;
		public int characterId;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			writer.Write(name);
			writer.Write(characterId);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			name = reader.ReadString();
			characterId = reader.ReadInt32();
		}
	}

	/// <summary>
	/// 영웅 생성 명령 응답 데이터 관리 클래스
	/// </summary>
	public class HeroCreateResponseBody : ResponseBody
	{
	}
}
