# MapManager
## 맵 데이터 관리
**by 배성철**

- 맵 데이터를 관리하고 표시하는 기능을 담당한다..

- **``SetRoomsKeyValue()``**
  - Awake에서 현재 방의 리스트를 딕셔너리 자료구조로 저장
 
- **``FindRoom(int numberInList)``**
  - 딕셔너리에서 방을 찾는 메서드
 
- **``UpdateRooms()``**
  - 플레이어가 맵을 이동할 때, 이동 가능한 방의 딕셔너리를 업데이트
 
- **``CheskIfRoomIsInRange(bool isMaxRange, int numberInList)``**
  - 해당 방이 플레이어를 기준으로 하여 범위 내에 있는지 확인하는 메서드
 
- **``MoveTo(RoomType roomType, int roomNumber)``**
  - 플레이어가 맵을 이동할 때 맵을 전환하는 메서드
 
- **``InitRoom(RoomType roomType, int roomNumber)``**
  - 게임을 불러올 때 초기 위치를 세팅하는 메서드
