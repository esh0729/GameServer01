using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 관심 영역 변경 서버 이벤트 데이터 관리 클래스
	/// </summary>
	public class SEBInterestedAreaChangedEventBody : ServerEventBody
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
