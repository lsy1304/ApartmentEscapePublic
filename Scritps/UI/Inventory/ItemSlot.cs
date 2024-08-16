using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemSlot : MonoBehaviour
{
    public ItemSO item;
    public UIInventory inventory;
    public Image icon;

    public Button button;
    public Outline outline;

    public int index;
    public bool equipped;
    public bool HasItem => icon.sprite != null;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.Sprite;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}