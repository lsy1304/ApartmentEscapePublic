using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int curRoomNumber;
    public RoomType curRoomType;
    public Vector3 playerTransform;

    public List<int> InvenItemIdx;
    public List<bool> gimmicksUnlock;
    public List<bool> puzzleUnlock;
    public List<bool> isObtained;
    public SaveData InitRawData()
    {
        SaveData data = new SaveData();
        data.curRoomNumber = 705;
        data.curRoomType = RoomType.MainRoom;
        data.playerTransform = new Vector3(-8f, -0.08f, 0f);

        data.InvenItemIdx = new List<int>();
        data.gimmicksUnlock = new List<bool>();
        data.puzzleUnlock = new List<bool>();
        data.isObtained = new List<bool>();
        return data;
    }

}