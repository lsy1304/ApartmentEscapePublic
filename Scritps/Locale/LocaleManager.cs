using Michsky.UI.Dark;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class LocaleManager : SingletonDontDestroy<LocaleManager>
{
    bool isChanging;
    public int CurrentLocaleIdx;
    Locale currentLocale;
    [SerializeField] TextMeshProUGUI currentSettingText;
    [SerializeField] HorizontalSelector LanguageSelector;
    Dictionary<string, string> localeDic = new Dictionary<string, string>();


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += FindComponent;
    }

    private void FindComponent(Scene arg0, LoadSceneMode arg1)
    {
        if (GameObject.FindGameObjectWithTag("LanguageSelector") == null) { GameObject.Find("SettingPanel")?.SetActive(false); Destroy(this); return; }
        LanguageSelector = GameObject.FindGameObjectWithTag("LanguageSelector").GetComponent<HorizontalSelector>();
        currentSettingText = GameObject.FindGameObjectWithTag("MainText").GetComponent<TextMeshProUGUI>();

        if (!PlayerPrefs.HasKey("Initialized"))
        {
            PlayerPrefs.SetInt("Initialized", 1);
        }


        if (PlayerPrefs.HasKey("Language"))
        {
            CurrentLocaleIdx = PlayerPrefs.GetInt("Language");
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    CurrentLocaleIdx = 0;
                    break;
                case SystemLanguage.Korean:
                    CurrentLocaleIdx = 1;
                    break;
                default:
                    CurrentLocaleIdx = 0;
                    break;
            }
        }

        if (localeDic.Count <= 0)
        {
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
            {
                localeDic.Add(LocalizationSettings.AvailableLocales.Locales[i].LocaleName, LanguageSelector.itemList[i].itemTitle);
            }
        }

        StartCoroutine(LanguageUIInit());

        GameObject.Find("SettingPanel").SetActive(false);

        PlayerPrefs.Save();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Language", CurrentLocaleIdx);
    }

    public void ChangeLocale()
    {
        if (isChanging) return;
        StartCoroutine(ChangeRoutine(CurrentLocaleIdx = (CurrentLocaleIdx + 1) % 2 ));
    }

    IEnumerator ChangeRoutine(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[CurrentLocaleIdx];
        PlayerPrefs.SetInt("Language", CurrentLocaleIdx);
        isChanging = false;
    }

    IEnumerator LanguageUIInit()
    {
        LanguageSelector.SetupSelector();

        yield return StartCoroutine(ChangeRoutine(CurrentLocaleIdx));

        while (currentSettingText.text != localeDic[LocalizationSettings.SelectedLocale.LocaleName])
        {
            LanguageSelector.PreviousClick();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FindComponent;

        PlayerPrefs.Save();
    }
}
