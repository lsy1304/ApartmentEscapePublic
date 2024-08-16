using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    [HideInInspector] public Dictionary<ItemSO, int> itemIndex = new Dictionary<ItemSO, int>();

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public QuickSlot quickSlot;
    public CombineTable combineTable;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public Image selectedItemIcon;
    public GameObject useBtn;
    public GameObject combineBtn;

    private ItemSlot selectedItemSlot;
    private ItemSlot combineTargetSlot;
    private bool isCombineMode = false;

    public ItemSO selectedItem { get; private set; }
    int selectedIndex = 0;

    public LocalizeItemName localizeName;
    public LocalizeItemDesc localizeDesc;

    private void Awake()
    {
        InitializeSlots();
        UpdateUI();
        ClearSelectedItemInfo();
        useBtn.GetComponent<Button>().onClick.AddListener(UseSelectedItem);
        combineBtn.GetComponent<Button>().onClick.AddListener(ToggleCombineMode);
    }

    private void InitializeSlots()
    {
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
    }

    public void ClearSelectedItemInfo()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemIcon.sprite = null;
        selectedItemIcon.enabled = false;
        selectedItem = null;
    }

    public void AddItem(ItemSO itemData, int slotIndex = -1)
    {
        if (slots == null || slots.Length == 0)
        {
            InitializeSlots();
        }

        if(slotIndex == -1) slotIndex = FindEmptySlotIndex();

        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].item = itemData;
            slots[slotIndex].Set();
            itemIndex[itemData] = slotIndex;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear(); 
            }
        }
        DataManager.Instance.GetCurInventoryData(slots);
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null) return slots[i];
        }
        return null;
    }

    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return i;
        }
        return -1;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        if (isCombineMode)
        {
            if (selectedItemSlot == null)
            {
                selectedItemSlot = slots[index];
            }
            else
            {
                combineTargetSlot = slots[index];
                CombineSelectedItem();
            }
        }
        else
        {
            selectedItem = slots[index].item;
            selectedIndex = index;
            selectedItemSlot = slots[index];

            localizeName.LocalizeTextString("Item Table", selectedItem);
            localizeDesc.LocalizeTextString("Item Table", selectedItem);
            selectedItemIcon.sprite = selectedItem.Sprite;
            selectedItemIcon.enabled = true;
        }
    }

    private void UseSelectedItem()
    {
        if (selectedItemSlot != null && quickSlot != null)
        {
            quickSlot.AddItemToQuickSlot(selectedItemSlot);
        }
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            itemIndex.Remove(slots[index].item);
            slots[index].Clear();
            UpdateUI();
        }
    }

    public void RemoveDescription()
    {
        selectedItemName.text = null;
        selectedItemDescription.text = null;
        selectedItemIcon.sprite = null;
        selectedItemIcon.enabled = false;
    }

    private void ToggleCombineMode()
    {
        isCombineMode = !isCombineMode;
        if (!isCombineMode)
        {
            selectedItemSlot = null;
            combineTargetSlot = null;
            ClearSelectedItemInfo();
        }
    }

    private void CombineSelectedItem()
    {
        if (selectedItemSlot != null && combineTargetSlot != null)
        {
            ItemSO resultItem = combineTable.GetResult(selectedItemSlot.item.ItemIdx, combineTargetSlot.item.ItemIdx);
            if (resultItem != null)
            {
                int targetIndex = selectedItemSlot.index;

                selectedItemSlot.Clear();
                combineTargetSlot.Clear();
                AddItem(resultItem);
            }
            resultItem.IsObtained = true;
        }
        isCombineMode = false;
        selectedItemSlot = null;
        combineTargetSlot = null;
        ClearSelectedItemInfo();
        UpdateUI();
    }

    public void ClearAllSlot()
    {
        foreach(ItemSlot slot in slots)
        {
            slot.Clear();
        }
    }
}