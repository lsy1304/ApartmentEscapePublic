using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleInField : MonoBehaviour, IInteractable
{
    public Puzzle puzzle;
    [SerializeField] private Button closeBtn;

    public event Action OnPuzzleClosed;
    private InteractionEvent interactionEvent;
    private DialogueUI ui;

    private void Awake()
    {
        interactionEvent = GetComponent<InteractionEvent>();
    }

    private void Start()
    {
        ui = DialogueManager.Instance.UI;
        puzzle.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() =>
        {
            puzzle.ResetValue();
            puzzle.gameObject.SetActive(false);
            OnPuzzleClosed?.Invoke();
            OnPuzzleClosed = null;
        });
    }

    public void Interact(ItemSO item = null)
    {
        ui.ShowDialogue(interactionEvent.GetDialogue(DialogueType.CorrectInteract));
        puzzle.gameObject.SetActive(true);
    }

    public bool TryInteract(ItemSO item)
    {
        if (puzzle.IsUnlock)
        {
            ui.ShowDialogue(interactionEvent.GetDialogue(DialogueType.AlreadyInteract));
            return false;
        }
        if (interactionEvent.IsFirstInteract)
        {
            ui.ShowDialogue(interactionEvent.GetDialogue(DialogueType.FirstInteract));
            return false;
        }
        if (puzzle.puzzleData.RequireItem == item)
            return true;
        var temp = puzzle.puzzleData;
        if (temp.IsRequireItem)
        { 
            if (item == null)
            {
                ui.ShowDialogue(interactionEvent.GetDialogue(DialogueType.NormalInteract));
                return false;
            }
            if (temp.RequireItem != item)
            {
                ui.ShowDialogue(interactionEvent.GetDialogue(DialogueType.WrongInteract));
                return false;
            }
        }
        temp = null;
        return true;
    }
}
