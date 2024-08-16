# LocaleManager
## Localization 패키지를 사용하여 언어 변경
**by 이승영**

- Localization 패키지를 사용하여 게임의 언어를 변경합니다..
 
- **``FindComponent(Scene arg0, LoadSceneMode arg1)``**
  - 씬이 바뀔 때 호출되는 메서드로 로딩 된 씬의 언어 설정 UI를 연결합니다.
  - 게임 기동 시, 언어를 마지막 언어 설정으로 변경합니다 & UI와 동기화.
 
- **``ChangeLocale()``**
  - 언어 설정 UI에서 버튼이 눌리면 언어를 변경합니다.
 
- **``ChangeRoutine(int idx)``**
  - 언어 설정 초기화를 대기한 후, 언어를 변경합니다.
