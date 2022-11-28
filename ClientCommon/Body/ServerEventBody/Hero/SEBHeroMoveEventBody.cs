using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 다른 영웅 이동 서버 이벤트 데이터 관리 클래스
	/// </summary>
	public class SEBHeroMoveEventBody : ServerEventBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public Guid heroId;
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

			writer.Write(heroId);
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

			heroId = reader.ReadGuid();
			position = reader.ReadPDVector3();
			yRotation = reader.ReadSingle();
		}
	}
}
