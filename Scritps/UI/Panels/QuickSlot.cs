using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public ItemSlot quickSlot;

    private void Start()
    {
        if (quickSlot == null)
        {
            quickSlot = GetComponentInChildren<ItemSlot>();
        }
        quickSlot.icon.enabled = false;
    }

    public void AddItemToQuickSlot(ItemSlot itemSlot)
    {
        if (quickSlot != null && itemSlot != null)
        {
            quickSlot.icon.enabled = true;
            quickSlot.item = itemSlot.item;
            quickSlot.Set();
        }
    }

    public void ClearQuickSlot()
    {
        if (quickSlot != null)
        {
            quickSlot.icon.enabled = false;
            quickSlot.Clear();
        }
    }
}