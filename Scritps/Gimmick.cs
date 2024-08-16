using System;
using UnityEngine;

public enum DialogueType
{
    FirstInteract = 0,
    NormalInteract,
    WrongInteract,
    CorrectInteract,
    AlreadyInteract
}
[Serializable]
public class Gimmick : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] linkedObjects; 
    [SerializeField] private ItemSO RequireItem;
    private InteractionEvent interactionEvent;
    [SerializeField] bool isUnlockable;
    [SerializeField] Transform itemSpawnPos;
    [SerializeField] GameObject SpawnItem;
    [SerializeField] ItemSO BadEndItem;
    [SerializeField] int idx;
    public bool IsUnlock;

    private void Start()
    {
        if (IsUnlock)
        { 
            Unlock();
            return;
        }
        interactionEvent = GetComponent<InteractionEvent>();
        foreach (GameObject obj in linkedObjects)
        {
            obj.GetComponent<Collider2D>().enabled = false;
        }
    }
    public void Unlock()
    {
        foreach (GameObject obj in linkedObjects)
        {
            obj.GetComponent<Collider2D>().enabled = true;
        }
        if (itemSpawnPos != null)
        {
            GameObject go = Instantiate(SpawnItem, itemSpawnPos, true);
            go.transform.localPosition = Vector2.zero;
        }
        IsUnlock = true;
        gameObject.SetActive(false);
    }

    public void Interact(ItemSO item = null)
    {
        if (BadEndItem != null && BadEndItem == item)
        {
            SceneLoadManager.Instance.StartLoadingEnding(idx);
            return;
        }
        if (interactionEvent == null) return;
        if (RequireItem == item && RequireItem != null)
        {
            DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.CorrectInteract));
            if (isUnlockable) Unlock();
            return;
        }
        if (interactionEvent.IsFirstInteract)
        {
            DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.FirstInteract));
            return;
        }
        if(RequireItem == null)
        {
            DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.CorrectInteract));
            if (isUnlockable) Unlock();
            return;
        }
        if (RequireItem != null)
        {
            if (item == null)
            {
                DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.NormalInteract));
                return;
            }
            else
            {
                if (item != RequireItem)
                {
                    DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.WrongInteract));
                    return;
                }
            }
        }
    }

    public bool CanInteract(ItemSO item)
    {
        return RequireItem == item && RequireItem != null;
    }
}