
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class StartVideoControl : MonoBehaviour {

    [Header("Video Player")] 
    [SerializeField] private VideoPlayer FirstPlayer;
    [SerializeField] private VideoPlayer SecondPlayer;
    
    [Header("Clip")]
    [SerializeField] private VideoClip StartVideoClip;
    [SerializeField] private VideoClip LoopVideoClip;
    [SerializeField] private GameObject StartButtons;

    private void Start() {
        this.StartButtons.SetActive(false);
        this.FirstPlayer.isLooping = false;
        this.FirstPlayer.clip = this.StartVideoClip;
        this.FirstPlayer.Prepare();  
        
        this.SecondPlayer.isLooping = true;
        this.SecondPlayer.clip = this.LoopVideoClip;
        this.SecondPlayer.Prepare();
        this.SecondPlayer.enabled = false;
        
        this.FirstPlayer.prepareCompleted += OnVideoPrepareCompleted;
        this.FirstPlayer.loopPointReached += OnPlayEnded;
    }
    
    private void OnVideoPrepareCompleted(VideoPlayer source) {
        source.playbackSpeed = 1.0f;
        source.Play();
    }

    private void OnPlayEnded(VideoPlayer source) {
        this.FirstPlayer.loopPointReached -= OnPlayEnded;
        this.FirstPlayer.Stop();
        this.FirstPlayer.enabled = false;
        this.SecondPlayer.enabled = true;
        this.OnVideoPrepareCompleted(this.SecondPlayer);
        this.StartButtons.SetActive(true); 
    }
}
