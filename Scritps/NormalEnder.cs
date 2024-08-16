using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalEnder : MonoBehaviour, IInteractable
{
    [SerializeField] int endingNumber;
    public void Interact(ItemSO item = null)
    {
        SceneLoadManager.Instance.StartLoadingEnding(endingNumber);
    }
}
