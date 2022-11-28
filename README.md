# GameServer01
- C# .Net Core를 사용한 멀티스레드 게임 서버  
- 네트워크 통신은 TAP 소켓 서버 방식 사용(https://github.com/esh0729/TCPNetworkLibrary 프로젝트의 TAPSocket 사용)  

# 프로젝트 정보
- ServerFramework : 네트워크 통신에 필요한 기본 클래스 구현 및 게임 서버 동작에 필요한 지원 클래스 구현 프로젝트
- GameServer : 게임 서버가 동작하는 프로젝트  
- ClientCommon : 클라이언트와 통신에 필요한 프로토콜을 관리하는 프로젝트

# 내부 구조
![슬라이드1](https://user-images.githubusercontent.com/100393621/204170881-a321b310-e6fb-40d1-b6be-ea3ed980c14b.PNG)
![슬라이드2](https://user-images.githubusercontent.com/100393621/204170883-82359870-d345-46f7-ba54-68dd2a70cdd1.PNG)
![슬라이드3](https://user-images.githubusercontent.com/100393621/204170886-9d5244b7-0d6f-4d04-a2c1-4d7009ad8434.PNG)
