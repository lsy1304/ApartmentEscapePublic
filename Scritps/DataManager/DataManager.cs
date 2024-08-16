using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private int curRoomNumber;
    private RoomType curRoomType;

    private Transform playerTransform;

    private ItemSlot[] slots;

    [SerializeField] private List<ItemSO> items;

    [SerializeField] private List<Gimmick> gimmicks;

    [SerializeField] private List<Puzzle> puzzles;

    SaveData data;
    Dictionary<int, ItemSO> itemDic = new Dictionary<int, ItemSO>();

    private SaveData saveData = new SaveData();

    public event Action OnDataSaveEvent;
    public UIInventory inventory;

    public bool IsNewGame => SceneLoadManager.Instance.IsNewGame;
    public int SaveSlotIdx => SceneLoadManager.Instance.SaveSlotIdx;

    void Awake()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemDic.Add(items[i].ItemIdx, items[i]);
        }
    }

    public void StartGame()
    {
        if (IsNewGame)
        {
            data = new SaveData().InitRawData();
            playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
            StartCoroutine(MapManager.Instance.InitRoom(data.curRoomNumber, data.curRoomType));
        }
        else
        {
            LoadData();
        }
    }

    public void GetCurRoomData(int roomNumber, RoomType roomType)
    {
        curRoomNumber = roomNumber;
        curRoomType = roomType;
    }

    public void GetCurInventoryData(ItemSlot[] slots)
    {
        this.slots = slots;
    }

    private void CallDataSaveEvent()
    {
        OnDataSaveEvent?.Invoke();
    }

    public void ResetItemValue()
    {
        foreach (ItemSO data in items)
        {
            data.IsObtained = false;
        }
    }

    public void SaveData()
    {
        CallDataSaveEvent();
        

        saveData.curRoomNumber = curRoomNumber;
        saveData.curRoomType = curRoomType;
        saveData.playerTransform = playerTransform.position;
        saveData.isObtained = new List<bool>();
        saveData.gimmicksUnlock = new List<bool>();
        saveData.puzzleUnlock = new List<bool>();
        saveData.InvenItemIdx = new List<int>();
        if (slots != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null) continue;
                saveData.InvenItemIdx.Add(slots[i].item.ItemIdx);
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            saveData.isObtained.Add(items[i].IsObtained);
        }
        for (int i = 0; i < gimmicks.Count; i++)
        {
            saveData.gimmicksUnlock.Add(gimmicks[i].IsUnlock);
        }
        for(int i = 0; i < puzzles.Count; i++)
        {
            saveData.puzzleUnlock.Add(puzzles[i].IsUnlock);
        }


        string saveDataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, $"SaveData{SaveSlotIdx}.json"), saveDataJson);
    }
     
    public void LoadData()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, $"SaveData{SaveSlotIdx}.json")))
        {
            data = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, $"SaveData{SaveSlotIdx}.json")));
            for (int i = 0; i < items.Count; i++)
            {
                items[i].IsObtained = data.isObtained[i];
            }
            inventory.ClearAllSlot();
            for(int i = 0; i< data.InvenItemIdx.Count; i++)
            { 
                inventory.AddItem(itemDic[data.InvenItemIdx[i]]);
            }
            for(int i = 0; i<data.gimmicksUnlock.Count; i++)
            {
                gimmicks[i].IsUnlock = data.gimmicksUnlock[i];
            }
            for (int i = 0; i < data.puzzleUnlock.Count; i++)
            {
                puzzles[i].IsUnlock = data.puzzleUnlock[i];
            }

            playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
            playerTransform.position = data.playerTransform;
        }
        else data = new SaveData().InitRawData();
        StartCoroutine(MapManager.Instance.InitRoom(data.curRoomNumber, data.curRoomType));
    }

    private void OnDestroy()
    {
        ResetItemValue();
    }
}
