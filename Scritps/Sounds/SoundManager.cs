using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : SingletonDontDestroy<SoundManager>
{
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioClip startSceneBGM;
    [SerializeField] private AudioClip mainSceneBGM;
    [SerializeField] private AudioClip footprintClip;
    [SerializeField][Range(0, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0, 1f)] private float soundEffectPitchVariance;

    private float currentMasterVolume = 1f;
    private float currentBGVolume = 1f;
    private float currentSFXVolume = 1f;
    private bool isPlayingFootprint = false;

    private Slider masterVolumeSlider;
    private Slider bgVolumeSlider;
    private Slider sfxVolumeSlider;
    private PlayerController controller;
    private AudioSource currentFootprintSource;
    private Coroutine coroutine;

    public Objectpool objectpool { get; private set; }
    public AudioMixer mixer;

    protected override void Awake()
    {
        base.Awake();
        LoadVolumeSettings();
        objectpool = GetComponent<Objectpool>();
        StartCoroutine(InitialBGPlay());
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                BGMPlay(startSceneBGM, currentBGVolume);
                break;
            case 1:
                BGMPlay(mainSceneBGM, currentBGVolume);
                break;
            default:
                break;
        }

        UpdateSliders();
    }

    private IEnumerator InitialBGPlay()
    {
        yield return new WaitForSeconds(1.6f);
        BGMPlay(startSceneBGM, currentBGVolume);
    }

    private void BGMPlay(AudioClip clip, float volume)
    {
        if (BGM == null || !BGM.gameObject.activeInHierarchy || !BGM.enabled) return;
        if (BGM.isPlaying) BGM.Stop();

        BGM.clip = clip;
        BGM.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        GameObject obj = objectpool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, soundEffectVolume);
    }

    public void PlayPitchClip(AudioClip clip)
    {
        GameObject obj = objectpool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, soundEffectVolume, soundEffectPitchVariance);
    }

    private void MasterSoundVolume(float val)
    {
        currentMasterVolume = Mathf.Clamp(val, 0.001f, 1f);
        mixer.SetFloat("MasterVolume", Mathf.Log10(currentMasterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", currentMasterVolume);
    }

    private void BGSoundVolume(float val)
    {
        currentBGVolume = Mathf.Clamp(val, 0.001f, 1f);
        mixer.SetFloat("BGVolume", Mathf.Log10(currentBGVolume) * 20);
        PlayerPrefs.SetFloat("BGVolume", currentBGVolume);
    }

    private void SFXSoundVolume(float val)
    {
        currentSFXVolume = Mathf.Clamp(val, 0.001f, 1f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(currentSFXVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", currentSFXVolume);
    }

    private void UpdateSliders()
    {
        if (masterVolumeSlider != null) masterVolumeSlider.value = currentMasterVolume;
        if (bgVolumeSlider != null) bgVolumeSlider.value = currentBGVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = currentSFXVolume;
    }

    public void AssignSliders(Slider master, Slider bg, Slider sfx)
    {
        masterVolumeSlider = master;
        bgVolumeSlider = bg;
        sfxVolumeSlider = sfx;

        if(masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(MasterSoundVolume);
            masterVolumeSlider.value = currentMasterVolume;
        }
        if (bgVolumeSlider != null)
        {
            bgVolumeSlider.onValueChanged.AddListener(BGSoundVolume);
            bgVolumeSlider.value = currentBGVolume;
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SFXSoundVolume);
            sfxVolumeSlider.value = currentSFXVolume;
        }
    }

    private void LoadVolumeSettings()
    {
        currentMasterVolume = Mathf.Clamp(PlayerPrefs.GetFloat("MasterVolume", 1f), 0.001f, 1f);
        currentBGVolume = Mathf.Clamp(PlayerPrefs.GetFloat("BGVolume", 1f), 0.001f, 1f);
        currentSFXVolume = Mathf.Clamp(PlayerPrefs.GetFloat("SFXVolume", 1f), 0.001f, 1f);

        mixer.SetFloat("MasterVolume", Mathf.Log10(currentMasterVolume) * 20); 
        mixer.SetFloat("BGVolume", Mathf.Log10(currentBGVolume) * 20); 
        mixer.SetFloat("SFXVolume", Mathf.Log10(currentSFXVolume) * 20);

        BGM.outputAudioMixerGroup = mixer.FindMatchingGroups("Background")[0];
        BGM.loop = true;
    }

    private void FootprintSoundEvent(PlayerState ps)
    {
        if (ps == PlayerState.Walk && !isPlayingFootprint)
        {
            isPlayingFootprint = true;
            if (coroutine != null) 
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(PlayFootprintClip());
        }
        else if (ps != PlayerState.Walk)
        {
            isPlayingFootprint = false;
        }
    }

    private IEnumerator PlayFootprintClip()
    {
        while (controller.currentState == PlayerState.Walk)
        {
            yield return new WaitForSeconds(footprintClip.length * 0.3f);
            PlayClip(footprintClip);
            yield return new WaitForSeconds(footprintClip.length * 0.7f);
        }
        isPlayingFootprint = false;
    }

    public void RegisterPlayerController(PlayerController playerController)
    {
        if (controller != null)
        {
            controller.OnStateChangeEvent -= FootprintSoundEvent;
        }

        controller = playerController;
        controller.OnStateChangeEvent += FootprintSoundEvent;
    }
}
