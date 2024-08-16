using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;
    public bool IsFirstInteract;

    private void Awake()
    {
        IsFirstInteract = true;
    }

    public Dialogue[] GetDialogue(DialogueType dialogueType)
    {
        int type = (int)dialogueType;
        dialogue.dialogues = DialogueManager.Instance.GetDialogues(dialogue.dialogueLines[type].startLine, dialogue.dialogueLines[type].endLine);
        if (IsFirstInteract)
        {
            IsFirstInteract = false;
        }
        return dialogue.dialogues;
    }
}
