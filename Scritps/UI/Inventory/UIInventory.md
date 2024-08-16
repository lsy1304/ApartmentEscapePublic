# UIInvetentory
## 인벤토리 관리
**by 안지수**

- 플레이어가 사용할 인벤토리를 관리합니다.

- **``InitailizeSlots()``**
  - 인벤토리에서 슬롯을 사용할 수 있도록 초기화 합니다.
 
- **``ClearSelectedItemInfo()``**
  - 선택된 슬롯의 데이터를 초기화 합니다.
 
- **``AddItem(ItemSO itemData, int slotIndex = -1)``**
  - 인벤토리에서 빈칸을 찾아 해당 슬롯에 아이템을 추가합니다.
 
- **``UpdateUI()``**
  - 인벤토리UI를 업데이트 합니다.
 
- **``SelectItem(int index)``**
  - 슬롯이 선택되면 해당 슬롯의 아이템 데이터를 UI에 반영합니다.
 
- **``RemoveItem(int index)``**
  - 해당 아이템을 인벤토리에서 제거합니다.
 
- **``ToggleCombineMode()``**
  - 아이템 조합 상태로 전환합니다.
 
- **``ClearAllSlot()``**
  - 인벤토리의 모든 슬롯의 데이터를 제거합니다.
