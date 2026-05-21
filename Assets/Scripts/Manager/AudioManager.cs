
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour{

    public static AudioManager Instance;

    [SerializeField] private AudioSource MainMusicAudioSource;
    [SerializeField] private AudioSource FootstepAudioSource;
    [SerializeField] private AudioSource DialogAudioSource;
    [SerializeField] private AudioClip[] FootstepAudios;
    
    [Header("Tip Sfx")]
    [SerializeField] private AudioClip ErrorSfx;

    private Coroutine CurrentDialogCoroutine;
    public bool DialogIsFinished { get; private set; } = false;
    private float MainMusicVolumeBeforeDialog;
    private bool DialogIsDecreaseMainMusicVolume = false;

    public float SettingMainMusicVolume {get; private set;} = 1.0f;
    public float SettingSfxVolume {get; private set;} = 1.0f;
    public float SettingDialogVolume {get; private set;} = 1.0f;

    private float MainMusicVolume {
        get => this.MainMusicAudioSource.volume;
        set => this.MainMusicAudioSource.volume = Mathf.Clamp(this.SettingMainMusicVolume * value, 0.0f, 1.0f);
    }

    private float SfxVolume {
        get => this.FootstepAudioSource.volume;
        set => this.FootstepAudioSource.volume = Mathf.Clamp(this.SettingSfxVolume * value, 0.0f, 1.0f);
    }

    private float DialogVolume {
        get => this.DialogAudioSource.volume;
        set => this.DialogAudioSource.volume = Mathf.Clamp(this.SettingDialogVolume * value, 0.0f, 1.0f);
    }

    private void Awake(){
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.HasKey(PlayerPrefsKeyName.MainMusicVolume)) {
            this.SettingMainMusicVolume = PlayerPrefs.GetFloat(PlayerPrefsKeyName.MainMusicVolume);
        }
        if (PlayerPrefs.HasKey(PlayerPrefsKeyName.SfxVolume)) {
            this.SettingSfxVolume = PlayerPrefs.GetFloat(PlayerPrefsKeyName.SfxVolume);
        }
        if (PlayerPrefs.HasKey(PlayerPrefsKeyName.DialogVolume)) {
            this.SettingDialogVolume = PlayerPrefs.GetFloat(PlayerPrefsKeyName.DialogVolume);
        }
        this.MainMusicVolume = 1.0f;
        this.SfxVolume = 1.0f;
        this.DialogVolume = 1.0f;
    }

    public void SetMainMusic(AudioClip newClip, float volume = 1.0f){
        this.MainMusicAudioSource.mute = true;
        this.MainMusicAudioSource.clip = newClip;
        this.MainMusicVolume = volume;
        this.MainMusicAudioSource.mute = false;
        this.MainMusicAudioSource.Play();
    }

    public void PlayErrorSfx() {
        if(this.ErrorSfx) this.PlaySfx(this.ErrorSfx);
    }

    public void FadeMainMusic(AudioClip newClip, float duration = 1.0f, float newVolume = 1.0f) {
        StartCoroutine(FadeMainMusicCoroutine(newClip, duration, newVolume));
    }

    private IEnumerator FadeMainMusicCoroutine(AudioClip newClip, float duration, float volume) {
        yield return FadeCoroutine(0.0f, duration / 2.0f);
        SetMainMusic(newClip, 0.0f);
        yield return FadeCoroutine(volume, duration / 2.0f);
    }
    
    private IEnumerator FadeCoroutine(float newVolume, float duration = 0.5f){
        float startVolume = this.MainMusicVolume;
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.MainMusicVolume = Mathf.Lerp(startVolume, newVolume, t / duration);
            yield return null;
        }
        this.MainMusicVolume = newVolume;
    }

    public AudioClip GetCurrentMainMusic() {
        return this.MainMusicAudioSource.clip;
    }

    public void StopMainMusic() {
        this.MainMusicAudioSource.Stop();
    }

    public void SetDialog(AudioClip newClip, bool decreaseMainMusic, float volume = 1.0f) {
        this.DialogIsFinished = false;
        this.DialogAudioSource.mute = true;
        this.DialogVolume = volume;
        this.DialogAudioSource.clip = newClip;
        this.DialogAudioSource.mute = false;
        this.DialogAudioSource.Play();

        this.DialogIsDecreaseMainMusicVolume = decreaseMainMusic;
        if (decreaseMainMusic) {
            this.MainMusicVolumeBeforeDialog = this.MainMusicVolume;
            this.MainMusicVolume = 0.3f;
        }
        
        if (this.CurrentDialogCoroutine != null) {
            StopCoroutine(this.CurrentDialogCoroutine);
        }
        this.CurrentDialogCoroutine = StartCoroutine(DialogPlayCoroutine(newClip.length));
    }

    public void SetDialogPlayPos(float seconds) {
        this.DialogAudioSource.time = seconds;
        if (this.CurrentDialogCoroutine != null) {
            StopCoroutine(this.CurrentDialogCoroutine);
        }

        float remain = this.DialogAudioSource.clip.length - seconds;
        this.CurrentDialogCoroutine = StartCoroutine(DialogPlayCoroutine(remain));
    }

    private IEnumerator DialogPlayCoroutine(float length) {
        yield return new WaitForSecondsRealtime(length);
        this.DialogIsFinished = true;
        if(this.DialogIsDecreaseMainMusicVolume) this.MainMusicAudioSource.volume = this.MainMusicVolumeBeforeDialog;
    }

    public void StopDialog() {
        if (this.CurrentDialogCoroutine != null) {
            StopCoroutine(this.CurrentDialogCoroutine);
            this.CurrentDialogCoroutine = null;
        }
        this.DialogIsFinished = true;
        if(this.DialogIsDecreaseMainMusicVolume) this.MainMusicAudioSource.volume = this.MainMusicVolumeBeforeDialog;
        this.DialogAudioSource.Stop();
    }

    public void PlaySfx(AudioClip clip){
        this.FootstepAudioSource.PlayOneShot(clip, 0.5f);
    }

    public void PlayFootstep(){
        if (this.FootstepAudios.Length == 0) return;
        int index = Random.Range(0, this.FootstepAudios.Length);
        this.FootstepAudioSource.clip = this.FootstepAudios[index];
        this.FootstepAudioSource.Play();
    }

    public void StopFootstep(){
        this.FootstepAudioSource.Stop();
        this.FootstepAudioSource.clip = null;
    }

    public void SetSettingMainMusicVolume(float volume) {
        this.SettingMainMusicVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
        this.MainMusicVolume = 1.0f;
        PlayerPrefs.SetFloat(PlayerPrefsKeyName.MainMusicVolume, this.SettingMainMusicVolume);
    }

    public void SetSettingSfxVolume(float volume) {
        this.SettingSfxVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
        this.SfxVolume = 1.0f;
        PlayerPrefs.SetFloat(PlayerPrefsKeyName.SfxVolume, this.SettingSfxVolume);
    }

    public void SetSettingDialogVolume(float volume) {
        this.SettingDialogVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
        this.DialogVolume = 1.0f;
        PlayerPrefs.SetFloat(PlayerPrefsKeyName.DialogVolume, this.SettingDialogVolume);
    }

}

