
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour{

    public static AudioManager Instance;

    [SerializeField] private AudioSource MainMusicAudioSource;
    [SerializeField] private AudioSource FootstepAudioSource;
    [SerializeField] private AudioClip[] FootstepAudios;

    private void Awake(){
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.MainMusicAudioSource = GetComponent<AudioSource>();
    }

    public void SetMainMusic(AudioClip newClip){
        this.MainMusicAudioSource.mute = true;
        this.MainMusicAudioSource.clip = newClip;
        this.MainMusicAudioSource.mute = false;
        this.MainMusicAudioSource.Play();
    }

    public void SetMainMusicVolume(float volume){
        this.MainMusicAudioSource.volume = volume;
    }

    public void Fade(float newVolume, float duration = 0.5f){
        StartCoroutine(FadeCoroutine(newVolume, duration));
    }

    private IEnumerator FadeCoroutine(float newVolume, float duration = 0.5f){
        float startVolume = this.MainMusicAudioSource.volume;
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.MainMusicAudioSource.volume = Mathf.Lerp(startVolume, newVolume, t / duration);
            yield return null;
        }
        this.MainMusicAudioSource.volume = newVolume;
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

