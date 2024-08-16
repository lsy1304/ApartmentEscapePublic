using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("Tutorials")]
    [SerializeField] private Image moveAndInter;
    [SerializeField] private Image dialogue;
    [SerializeField] private Image use;
    [SerializeField] private Image combine;
    [Header("TutorialTxtLink")]
    [SerializeField] TextMeshProUGUI moveTxt;
    [SerializeField] TextMeshProUGUI interactionTxt;
    [SerializeField] TextMeshProUGUI dialogueTxt;
    [SerializeField] TextMeshProUGUI useTxt;
    [SerializeField] TextMeshProUGUI combineTxt;

    [Header("EngWord")]
    [SerializeField] string moveWordEng;
    [SerializeField] string interactionWordEng;
    [SerializeField] string dialogueWordEng;
    [SerializeField] string useWordEng;
    [SerializeField] string combineWordEng;

    [Header("KorWord")]
    [SerializeField] string moveWordKor;
    [SerializeField] string interactionWordKor;
    [SerializeField] string dialogueWordKor;
    [SerializeField] string useWordKor;
    [SerializeField] string combineWordKor;
    private void Awake()
    {
        SetLocale(LocaleManager.Instance.CurrentLocaleIdx);
        ResetImageActives();
    }
    private void SetLocale(int localeIdx)
    {
        if(localeIdx == 0)
        {
            moveTxt.text = moveWordEng;
            interactionTxt.text = interactionWordEng;
            dialogueTxt.text = dialogueWordEng;
            useTxt.text = useWordEng;
            combineTxt.text = combineWordEng;
        }
        else
        {
            moveTxt.text = moveWordKor;
            interactionTxt.text = interactionWordKor;
            dialogueTxt.text = dialogueWordKor;
            useTxt.text = useWordKor;
            combineTxt.text = combineWordKor;
        }
    }
    private void ResetImageActives()
    {
        gameObject.SetActive(SceneLoadManager.Instance.IsNewGame);
        moveAndInter.gameObject.SetActive(true);
        dialogue.gameObject.SetActive(false);
        use.gameObject.SetActive(false);
        combine.gameObject.SetActive(false);
        
    }
    public void OnButton()
    {
        if (moveAndInter.gameObject.activeSelf)
        {
            moveAndInter.gameObject.SetActive(false);
            dialogue.gameObject.SetActive(true);
        }
        else if (dialogue.gameObject.activeSelf)
        {
            dialogue.gameObject.SetActive(false);
            use.gameObject.SetActive(true);
        }
        else if (use.gameObject.activeSelf)
        {
            use.gameObject.SetActive(false);
            combine.gameObject.SetActive(true);
        }
        else
        {
            combine.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
