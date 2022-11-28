# GameServer01
- C# .Net Core를 사용한 멀티스레드 게임 서버  
- 네트워크 통신은 TAP 소켓 서버 방식 사용(https://github.com/esh0729/TCPNetworkLibrary 프로젝트의 TAPSocket 사용)  

# 프로젝트 정보
- ServerFramework : 네트워크 통신에 필요한 기본 클래스 구현 및 게임 서버 동작에 필요한 지원 클래스 구현 프로젝트
- GameServer : 게임 서버가 동작하는 프로젝트  
- ClientCommon : 클라이언트와 통신에 필요한 프로토콜을 관리하는 프로젝트

# 내부 구조
![슬라이드1](https://user-images.githubusercontent.com/100393621/204171418-4d9aec84-1b66-47c4-adc2-79071abf94fb.PNG)
![슬라이드2](https://user-images.githubusercontent.com/100393621/204171423-5ccdd51a-4859-46c7-933c-8627df7c7cc1.PNG)
![슬라이드3](https://user-images.githubusercontent.com/100393621/204171425-f40d918b-10d1-489c-844d-4e84e9d34dc5.PNG)
