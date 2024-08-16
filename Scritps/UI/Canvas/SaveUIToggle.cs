using System.Collections.Generic;
using UnityEngine;

public class SaveUIToggle : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotUI;

    private bool isOn = true;

    public void ToggleSaveSlotUI()
    {
        isOn = !isOn;

        if (!isOn)
        {
            saveSlotUI.SetActive(true);
        }
        else 
        {
            saveSlotUI.SetActive(false);
        }
    }

    public void CloseSaveSlotUI()
    {
        saveSlotUI.SetActive(false);
        isOn = true;
    }
}
