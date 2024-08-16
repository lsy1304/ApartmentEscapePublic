using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Ending{
    NormalEnd = 1,
    BadEnd1 = 101,
    BadEnd2


}
public class EndingSceneManager : MonoBehaviour
{
    private EndDialogue curDialogue;
    private string[] curComments;
    [SerializeField] EndDialogue EngEnding;
    [SerializeField] EndDialogue KorEnding;
    [SerializeField] TextMeshProUGUI Comment;
    [SerializeField] float contextTime;
    [SerializeField] CanvasGroup DialogueCanvas;
    [SerializeField] CanvasGroup BadEndCanvas;
    [SerializeField] CanvasGroup NormalEndCanvas;
    private Ending ending;
    private Coroutine curCoroutine;
    private string curContext;
    private int curContextNumber;
    private bool isCommentsEnd = false;
    private bool isLoadingEnd = false;

    private void Awake()
    {
        curContextNumber = 0;
        curContext = null;
        if(LocaleManager.Instance.CurrentLocaleIdx==0){
            curDialogue = EngEnding;
        }else{
            curDialogue = KorEnding;
        }
        
    }
    public void GetEndingNumber(Ending ending)
    {
        this.ending = ending;
        switch(ending)
        {
            case Ending.NormalEnd:
            curComments = curDialogue.NormalEnd;
            break;
            case Ending.BadEnd1:
            curComments = curDialogue.BadEnd101;
            break;
            case Ending.BadEnd2:
            curComments = curDialogue.BadEnd102;
            break;
        }

    }

    public void DialogueInvoke (InputAction.CallbackContext context)
    {
        if(context.performed&&!isLoadingEnd)
        {
            ActivateDialogue();
        }

    }
    private void ActivateDialogue()
    {
        if (isCommentsEnd)
        {
            SceneLoadManager.Instance.LoadMainMenu(true);
            return;
        }
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            Comment.text = curContext;
            curCoroutine = null;
            return;
        }
        if(curContextNumber>=curComments.Length)
        {
            LoadEnding();
            return;
        }
        curContext = curComments[curContextNumber];
        curCoroutine = StartCoroutine(PrintDialogue());
        curContextNumber++;

    }
    private void LoadEnding()
    {
        isLoadingEnd = true;
        CanvasGroup canvasGroup = ending == Ending.NormalEnd ? NormalEndCanvas : BadEndCanvas;
        StartCoroutine(ChangeAlpha(canvasGroup));

    }

    private IEnumerator PrintDialogue()
    {
        Comment.text = null;
        for(int i = 0 ; i<curContext.Length; i++)
        {
            Comment.text += curContext[i];
            yield return new WaitForSeconds(contextTime);
        }
    }
    private IEnumerator ChangeAlpha(CanvasGroup canvasGroup)
    {
        DialogueCanvas.alpha = 1;
        while(DialogueCanvas.alpha > 0f)
        {
            DialogueCanvas.alpha -= Time.deltaTime;
            yield return null;
        }
        DialogueCanvas.gameObject.SetActive(false);
        canvasGroup.gameObject.SetActive(true);
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
        isCommentsEnd = true;
        isLoadingEnd = false;
    }
}
