# GameServer01
- C# .Net Core를 사용한 멀티스레드 게임 서버  
- 네트워크 통신은 TAP 소켓 서버 방식 사용(https://github.com/esh0729/TCPNetworkLibrary 프로젝트의 TAPSocket 사용)  

# 프로젝트 정보
- ServerFramework : 네트워크 통신에 필요한 기본 클래스 구현 및 게임 서버 동작에 필요한 지원 클래스 구현 프로젝트
- GameServer : 게임 서버가 동작하는 프로젝트  
- ClientCommon : 클라이언트와 통신에 필요한 프로토콜을 관리하는 프로젝트  

# 동작
- GameServer 프로젝트 System/Program 클래스에서 동작 시작
- Server 객체 생성 후 Start 함수 호출 시 작업자 초기화 및 리소스 데이터 로드 이후 서버 시작
- 사용 인증 서버 : https://github.com/esh0729/AuthServer_DotNetCore 프로젝트
- 사용 데이터베이스  : MS-SQL(구현부 https://github.com/esh0729/SqlResource 프로젝트)
- 구동 영상 : https://www.youtube.com/watch?v=kXhNyMZ8bfA

# 내부 구조
![슬라이드1](https://user-images.githubusercontent.com/100393621/204775298-e0e16d36-1594-4000-80d7-679a30b9b26b.PNG)
![슬라이드2](https://user-images.githubusercontent.com/100393621/204775306-301778bd-3ba2-48dc-a26e-0d4cb9a7643c.PNG)
![슬라이드3](https://user-images.githubusercontent.com/100393621/204775310-8b144b46-bba0-41ba-b94d-3734af85aac6.PNG)
![슬라이드4](https://user-images.githubusercontent.com/100393621/204775320-6996e1ea-8fb7-4a6f-a00f-015d079a19f1.PNG)

