# SceneLoadManager
## 씬 비동기 로드 관련
**by 이승영**

- 씬을 비동기 방식으로 로드합니다.

- **``StartNewGame()``**
  - 현재 상태를 (새 게임 시작) 상태로 설정합니다.
 
- **``ContinewGame()``**
  - 현재 상태를 (이어하기) 상태로 설정합니다.
 
- **``LoadAsyncScene(int sceneIdx)``**
  - 화면이 어두워지는 효과 & 로딩 바 & 씬 불러오기 기능을 담당합니다.
 
- **``LoadMainMenu(bool isEnding)``**
  - 메인 메뉴로 이동합니다. (엔딩인 경우 ResetItemValue 호출)
 
- **``StartLoadingEnding(int idx)``**
  - idx를 사용해서 해당하는 엔딩을 표시합니다.
