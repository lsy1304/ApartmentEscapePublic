using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public enum RoomType
{
    Corridor,
    MainRoom,
    BedRoom,
    BathRoom
}
public class MapManager : Singleton<MapManager>
{
    private int curRoomNumber;
    private RoomType curRoomType;
    private Room curRoom;
    public float changeSpeed;
    private bool isOnMoving;
    private PlayerController controller;
    public bool IsOnMoving
    {
        get => isOnMoving;
        set
        {
            controller.CallMoveEvent(Vector2.zero);
            isOnMoving = value;
        }
    }

    private const int FLOOR_DIVISOR = 100;
    [Header("Black Curtain")]
    [SerializeField] private Image blackCurtain;

    [Header("BackGround Objects")]
    [SerializeField] private GameObject corridorBackGround;
    [SerializeField] private GameObject mainRoomBackGround;
    [SerializeField] private GameObject bedRoomBackGround;
    [SerializeField] private GameObject bathRoomBackGround;

    [Header("BackGround Tilemaps")]
    [SerializeField] private Tilemap mainRoomTilemap;
    [SerializeField] private Tilemap bedRoomTilemap;
    [SerializeField] private Tilemap bathRoomTilemap;

    [Header("Room Container")]
    [SerializeField] private Transform roomContainer;

    [Header("Room Count Setting")]
    [SerializeField] private int maxFloorCount;
    [SerializeField] private int maxRoomCountPerFloor;

    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
    private Dictionary<int, int> roomsKeyValuePairs = new Dictionary<int, int>();



    protected void Awake()
    {
        blackCurtain = GameObject.Find("BlackCurtain").GetComponent<Image>();
        SetRoomsKeyValue();
        DataManager.Instance.OnDataSaveEvent += SendMapData;
    }
    private void SetRoomsKeyValue()
    {
        int roomCount = roomContainer.childCount;
        for(int i = 0; i < roomCount; i++)
        {
            if(int.TryParse(roomContainer.GetChild(i).name, out int roomNumber))
            {
                roomsKeyValuePairs.Add(roomNumber, i);
            }
        }
    }

    private void SendMapData()
    {
        DataManager.Instance.GetCurRoomData(curRoomNumber, curRoomType);
    }

    private Room FindRoom(int numberInList)
    {
        if (!roomsKeyValuePairs.ContainsKey(numberInList)) return null;
        return roomContainer.GetChild(roomsKeyValuePairs[numberInList]).GetComponent<Room>().SetRoom();
    }
    private void UpdateRooms()
    {
        for(int floor = 1; floor <= maxFloorCount; floor++)
        {
            for(int room = 0; room <= maxRoomCountPerFloor; room++)
            {
                RemoveRoom(floor, room);
                AddRoom(floor, room);
            }
        }
    }
    private bool CheckIfRoomIsInRange(bool isMaxRange, int numberInList)
    {
        bool IsSameCorridor(int num)
        {
            return num == curRoomNumber - curRoomNumber % FLOOR_DIVISOR;
        }

        int GetFloorNumber(int roomNumber)
        {
            return roomNumber / FLOOR_DIVISOR;
        }

        bool IsWithinMaxRangeForCorridor(int num, int floorDiff)
        {
            if(num % FLOOR_DIVISOR != 0)
            {
                return floorDiff <= 1;
            }
            else
            {
                return floorDiff <= 2;
            }
        }

        bool IsWithinNormalRange(int num, int floorDiff)
        {
            if(num % FLOOR_DIVISOR != 0)
            {
                return floorDiff == 0;
            }
            else
            {
                return floorDiff <= 1;
            }
        }

        if (numberInList == curRoomNumber || IsSameCorridor(numberInList)) return true;
        
        int floorDifference = Math.Abs(GetFloorNumber(numberInList)-GetFloorNumber(curRoomNumber));
        
        if(isMaxRange && curRoomType == RoomType.Corridor)
        {
            return IsWithinMaxRangeForCorridor(numberInList, floorDifference);
        }
        else if (isMaxRange || curRoomType == RoomType.Corridor)
        {
            return IsWithinNormalRange(numberInList,floorDifference);
        }
        return false;
    }
    private void AddRoom(int floor, int room)
    {
        int numberInList = floor *100 + room;
        if (!roomsKeyValuePairs.ContainsKey(numberInList)) return;
        if (rooms.ContainsKey(numberInList) || !CheckIfRoomIsInRange(false, numberInList)) return;
        rooms.Add(numberInList, FindRoom(numberInList));
        
    }

    private void RemoveRoom(int floor, int room)
    {
        int numberInList = floor * 100 + room;
        if (!rooms.ContainsKey(numberInList) || CheckIfRoomIsInRange(true, numberInList)) return;
        rooms.Remove(numberInList);
        
    }
    public void MoveTo(RoomType roomType, int roomNumber)
    {
        if (!roomsKeyValuePairs.ContainsKey(roomNumber)) return;
        IsOnMoving = true;
        if (roomType == RoomType.Corridor || curRoomType == RoomType.Corridor)
        {
            StartCoroutine(GoToHallOrRoom(roomNumber));
        }
        else
        {
            StartCoroutine(GoBetweenMainandInRoom(roomType));
        }
    }
    private void SetRoom(int roomNumber)
    {
        void SetBackGroundAndObjects(bool isMovingToCorridor)
        {
            if (curRoom != null) curRoom.gameObject.SetActive(false);
            if (curRoomType == RoomType.Corridor && isMovingToCorridor) return;
            corridorBackGround.SetActive(roomNumber % FLOOR_DIVISOR == 0);
            mainRoomBackGround.SetActive(roomNumber % FLOOR_DIVISOR != 0);
        }

        float CalculateXpos()
        {
            if (roomNumber % FLOOR_DIVISOR != 0) return -8;
            switch(curRoomNumber%FLOOR_DIVISOR)
            {
                case 1:
                    return -15;
                case 2:
                    return -10;
                case 3:
                    return -5;
                case 4:
                    return 10;
                case 5:
                    return 15;
                default:
                    return 2.5f;
            }
        }
        SetBackGroundAndObjects(roomNumber%FLOOR_DIVISOR == 0);
        if(controller == null) controller = FindFirstObjectByType<PlayerController>();
        if(curRoomType == RoomType.Corridor || curRoomType == RoomType.MainRoom)
        controller.transform.position = new Vector2(CalculateXpos(), 0);
        curRoomNumber = roomNumber;
        curRoomType = roomNumber % 100 == 0 ? RoomType.Corridor : RoomType.MainRoom;
        curRoom = rooms[curRoomNumber];
        curRoom.gameObject.SetActive(true);
        UpdateRooms();
    }

    private IEnumerator GoToHallOrRoom(int roomNumber)
    {
        Image curtain = blackCurtain.GetComponent<Image>();
        float alpha = 0;
        while (alpha<1)
        {
            alpha += changeSpeed * Time.deltaTime;
            curtain.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SetRoom(roomNumber);
        yield return new WaitForSeconds(0.5f);
        while (alpha > 0)
        {
            alpha -= changeSpeed * Time.deltaTime;
            curtain.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        IsOnMoving = false;

    }
    private IEnumerator GoBetweenMainandInRoom(RoomType roomType)
    {
        GetVisualObjects(curRoomType, out SpriteRenderer[] spritesToHide, out Tilemap tilemapToHide, out GameObject backGroundToHide, out GameObject roomToHide, out GameObject doorToHide);
        GetVisualObjects(roomType, out SpriteRenderer[] spritesToShow, out Tilemap tilemapToShow, out GameObject backGroundToShow, out GameObject roomToShow, out GameObject doorToShow);
        if (doorToShow != null) doorToShow.SetActive(true);
        roomToShow.SetActive(true);
        backGroundToShow.SetActive(true);
        float showAlpha = 0;
        float hideAlpha = 1;
        while (showAlpha < 1 || hideAlpha > 0)
        {
            hideAlpha -= changeSpeed * Time.deltaTime;
            showAlpha += changeSpeed * Time.deltaTime;
            ChangeAlpha(spritesToHide, tilemapToHide, hideAlpha);
            ChangeAlpha(spritesToShow,tilemapToShow,showAlpha);
            yield return null;
        }
        if(doorToHide != null) doorToHide.SetActive(false);
        roomToHide.SetActive(false);
        backGroundToHide.SetActive(false);
        curRoomType = roomType;
        IsOnMoving = false;
    }
    private void ChangeAlpha(SpriteRenderer[] spriteRenderers, Tilemap tilemap, float alpha)
    {
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if(spriteRenderer == null) continue;
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, alpha);
        }
        Color color = tilemap.color;
        tilemap.color = new Color(color.r,color.g,color.b, alpha);
        
    }
    private void GetVisualObjects(RoomType roomType, out SpriteRenderer[] spriteRenderers, out Tilemap tilemap,out GameObject backGround, out GameObject room, out GameObject door)
    {
        switch(roomType)
        {
            case RoomType.MainRoom:
                spriteRenderers = curRoom.mainSprites;
                tilemap = mainRoomTilemap;
                backGround = mainRoomBackGround;
                room = curRoom.mainRoom.gameObject;
                door = null;
                return;
            case RoomType.BedRoom:
                spriteRenderers = curRoom.bedSprites;
                tilemap = bedRoomTilemap;
                backGround = bedRoomBackGround;
                room = curRoom.bedRoom.gameObject;
                door = curRoom.doorBedtoMain.gameObject;
                return;
            case RoomType.BathRoom:
                spriteRenderers = curRoom.bathSprites;
                tilemap = bathRoomTilemap;
                backGround = bathRoomBackGround;
                room = curRoom.bathRoom.gameObject;
                door = curRoom.doorBathtoMain.gameObject;
                return;
        }
        spriteRenderers = null;
        tilemap = null;
        backGround=null;
        room = null;
        door = null;
        return;
    }

    public IEnumerator InitRoom(int roomNumber, RoomType roomType)
    {
        controller = FindFirstObjectByType<PlayerController>();
        blackCurtain.gameObject.SetActive(true);
        blackCurtain.color = Color.black;
        controller = FindFirstObjectByType<PlayerController>();
        curRoomNumber = roomNumber;
        curRoomType = roomType;
        UpdateRooms();
        curRoom = rooms[roomNumber];
        InitAllVisualObjects();
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= changeSpeed * Time.deltaTime;
            blackCurtain.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private void InitAllVisualObjects()
    {
        corridorBackGround.SetActive(false);
        mainRoomBackGround.SetActive(false);
        bedRoomBackGround.SetActive(false);
        bathRoomBackGround.SetActive(false);
        int roomCount = roomContainer.childCount;
        for(int i = 0; i<roomCount;i++)
        {
            GameObject room = roomContainer.GetChild(i).gameObject;
            if(!room.activeSelf||curRoomNumber == int.Parse(room.name)) continue;
            room.SetActive(false);
        }
        curRoom.gameObject.SetActive(true);
        if(curRoomType == RoomType.Corridor)
        {
            corridorBackGround.SetActive(true);
            return;
        }
        GetVisualObjects(curRoomType, out SpriteRenderer[] spritesToShow, out Tilemap tileToShow, out GameObject backgroundToShow, out GameObject roomToShow, out GameObject doorToShow);
        ChangeAlpha(spritesToShow, tileToShow, 1f);
        backgroundToShow.SetActive(true);
        roomToShow.SetActive(true);
        if(doorToShow != null) doorToShow.SetActive(true);
        switch(curRoomType)
        {
            case RoomType.MainRoom:
            curRoom.doorBathtoMain.SetActive(false);
            curRoom.doorBedtoMain.SetActive(false);
            curRoom.bedRoom.gameObject.SetActive(false);
            curRoom.bathRoom.gameObject.SetActive(false);
            break;
            case RoomType.BedRoom:
            curRoom.doorBathtoMain.SetActive(false);
            curRoom.mainRoom.gameObject.SetActive(false);
            curRoom.bathRoom.gameObject.SetActive(false);
            break;
            case RoomType.BathRoom:
            curRoom.doorBedtoMain.SetActive(false);
            curRoom.mainRoom.gameObject.SetActive(false);
            curRoom.bedRoom.gameObject.SetActive(false);
            break;
        }

    }
}
