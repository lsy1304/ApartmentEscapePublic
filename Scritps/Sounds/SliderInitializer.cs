using UnityEngine;
using UnityEngine.UI;

public class SliderInitializer : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider; 
    [SerializeField] private Slider bgVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        SoundManager.Instance.AssignSliders(masterVolumeSlider, bgVolumeSlider, sfxVolumeSlider);
    }
}
