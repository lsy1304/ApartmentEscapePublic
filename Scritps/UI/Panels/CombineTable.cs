using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombineTable", menuName = "CombineTable", order = 1)]
public class CombineTable : ScriptableObject
{
    [Serializable]
    public struct Combination
    {
        public int itemIndexOne;
        public int itemIndexTwo;
        
        public ItemSO result;
    }
    public List<Combination> combinations;

    private Dictionary<(int, int), ItemSO> combineDict;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        combineDict = new Dictionary<(int, int), ItemSO>();

        foreach (var combo in combinations)
        {
            combineDict[(combo.itemIndexOne, combo.itemIndexTwo)] = combo.result;
            combineDict[(combo.itemIndexTwo, combo.itemIndexOne)] = combo.result;
        }
    }
    public ItemSO GetResult(int item1, int item2)
    {
        if (combineDict.TryGetValue((item1, item2), out ItemSO result))
        {
            return result;
        }
        return null;
    }
}