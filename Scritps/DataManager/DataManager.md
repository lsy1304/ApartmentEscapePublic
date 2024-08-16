# DataManager
## 데이터 저장 / 불러오기 관리
**by 이승영**

- JSON을 활용하여 세이브 데이터를 저장, 불러오는 기능을 담당합니다.

- **``Awake()``**
  - 초기에 SaveData에서 사용될 아이템 전체 리스트를 딕셔너리의 자료구조로 생성합니다.
 
- **``StartGame()``**
  - 게임이 진입될 때, 현재 상황에 따라 새 게임 시작 또는 데이터를 불러옵니다.

- **``GetCurRoomData(int roomNumber, RoomType roomType)``**
  - 게임을 저장할 때 MapManager의 현재 Map 데이터를 받아옵니다.
 
- **``GetCurInventoryData(ItemSlot[] slots)``**
  - 게임을 저장할 때, UIInventory에서 현재 인벤토리 데이터를 받아옵니다.
 
- **``CallDataSaveEveent()``**
  - 게임을 저장할 때, 구독되어 있는 모든 메서드를 호출합니다.
 
- **``SaveData()``**
  - 현재 게임 위치, 인벤토리, 퍼즐 해결 여부, 아이템 획득 여부를 JSON을 활용하여 세이브 파일로 저장합니다.

- **``LoadData()``**
  - 세이브 파일을 불러오고, 해당 데이터를 게임에서 활용할 수 있도록 할당해줍니다.

- **``ResetValue()``**
  - 메인메뉴로 나갈 때, 초기화되지 않던 아이템 ScriptableObject의 데이터를 초기화합니다..
