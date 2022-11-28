using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 영웅 이동 종료 명령 데이터 관리 클래스
	/// </summary>
	public class HeroMoveEndCommandBody : CommandBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		public Guid placeInstanceId;
		public PDVector3 position;
		public float yRotation;

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
		}
	}

	/// <summary>
	/// 영웅 이동 종료 명령 응답 데이터 관리 클래스
	/// </summary>
	public class HeroMoveEndResponseBody : ResponseBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public PDHero[]? addedHeroes;
		public Guid[]? removedHeroes;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 직렬화 처리 함수
		/// </summary>
		/// <param name="writer">직렬화 처리 객체</param>
		protected override void Serialize(PacketWriter writer)
		{
			base.Serialize(writer);

			writer.Write(addedHeroes);
			writer.Write(removedHeroes);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			addedHeroes = reader.ReadPacketDatas<PDHero>();
			removedHeroes = reader.ReadGuids();
		}
	}
}
