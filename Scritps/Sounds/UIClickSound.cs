using UnityEngine;

public class UIClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip click;

    public void PlayClickClip()
    {
        SoundManager.Instance.PlayPitchClip(click);
    }
}
