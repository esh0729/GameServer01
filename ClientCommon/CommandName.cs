﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommon
{
	/// <summary>
	/// 클라이언트 명령(응답이 필요한 요청) 타입
	/// </summary>
	public enum CommandName : int
	{
        None = 0,

        //
        // 계정
        //

        // 계정 로그인
        Login,
        // 로비 정보
        LobbyInfo,

        //
        // 영웅
        //

        // 영웅 생성
        HeroCreate,
        // 영웅 로그인
        HeroLogin,
        // 영웅 로그아웃
        HeroLogout,
        // 영웅 초기입장
        HeroInitEnter,

        // 영웅 이동시작
        HeroMoveStart,
        // 영웅 이동
        HeroMove,
        // 영웅 이동종료
        HeroMoveEnd,

        //
        // 행동
        //

        // 특정행동시작
        Action
    }
}
