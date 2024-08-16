using UnityEngine;

public class UIHoverSound : MonoBehaviour
{
    [SerializeField] private AudioClip hover;

    public void PlayHoverClip()
    {
        SoundManager.Instance.PlayPitchClip(hover);
    }
}
