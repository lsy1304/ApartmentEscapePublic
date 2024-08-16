using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName= "DefaultItmeSO", menuName = "ItemSO/Default",order = 0)]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public int ItemIdx;
    public Sprite Sprite;
    public bool IsCombinable;
    public bool IsDestroyOnCombine;
    public int OutcomeItemIdx;
    public int MaxUsageCount;
    public bool IsObtained;
}

