using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 로비 정보 명령 데이터 관리 클래스
	/// </summary>
	public class LobbyInfoCommandBody : CommandBody
	{
	}

	/// <summary>
	/// 로비 정보 명령 응답 데이터 관리 클래스
	/// </summary>
	public class LobbyInfoResponseBody : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public PDLobbyHero[]? heroes;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			writer.Write(heroes);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			heroes = reader.ReadPacketDatas<PDLobbyHero>();
		}
	}
}
