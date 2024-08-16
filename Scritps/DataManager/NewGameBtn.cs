using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameBtn : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] GameObject SaveSlots;
    private void Awake()
    {
        btn.onClick.AddListener(() => 
        { 
            SceneLoadManager.Instance.StartNewGame();
            SaveSlots.SetActive(true);
        });
    }
}
