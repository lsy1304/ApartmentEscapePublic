using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotBtn : MonoBehaviour
{
    private Button Btn;
    bool IsContainSave;
    string tempTitle;
    string filePath;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button DeleteBtn;
    [SerializeField] private Button DeleteConfirmBtn;
    [SerializeField] private GameObject save;
    [SerializeField] private GameObject closeBtn;

    private void Awake()
    {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(() => SceneLoadManager.Instance.StartGame(Convert.ToInt32(gameObject.name)));
        IsContainSave = File.Exists(filePath = Path.Combine(Application.persistentDataPath, $"SaveData{Btn.name}.json"));
        if (IsContainSave)
            tempTitle = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, $"SaveData{Btn.name}.json"))).curRoomNumber.ToString();
        if (DeleteBtn != null)
        {
            DeleteBtn.onClick.AddListener(() => ShowDeleteUI());
            DeleteBtn.interactable = IsContainSave;
        }
        
    }

    private void ShowDeleteUI()
    {
        DeleteConfirmBtn.gameObject.SetActive(true);
        save.SetActive(false);
        if (DeleteConfirmBtn != null) DeleteConfirmBtn.onClick.AddListener(() => DeleteSave());
    }

    private void OnEnable()
    {
        if (!SceneLoadManager.Instance.IsNewGame)
        {
            if(IsContainSave)
            {
                Btn.interactable = true;
                text.text = $"{tempTitle}";
            }
            else
            {
                Btn.interactable = false;
                text.text = "None";
            }
        }
        else
        {
            Btn.interactable = true;
            text.text = $"Save {name}";
        }
    }

    public void DeleteSave()
    {
        File.Delete(filePath);
        Btn.interactable = false;
        text.text = "None";
        IsContainSave = false;
        DeleteBtn.interactable = false;
        DeleteBtn.gameObject.SetActive(true);
        DeleteConfirmBtn.gameObject.SetActive(false);
        save.SetActive(true) ;
        closeBtn.gameObject.SetActive(false) ;
    }
}
