
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour{

    public static AudioManager Instance;

    private AudioSource MainMusicAudioSource;

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
        AudioSource.PlayClipAtPoint(clip, point, volume);
    }

}

