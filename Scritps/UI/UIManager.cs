using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Main Panel")]
    [SerializeField] private GameObject mainPanel;

    [Header("UIPages")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject mapUI;

    [Header("UI Buttons")]
    [SerializeField] private Button mainBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button mapBtn;

    [Header("Main Panel Buttons")]
    [SerializeField] private Button returnHomeBtn;
    [SerializeField] private Button shotDownBtn;

    [Header("Pause Panel")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject settingPanel;

    public bool IsUIOpen { get; private set; } = false;

    private PlayerController controller;

    protected void Awake()
    {

        controller = FindObjectOfType<PlayerController>();
        controller.OnPauseEvent += ShowPauseUI;
        DisablePanel();

        mainBtn.onClick.AddListener(OpenMainUI);
        inventoryBtn.onClick.AddListener(OpenInventoryUI);
        mapBtn.onClick.AddListener(OpenMapUI);

        returnHomeBtn.onClick.AddListener(OpenMainUI);
        shotDownBtn.onClick.AddListener(DisablePanel);

        returnHomeBtn.gameObject.SetActive(false);
        shotDownBtn.gameObject.SetActive(false);

        if (pauseUI == null)
        {
            pauseUI = GameObject.Find("PauseUI");
        }
        pauseUI.SetActive(false);
    }

    private void ShowPauseUI()
    {
        if(!PlayerController.IsInteractInterrupted)
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
        if(settingPanel.activeInHierarchy) settingPanel.SetActive(false);
    }

    private void ClosePauseUI()
    {
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
        }
        else
        {
            pauseUI.SetActive(false);
        }
    }

    private void OpenMainUI()
    {
        EnablePanel(mainUI);
    }

    private void OpenInventoryUI()
    {
        EnablePanel(inventoryUI);
    }

    private void OpenMapUI()
    {
        EnablePanel(mapUI);
    }

    private void EnablePanel(GameObject uiElement)
    {
        DisablePanel();
        mainPanel.SetActive(true);
        uiElement.SetActive(true);

        IsUIOpen = true;
        returnHomeBtn.gameObject.SetActive(uiElement != mainUI);
        returnHomeBtn.transform.SetAsLastSibling();
        shotDownBtn.gameObject.SetActive(uiElement != false);
        shotDownBtn.transform.SetAsLastSibling();

        mainBtn.gameObject.SetActive(false);
    }

    private void DisablePanel()
    {
        mainPanel.SetActive(false);
        mainUI.SetActive(false);
        inventoryUI.SetActive(false);
        mapUI.SetActive(false);

        IsUIOpen = false;
        controller.CallMoveEvent(Vector2.zero);

        mainBtn.gameObject.SetActive(true);
    }

    public void DisableMainBtn()
    {
        mainBtn.interactable = false;
    }

    public void EnableMainBtn()
    {
        mainBtn.interactable = true;
    }
}