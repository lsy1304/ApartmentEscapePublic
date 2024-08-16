using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private int roomNumberToMove;
    [SerializeField] private RoomType roomTypeToMove;

    public void Interact(ItemSO item = null)
    {
        MapManager.Instance.MoveTo(roomTypeToMove, roomNumberToMove);
    }
}
