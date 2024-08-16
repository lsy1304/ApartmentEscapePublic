using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;
    public ItemSO ItemData { get { return itemData; } }
    private SpriteRenderer Image;
    private InteractionEvent interactionEvent;
    private void Awake()
    {
        if (itemData.IsObtained)
        {
            if(gameObject != null)
                Destroy(gameObject);
            return;
        }
        interactionEvent = GetComponent<InteractionEvent>();
        Image = GetComponent<SpriteRenderer>();
        Image.sprite = ItemData.Sprite;
    }
    public void Interact(ItemSO item = null)
    {
        if (interactionEvent != null)
        DialogueManager.Instance.UI.ShowDialogue(interactionEvent.GetDialogue(DialogueType.FirstInteract));
        item.IsObtained = true;
        Destroy(gameObject);
    }
}