using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveBtn : MonoBehaviour
{
    Button btn;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => DataManager.Instance.SaveData());
    }
}
