using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController controller;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private ItemSO quickSlotItem => quickSlot.item;
    private DialogueUI ui;

    [SerializeField] private UIInventory inventory;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactionDistance = 0.5f;
    [SerializeField] private ItemSlot quickSlot;
    [SerializeField] private AudioClip doorClip;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        playerTransform = GetComponent<Transform>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        controller.OnInteractEvent += Interact;
        ui = DialogueManager.Instance.UI;
    }

    private void Interact()
    {
        if (PlayerController.IsInteractInterrupted) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerTransform.position, interactionDistance, interactableLayerMask);

        if (hits.Length == 0) return;

        Collider2D closestHit = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(playerTransform.localPosition, hit.transform.localPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
            }
        }

        if (closestHit != null && closestHit.gameObject.TryGetComponent(out IInteractable interactable))
        {
            switch (interactable)
            {
                case Item item:
                    controller.EnableInput(!ui.IsDialogueOn);
                    item.Interact(item.ItemData);
                    inventory.AddItem(item.ItemData);
                    break;

                case PuzzleInField puzzleInField:
                    controller.EnableInput(false);
                    if (!puzzleInField.TryInteract(quickSlotItem)) return;
                    puzzleInField.Interact();
                    if (!puzzleInField.puzzle.IsUnlock)
                    {
                        UIManager.Instance.DisableMainBtn();
                        puzzleInField.OnPuzzleClosed += PuzzleClose;
                        puzzleInField.puzzle.OnClearEvent += PuzzleClose;
                    }
                    break;

                case Door door:
                    door.Interact();
                    playerMovement.DoorStop();
                    controller.EnableInput(!ui.IsDialogueOn);
                    SoundManager.Instance.PlayClip(doorClip);
                    break;

                case Gimmick gimmick:
                    bool canInteract = gimmick.CanInteract(quickSlotItem);
                    gimmick.Interact(quickSlotItem);
                    controller.EnableInput(!ui.IsDialogueOn);
                    if (canInteract)
                    {
                        if (inventory.itemIndex.TryGetValue(quickSlotItem, out int usedItemIndex))
                        {
                            inventory.RemoveItem(usedItemIndex);
                            inventory.RemoveDescription();
                            quickSlot.Clear();
                        }
                    }
                    break;
                case NormalEnder ender:
                    ender.Interact(null);
                    break;
            }
        }
    }

    private void PuzzleClose()
    {
        controller.EnableInput(true);
        UIManager.Instance.EnableMainBtn();
    }
}
