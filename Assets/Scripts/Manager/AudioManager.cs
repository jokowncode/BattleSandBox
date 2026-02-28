
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

    private void Awake(){
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetMainMusic(AudioClip newClip, float volume = 1.0f){
        this.MainMusicAudioSource.mute = true;
        this.MainMusicAudioSource.clip = newClip;
        this.MainMusicAudioSource.volume = volume;
        this.MainMusicAudioSource.mute = false;
        this.MainMusicAudioSource.Play();
    }

    public void PlayErrorSfx() {
        if(this.ErrorSfx) this.PlaySfxAtPoint(this.transform.position, this.ErrorSfx);
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
        float startVolume = this.MainMusicAudioSource.volume;
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.MainMusicAudioSource.volume = Mathf.Lerp(startVolume, newVolume, t / duration);
            yield return null;
        }
        this.MainMusicAudioSource.volume = newVolume;
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
        this.DialogAudioSource.volume = volume;
        this.DialogAudioSource.clip = newClip;
        this.DialogAudioSource.mute = false;
        this.DialogAudioSource.Play();

        this.DialogIsDecreaseMainMusicVolume = decreaseMainMusic;
        if (decreaseMainMusic) {
            this.MainMusicVolumeBeforeDialog = this.MainMusicAudioSource.volume;
            this.MainMusicAudioSource.volume = 0.3f;
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

    public void SetMainMusicVolume(float volume){
        this.MainMusicAudioSource.volume = volume;
    }

    public void PlaySfxAtPoint(Vector3 point, AudioClip clip, float volume = 1.0f){
        // AudioSource.PlayClipAtPoint(clip, point, volume);
        this.MainMusicAudioSource.PlayOneShot(clip, 0.5f);
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

}

