using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(ItemSO item = null);
}

[Serializable]
public abstract class Puzzle : MonoBehaviour
{
    [SerializeField] private PuzzleSO PuzzleData;
    [SerializeField] private Transform itemSpawnPos;
    public PuzzleSO puzzleData
    {
        get { return PuzzleData; }
    }
    public bool IsUnlock = false;
    protected int[] CurrentInput;
    protected event Action OnAnswerCheckEvent;
    public event Action OnClearEvent;
    [SerializeField] private List<Gimmick> UnlockGimmicks;
    [SerializeField] private InteractionEvent interactionEvent;

    void Start()
    {
        interactionEvent = GetComponent<InteractionEvent>();
    }

    protected void UnlockGimmick()
    {
        IsUnlock = true;
        if(interactionEvent != null)
            DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.FirstInteract));
        if (UnlockGimmicks != null)
        {
            foreach (Gimmick gimmick in UnlockGimmicks)
            {
                gimmick.Unlock();
            }
        }
        if (puzzleData.IsItemSpawnable)
        {
            GameObject go = Instantiate(puzzleData.SpawnItem, itemSpawnPos, true);
            go.transform.localPosition = Vector2.zero;
        }
        CallClearEvent();
        OnClearEvent = null;
    }    

    protected void CallAnswerCheckEvent()
    {
        OnAnswerCheckEvent?.Invoke();
    }

    private void CallClearEvent()
    {
        OnClearEvent?.Invoke(); 
    }

    public virtual void ResetValue()
    {
        
    }
}
