# ApartmentEscapePublic
![AppIconwide](https://github.com/lsy1304/A3-Team-Project/blob/Dev2/Assets/Image/%EB%B0%9B%EC%9D%80%EA%B1%B0.png)

<br><br>

## 목차

| [프로젝트 소개](#프로젝트-소개) |
| :---: |
| [사용 기술 스택](#사용-기술-스택) |
| [기술적 고민과 트러블 슈팅](#기술적-고민과-트러블-슈팅) |
| [만든 사람들](#만든-사람들) |

<br><br>

* * *

<br><br>

# 프로젝트 소개

### [YouTube]

<br><br>

### 누군가가 나를 납치했다! 무슨 일이 있어도 탈출해야 한다!

![슬라이드0001](https://github.com/user-attachments/assets/af2ed728-5b64-4843-96fc-5500a5fecc4f)
![슬라이드0002](https://github.com/user-attachments/assets/c0aefba6-e9c1-4f37-99bc-cca37caef4d9)
![슬라이드0003](https://github.com/user-attachments/assets/1be7fa49-dba8-4cf8-a11a-6d86d4b9b658)
![슬라이드0004](https://github.com/user-attachments/assets/65caefeb-4769-445b-b082-fe7e720c8d57)

| 게임명 | 아파트 탈출 |
| :---: | :---: |
| 장르 | 시리어스 횡스크롤 방탈출 |
| 개발 환경 | Unity 2022.3.17f1 |
| 타겟 플랫폼 | PC |
| 개발 기간 | 2024.06.27 ~ 2024.8.14 |

[목차로 돌아가기](목차)

<br><br>
---
<br><br>
# 사용 기술 스택
| 기술 스택 | 사용 이유 |
|:---:|:---:|
|ScriptableObject| 퍼즐과 아이템의 일괄적인 관리|
|FSM|플레이어 상태의 유연한 전환|
|CSV|수정에 용이하여 팀원과의 협업 도모|
|Localization|다국어 지원|
|JSON|데이터 저장 / 불러오기|
|ObjectPool|자주 사용하는 음향 효과를 미리 생성 / 관리하여 최적화|
|ShaderGraph|공포스러운 분위기 연출|

[목차로 돌아가기](목차)
<br><br>

---

<br><br>
# 기술적 고민과 트러블 슈팅

### 기술적 고민

#### 1. 각기 다른 퍼즐의 퍼즐 작동 방식 통일
  - 키패드 퍼즐 / 슬라이드 퍼즐 / 숫자 퍼즐 / 스위치 퍼즐 각각 작동 방식이 다름.
  - 이를 통일시켜서 상호작용 하는 쪽에서 접근하기 편하게 만들어줘야 함.

    → 퍼즐의 공통된 요소 & 동작을 Puzzle 스크립트에 작성, 각 퍼즐 스크립트에 부모 클래스로서 상속

    ```cs
    public abstract class Puzzle : MonoBehaviour
    {
      [SerializeField] private PuzzleSO PuzzleData;
      [SerializeField] private Transform itemSpawnPos;
      public PuzzleSO puzzleData
      {
          get { return PuzzleData; }
      }
      public bool IsUnlock = false;
      protected int[] CurrentInput;
      protected event Action OnAnswerCheckEvent;
      public event Action OnClearEvent;
      [SerializeField] private List<Gimmick> UnlockGimmicks;
      [SerializeField] private InteractionEvent interactionEvent;

      void Start()
      {
          interactionEvent = GetComponent<InteractionEvent>();
      }

      protected void UnlockGimmick()
      {
          IsUnlock = true;
          if(interactionEvent != null)
              DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.FirstInteract));
          if (UnlockGimmicks != null)
          {
              foreach (Gimmick gimmick in UnlockGimmicks)
              {
                  gimmick.Unlock();
              }
          }
          if (puzzleData.IsItemSpawnable)
          {
              GameObject go = Instantiate(puzzleData.SpawnItem, itemSpawnPos, true);
              go.transform.localPosition = Vector2.zero;
          }
          CallClearEvent();
          OnClearEvent = null;
      }    

      protected void CallAnswerCheckEvent()
      {
          OnAnswerCheckEvent?.Invoke();
      }

      private void CallClearEvent()
      {
          OnClearEvent?.Invoke(); 
      }

      public virtual void ResetValue()
      {
        
      }  
    }
    ```
#### 2. 다국어 지원에 따른 CSV 파일 교체
 - 현재 언어 설정에 따라서 대사 언어도 바뀌어야 함. + 대사 데이터는 CSV로 관리 하고 있음. = 언어 설정에 따라서 CSV 파일을 교체해야함.

     → 언어 설정이 바뀔 때마다, 현재 언어 상태에 따라서 다른 CSV 파일을 불러오도록 구현
    ```cs
    protected void Awake()
    {
        UI = GetComponent<DialogueUI>();
        theParser = GetComponent<DialogueParser>();
        dialogueDic = theParser.Parse(csvFileName);
    
        IsFinish = true;
        LocalizationSettings.SelectedLocaleChanged += ReParse;
    }

    private void ReParse(Locale locale)
    {
        dialogueDic.Clear();
        dialogueDic = theParser.Parse(csvFileName);
    }
    ```
    
    ```cs
    public Dictionary<int, Dialogue> Parse(string csvFileName)
    {
        Dictionary<int, Dialogue> dialogueList = new Dictionary<int, Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName + LocaleManager.Instance.CurrentLocaleIdx.ToString());
    
        string[] data = csvData.text.Split('\n');
        for(int i = 1; i<data.Length;)
        {
            string[] row = data[i].Split(',');
            Dialogue newDialogue = new Dialogue();

            int key = Convert.ToInt32(row[0]);
            newDialogue.name = row[1];
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);
                if (++i < data.Length)
                {
                    row = data[i].Split(',');
                }
                else break;
            } while (row[0].ToString() == "");
        
            newDialogue.context = contextList.ToArray();
            dialogueList.Add(key, newDialogue);
        }
        return dialogueList;
    }
    ```

[목차로 돌아가기](목차)
<br><br>

<br><br>
### 트러블 슈팅

#### 1. 퍼즐 조작시 플레이어 조작이 무시되지 않거나(1-1), 조작이 복원되어야 하는 상황에서 복원되지 않는(1-2) 문제
  - (1-1)의 경우, PlayerController와 PlayerInteract에서 플레이어의 조작을 무시하는 로직이 없어서 생김

    → PlayerController에서 현재 상황이 조작 가능한 상황인지 체크하는 bool 변수를 생성, 조작 키가 입력되면 해당 조건을 참조하도록 수정
    ```cs
    public static bool IsInteractInterrupted => DialogueManager.Instance.UI.IsDialogueOn || MapManager.Instance.IsOnMoving || UIManager.Instance.IsUIOpen || !inputEnable;
    ```
  - (1-2)의 경우, PlayerInteraction에서 조작 무시 조건을 true로 변환하고 대사창에서 false로 바꿔주는데, 퍼즐에 대사창이 없으면 복원되지 않음.

    → 퍼즐에 대사가 없을 때, return 하기 전에 PlayerController.InputEnable을 true로 변경하는 로직 추가
    ```cs
    public void ShowDialogue(Dialogue[] p_dialogues)
    {
      if (p_dialogues == null || p_dialogues[0].context[0] == string.Empty || p_dialogues[0].context[0] == "\r")
      {
          controller.EnableInput(true);
          return;
      }
      isDialogue = true;
      dialogues = p_dialogues;
      textDisplay = StartCoroutine(TypeWriter());
    }
    ```
#### 2. 세이브 이후 새 게임으로 진입할 때, 일부 아이템이 생성되지 않아 진행이 막히는 문제
  - 데이터 저장에 사용될 아이템의 획득 여부를 ScriptableObject에 들어있음. + ScriptableObject는 플레이가 꺼져도 수정된 값이 유지됨. + 게임 씬 진입할 때, 해당 아이템이 이미 획득된 아이템인 경우 파괴 = 새 게임을 기준으로 획득하지 않은 아이템이 파괴되어버림.

     → 메인 메뉴로 나갈 때, 아이템 ScriptableObject의 획득 여부 데이터를 초기화 하는 로직 추가
      ```cs
      public void LoadMainMenu(bool isEnding)
      {
        if(!isEnding) DataManager.Instance.ResetItemValue();
        StartCoroutine(LoadAsyncScene(0));
      }
      ```
      
      ```cs
      public void ResetItemValue()
      {
        foreach (ItemSO data in items)
        {
            data.IsObtained = false;
        }
      }
      ```
#### 3. 상호작용 오브젝트가 겹칠 때, 동작이 다소 부자연스러운 문제
  - PlayerInteraction에서 상호작용 가능한 오브젝트가 플레이어의 상호작용 영역 안에 존재하면, 모든 오브젝트에 상호작용 함. + 상호작용하는 오브젝트에 InteractionEvent가 있으면 대사를 출력함. = InteractionEvent가 여러개 감지되느 경우 대사창이 고장남.

    → 플레이어의 상호작용 가능한 영역 안에 여러 개의 오브젝트가 검출되는 경우, 가장 가까운 것만 상호작용 하도록 수정
    ```cs
    if (PlayerController.IsInteractInterrupted) return;
    Collider2D[] hits = Physics2D.OverlapCircleAll(playerTransform.position, interactionDistance, interactableLayerMask);

    if (hits.Length == 0) return;

    Collider2D closestHit = null;
    float closestDistance = Mathf.Infinity;

    foreach (Collider2D hit in hits)
    {
        float distance = Vector2.Distance(playerTransform.localPosition, hit.transform.localPosition);
        if (distance < closestDistance)
        {
            closestDistance = distance;
            closestHit = hit;
        }
    }
    ```
#### 4. 화면 해상도가 변경되면 슬라이드 퍼즐이 고장나는 문제
  - 기존 로직 : 타일이 클릭되면 Ray를 상하좌우로 발사, 빈 공간이 감지되면 해당 방향으로 정해진 수치만큼 이동

    → 해상도가 바뀌면 퍼즐의 RectTransform이 바뀌므로 비율이 깨져버림

  - 개선 로직 : Ray를 전부 제거하고, Awake에서 현재 타일의 위치를 2차원 배열로 저장, 빈칸을 기준으로 움직일 수 있는 타일의 인덱스 값을 저장 / 사용해서 움직일 수 있는 타일이 클릭되는 경우, 빈 타일과 해당 타일의 이미지를 교환

    → 애니메이션이 없다는 점을 이용하여 서로 값만 변경하도록 수정 (애니메이션이 있는 경우 AnimationCurve를 사용하여 움직이는 효과도 충분히 구현 가능)

    ```cs
    private void Awake()
    {
      for(int i =0;i<tiles.transform.childCount; i++)
      {
          slideTiles[i] = tiles.transform.GetChild(i).GetComponent<SlideTile>();
      }

      for (int i = 0; i < names.Length; i++)
      {
          names[i] = slideTiles[i].name = slideTiles[i].text.text;
          if (!slideTiles[i].img.enabled) emptyIndex = i;
          array[i / 3, i % 3] = slideTiles[i].idx = i;
      }
      UpdateMovableTileIdx();
      OnAnswerCheckEvent += CheckPuzzleAnswer;
    }
    ```

    ```cs
    public void UpdateTileValue(int idx)
    {
      bool IsOkToMove = false;
  
      var Tile = slideTiles[idx];
      var Tile2 = slideTiles[emptyIndex];
      for (int i = 0; i < movableIdx.Count; i++)
      {
          if (Tile.idx == movableIdx[i])
          {
              IsOkToMove = true;
              break;
          }
      }
      if (!IsOkToMove) return;
  
      Tile2.text.enabled = true;
  
      var tempTxt = Tile.text.text;
      Tile.text.text = Tile2.text.text; ;
      Tile2.text.text= tempTxt;
  
      Tile.text.enabled = false;
  
      Tile2.img.enabled = true;
  
      var tempSprite = Tile.img.sprite;
      Tile.img.sprite = Tile2.img.sprite;
      Tile2.img.sprite = tempSprite;
  
      Tile.img.enabled = false;
  
      var temp = names[idx];
      names[idx] = names[emptyIndex];
      names[emptyIndex] = temp;
  
      emptyIndex = Tile.idx;
  
      UpdateMovableTileIdx();
      CallAnswerCheckEvent();
    }
    ```

<br><br>

<br><br>
### 만든 사람들

#### 이승영 : 퍼즐, 대사, 기믹, 로컬라이제이션
#### 문주원 : 상호작용, 사운드, 플레이어, 셰이더
#### 배성철 : 맵 디자인, 엔딩, 아이템, 타일 맵
#### 안지수 : 인벤토리, UI, 이미지
<br><br>
