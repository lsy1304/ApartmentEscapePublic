using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleSO", menuName = "SO / PuzzleSO")]
public class PuzzleSO : ScriptableObject
{
    [Header("Base Info")]
    public int[] Answer;

    [Header("Require Item")]
    public bool IsRequireItem;
    public ItemSO RequireItem;

    [Header("Spawn Item")]
    public bool IsItemSpawnable;
    public GameObject SpawnItem;
}
