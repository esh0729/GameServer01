using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 영웅 초기 입장 명령 데이터 관리 클래스
	/// </summary>
	public class HeroInitEnterCommandBody : CommandBody
	{
	}

	/// <summary>
	/// 영웅 초기 입장 명령 응답 데이터 관리 클래스
	/// </summary>
	public class HeroInitEnterResponseBody : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public Guid placeInstanceId;

		public PDVector3 position;
		public float yRotation;

		public PDHero[]? heroes;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			writer.Write(placeInstanceId);

			writer.Write(position);
			writer.Write(yRotation);

			writer.Write(heroes);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			placeInstanceId = reader.ReadGuid();

			position = reader.ReadPDVector3();
			yRotation = reader.ReadSingle();

			heroes = reader.ReadPacketDatas<PDHero>();
		}
	}
}
