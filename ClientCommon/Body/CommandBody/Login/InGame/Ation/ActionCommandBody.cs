using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 행동 명령 데이터 관리 클래스
	/// </summary>
	public class ActionCommandBody : CommandBody
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		public int actionId;
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

			writer.Write(actionId);
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

			actionId = reader.ReadInt32();
			position = reader.ReadPDVector3();
			yRotation = reader.ReadSingle();
		}
	}

	/// <summary>
	/// 행동 명령 응답 데이터 관리 클래스
	/// </summary>
	public class ActionResponseBody : ResponseBody
	{
	}
}
