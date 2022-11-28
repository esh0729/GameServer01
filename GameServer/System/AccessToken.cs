using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using LitJson;

namespace GameServer
{
	public class AccessToken
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables

		private Guid m_userId;
		private string m_sAccessSecret;
		private string m_sSignature;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// 
		/// </summary>
		public AccessToken()
		{
			m_userId = Guid.Empty;
			m_sAccessSecret = string.Empty;
			m_sSignature = string.Empty;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Properties

		public Guid userId
		{
			get { return m_userId; }
		}

		public string? accessSecret
		{
			get { return m_sAccessSecret; }
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		/// <summary>
		/// 액세스 토큰 분석 함수
		/// </summary>
		/// <param name="sAccessToken">액세스 토큰</param>
		/// <returns>분석이 완료 되었을 경우 true, 분석에 실패 하였을 경우 false 반환</returns>
		public bool Parse(string sAccessToken)
		{
			JsonData token = JsonMapper.ToObject(sAccessToken);

			if (!Validate(token, "accessToken", out string sTokenData))
				return false;

			string[] tokenValidationParameters = sTokenData!.Split('.');
			
			string sPayload = Encoding.UTF8.GetString(Convert.FromBase64String(tokenValidationParameters[0]));
			JsonData payload = JsonMapper.ToObject(sPayload);

			if (!Validate(payload, "userId", out string sUserId))
				return false;

			m_userId = Guid.Parse(sUserId!);

			if (!Validate(payload, "accessSecret", out m_sAccessSecret))
				return false;

			m_sSignature = tokenValidationParameters[1];

			return true;
		}

		/// <summary>
		/// Json 데이터 내부 데이터 확인 함수 
		/// </summary>
		/// <param name="data">JsonData 객체</param>
		/// <param name="sKey">JsonData에서 꺼내올 key 값</param>
		/// <param name="sParameters">key값에 대응하는 값을 반환 받을 변수</param>
		/// <returns>데이터가 존재할 경우 true, 존재하지 않을 경우 false 반환</returns>
		private bool Validate(JsonData data, string sKey, out string sParameters)
		{
			sParameters = String.Empty;

			if (!data.ContainsKey(sKey))
				return false;

			sParameters = data[sKey].ToString();

			return true;
		}

		/// <summary>
		/// 액세스 토큰 검증 함수(JWT)
		/// </summary>
		/// <returns>검증에 성공했을 경우 true, 실패 하였을 경우 false 반환</returns>
		public bool Verify()
		{
			// 사용자ID와 엑세스시크릿을 가지고 있는 JsonData 생성
			JsonData payload = JsonUtil.CreateObject();
			payload["userId"] = m_userId.ToString();
			payload["accessSecret"] = m_sAccessSecret;

			// 생성된 JsonData 객체 Base64 기반의 문자열로 변환
			string sPayloadToBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload.ToJson()));

			// 시스템설정에 정의되어있는 비밀키를 사용하여 해시클래스 생성
			byte[] securityKey = Encoding.UTF8.GetBytes(SystemConfig.accessTokenSecurityKey);
			HMACMD5 hasher = new HMACMD5(securityKey);

			// Base64로 변환된 JsonData를 해싱 이후 Base64 기반의 문자열로 변환
			byte[] value = hasher.ComputeHash(Encoding.UTF8.GetBytes(sPayloadToBase64));
			string sSignature = Convert.ToBase64String(value);

			// 클라이언트에서 전달한 서명과 서버측에서 변환한 서명 비교
			if (m_sSignature != sSignature)
				return false;

			return true;
		}
	}
}
