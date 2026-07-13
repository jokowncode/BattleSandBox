
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StoryVideo : MonoBehaviour {

	[SerializeField] private GameObject VSkipProgressBar;
	[SerializeField] private float ProgressSpeed = 0.2f;
	
	private VideoPlayer Player;
	private Image VSkipProgressImage;
	private bool IsSkipVideoClick = false;

	public bool IsPlayVideo => this.enabled;
	public Action OnVideoEnded;
	
	private void Awake() {
		this.Player = this.GetComponent<VideoPlayer>();
		if (this.VSkipProgressBar) this.VSkipProgressImage = this.VSkipProgressBar.GetComponentInChildren<Image>();
		this.enabled = false;
	}

	public void PlayVideo(VideoClip clip) {
		if (!clip) return;
		this.enabled = true;
		this.gameObject.SetActive(true);
		this.Player.clip = clip;
		this.Player.isLooping = false;
		this.Player.loopPointReached += OnVideoPlayEnded;
		this.Player.Play();
	}
	
	private void OnVideoPlayEnded(VideoPlayer source) {
		this.EndVideo();
	}

	public void StopVideo() {
		this.enabled = false;
		this.gameObject.SetActive(false);
		this.Player.clip = null;
		this.Player.Stop();
	}

	private void EndVideo() {
		this.Player.loopPointReached -= OnVideoPlayEnded;
		this.StopVideo();
		this.OnVideoEnded?.Invoke();
	}

	private void Update() {
		if (!this.VSkipProgressImage) return;
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !this.IsSkipVideoClick) {
			this.VSkipProgressImage.fillAmount = 0.0f;
			this.IsSkipVideoClick = true;
		}
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && this.IsSkipVideoClick) {
			this.IsSkipVideoClick = false;
		}
		this.VSkipProgressBar.SetActive(this.IsSkipVideoClick);
		if (this.IsSkipVideoClick) {
			this.VSkipProgressImage.fillAmount += this.ProgressSpeed * Time.deltaTime;
		}
		if (this.VSkipProgressImage.fillAmount >= 1.0f) {
			this.VSkipProgressBar.SetActive(false);
			this.IsSkipVideoClick = false;
			this.EndVideo();
		}
	}
}

