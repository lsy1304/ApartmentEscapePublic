using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : SingletonDontDestroy<SceneLoadManager>
{
    [SerializeField] private Image blackCurtain;
    [SerializeField] private Image LoadingBar;

    public bool IsNewGame;
    public int SaveSlotIdx;
    public string BtnName;

    public void StartNewGame()
    {
        IsNewGame = true;
    }

    IEnumerator LoadAsyncScene(int sceneIdx)
    {
        while (blackCurtain.color.a < 1f)
        {
            Color c = blackCurtain.color;
            blackCurtain.color = new Color(c.r, c.g, c.b, c.a + Time.deltaTime);
            yield return null;
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Single);
        float timer = 0f;
        while (!async.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            async.allowSceneActivation = false;
            if (async.progress < .9f)
            {
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, async.progress, timer);
                if (LoadingBar.fillAmount >= async.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, 1f, timer);
                if(LoadingBar.fillAmount == 1f)
                {
                    async.allowSceneActivation = true;
                    LoadingBar.fillAmount = 0f;
                    break;
                }
            }
        }

        yield return null;

        switch (sceneIdx)
        {
            case 1:
                DataManager.Instance.StartGame();
                break;
            default:
                while (blackCurtain.color.a > 0f)
                {
                    Color c = blackCurtain.color;
                    blackCurtain.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime);
                    yield return null;
                }
                break;
        }
    }

    public void ContinueGame()
    {
        IsNewGame = false;
    }

    public void StartGame(int idx)
    {
        SaveSlotIdx = idx;
        StartCoroutine(LoadAsyncScene(1));
    }

    public void LoadMainMenu(bool isEnding)
    {
        if(!isEnding) DataManager.Instance.ResetItemValue();
        StartCoroutine(LoadAsyncScene(0));
    }
    public void StartLoadingEnding(int idx)
    {
        StartCoroutine(LoadEndScene(idx));
    }
    private IEnumerator LoadEndScene(int idx)
    {
        blackCurtain.color = new Color(1f, 1f, 1f, 0f);
        while(blackCurtain.color.a <1f)
        {
            Color c = blackCurtain.color;
            blackCurtain.color = new Color(c.r, c.g, c.b, c.a + Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene(2);
        yield return null;
        blackCurtain.color = new Color(0f,0f, 0f, 0f);
        FindObjectOfType<EndingSceneManager>().GetEndingNumber((Ending)idx);
    }
}
