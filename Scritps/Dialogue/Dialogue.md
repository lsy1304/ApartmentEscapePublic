# DialogueManager
## CSV에서 추출된 데이터를 딕셔너리로 저장
**by 이승영**

- 딕셔너리로 저장된 대사 데이터를 필요한 만큼 반환합니다.
 
- **``ReParse(Locale locale)``**
  - 언어가 바뀔 때 호출되는 메서드로, 딕셔너리를 삭제하고 CSV 파일을 다시 로드합니다.
 
- **``GetDialogues(int startNum, int endNum)``**
  - 딕셔너리에서 startNum 부터 endNum 까지의 대사 데이터를 추출합니다.

---

# DialogueUI
## 대사를 UI에 표시
**by 이승영**

- DialogueManager에서 추출한 대사 데이터를 UI에 표시합니다.

- **``ShowDialogue(Dialogue[] p_dialogues)``**
  - 추출한 대사 데이터를 UI에 표시합니다. (대사 데이터 비어있는 경우 무시)
 
- **``TypeWriter()``**
  - 대사를 표시할 때, 한 줄씩, 한 글자 씩 순차적으로 표시합니다.

---
 
# DialogueParser
## CSV에서 데이터를 추출
**by 이승영**

- 딕셔너리로 저장된 대사 데이터를 필요한 만큼 반환합니다.

- **``Parse(string csvFileName)``**
  - 현재 언어에 맞는 CSV 파일을 로드 및 딕셔너리의 자료구조로 변환합니다.
