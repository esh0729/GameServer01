using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 영웅 로그인 명령 데이터 관리 클래스
	/// </summary>
	public class HeroLoginCommandBody : CommandBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public Guid heroId;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			writer.Write(heroId);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			heroId = reader.ReadGuid();
		}
	}

	/// <summary>
	/// 영웅 로그인 명령 응답 데이터 관리 클래스
	/// </summary>
	public class HeroLoginResponseBody : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		//
		// 영웅 정보
		//

		public Guid heroId;
		public string? name;
		public int characterId;

		//
		// 위치 정보
		//

		public int enterContinentId;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 함수</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			//
			// 영웅 정보
			//

			writer.Write(heroId);
			writer.Write(name);
			writer.Write(characterId);

			//
			// 위치 정보
			//

			writer.Write(enterContinentId);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 함수</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			//
			// 영웅 정보
			//

			heroId = reader.ReadGuid();
			name = reader.ReadString();
			characterId = reader.ReadInt32();

			//
			// 위치 정보
			//

			enterContinentId = reader.ReadInt32();
		}
	}
}
