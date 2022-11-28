using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 영웅 정보 저장 클래스
	/// </summary>
	public class PDHero : PacketData
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public Guid heroId;
		public string? name;
		public int characterId;
		public PDVector3 position;
		public float yRotation;
		public int actionId;

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
			writer.Write(name);
			writer.Write(characterId);
			writer.Write(position);
			writer.Write(yRotation);
			writer.Write(actionId);
		}

		/// <summary>
		/// 역직렬화 처리 함수
		/// </summary>
		/// <param name="reader">역직렬화 처리 객체</param>
		protected override void Deserialize(PacketReader reader)
		{
			base.Deserialize(reader);

			heroId = reader.ReadGuid();
			name = reader.ReadString();
			characterId = reader.ReadInt32();
			position = reader.ReadPDVector3();
			yRotation = reader.ReadSingle();
			actionId = reader.ReadInt32();
		}
	}
}
