using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject contextBody;
    [SerializeField] private GameObject nameBody;
    [SerializeField] private TextMeshProUGUI contextTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField][Range(0.0f, 2.0f)] float charDelay;
    [SerializeField] private AudioClip clip;

    public bool IsDialogueOn => contextBody.activeInHierarchy;
    private Dialogue[] dialogues;
    private PlayerController controller;

    bool isDialogue = false;
    bool isNext = false;

    int lineCount = 0;
    int contextCount = 0;

    string replaceText;
    Coroutine textDisplay;


    void Start()
    {
        controller = FindAnyObjectByType<PlayerController>();
        controller.OnDialogueEvent += CallTypeWriter;
        Hide();
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        if (p_dialogues == null || p_dialogues[0].context[0] == string.Empty || p_dialogues[0].context[0] == "\r") return;
        isDialogue = true;
        dialogues = p_dialogues;
        controller.EnableInput(false);
        textDisplay = StartCoroutine(TypeWriter());
    }

    private void CallTypeWriter()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                isNext = false;
                contextTxt.text = string.Empty;

                if (++contextCount >= dialogues[lineCount].context.Length)
                {
                    contextCount = 0;
                    if (++lineCount >= dialogues.Length)
                    {
                        lineCount = 0;
                        isDialogue = false;
                        controller.EnableInput(true);
                        Hide();
                        return;
                    }
                }
                textDisplay = StartCoroutine(TypeWriter());
            }
            else
            {
                StopCoroutine(textDisplay);
                isNext = true;
                contextTxt.text = replaceText;
            }
        }
    }

    IEnumerator TypeWriter()
    {
        Show();

        replaceText = dialogues[lineCount].context[contextCount];
        replaceText = replaceText.Replace("^", ",");

        switch(dialogues[lineCount].name)
        {
            case "È¿°ú":
                nameBody.SetActive(false);
                contextTxt.fontStyle = FontStyles.Italic;
                break;
            case "":
                nameBody.SetActive(false);
                contextTxt.fontStyle = FontStyles.Normal;
                break;
            default:
                nameBody.SetActive(true);
                nameTxt.text = dialogues[lineCount].name;
                contextTxt.fontStyle = FontStyles.Normal;
                break;
        }

        for(int i = 0; i<replaceText.Length; i++)
        {
            contextTxt.text += replaceText[i];
            yield return new WaitForSeconds(charDelay);
        }
        
        isNext = true;
        yield return null;
    }

    public void Show()
    {
        contextTxt.text= string.Empty;
        nameTxt.text= string.Empty;
        contextBody.SetActive(true);
        controller.CallMoveEvent(Vector2.zero);
        nameBody.SetActive(true);
        SoundManager.Instance.PlayClip(clip);
    }

    public void Hide()
    {
        contextBody.SetActive(false);
        controller.CallMoveEvent(Vector2.zero);
        nameBody.SetActive(false);
    }
}
