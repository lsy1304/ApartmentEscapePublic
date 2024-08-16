using UnityEngine;

public class BeepSound : MonoBehaviour
{
    [SerializeField] private AudioClip beepSound;

    public void PlayBeepClip()
    {
        SoundManager.Instance.PlayPitchClip(beepSound);
    }
}
