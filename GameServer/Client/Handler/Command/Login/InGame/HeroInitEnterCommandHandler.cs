using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientCommon;

namespace GameServer
{
	/// <summary>
	/// 영웅 초기 입장 클라이언트 명령 처리 클래스
	/// </summary>
	public class HeroInitEnterCommandHandler : InGameCommandHandler<HeroInitEnterCommandBody,HeroInitEnterResponseBody>
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		protected override bool globalLockRequired
		{
			get { return true; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 클라이언트 명령 처리 함수
		/// </summary>
		protected override void OnInGameCommandHandle()
		{
			DateTimeOffset currentTime = DateTimeUtil.currentTime;

			//
			// 영웅 검사
			//

			HeroInitEnterParam? param = m_myHero!.entranceParam as HeroInitEnterParam;
			if (param == null)
				throw new CommandHandleException(kResult_Error, "초기 입장을 할 수 없습니다.");

			Continent continent = param.continent;
			Vector3 position = param.position;
			float fYRotation = param.yRotation;

			ContinentInstance? continentInstance = Cache.instance.GetContinentInstance(continent.id);
			if (continentInstance == null)
				throw new CommandHandleException(kResult_Error, "대륙인스턴스가 존재하지 않습니다. continentId = " + continent.id);

			lock (continentInstance.syncObject)
			{
				m_myHero.SetPosition(position, fYRotation);
				continentInstance.Enter(m_myHero);

				// 응답 객체 생성
				HeroInitEnterResponseBody resBody = new HeroInitEnterResponseBody();
				resBody.placeInstanceId = continentInstance.instanceId;

				resBody.position = m_myHero.position;
				resBody.yRotation = m_myHero.yRotation;

				resBody.heroes = Hero.ToPDHeroes(continentInstance.GetInterestedHeroes(m_myHero.sector!, m_myHero.id)).ToArray();

				// 응답 송신
				SendResponseOK(resBody);
			}
		}
	}
}
