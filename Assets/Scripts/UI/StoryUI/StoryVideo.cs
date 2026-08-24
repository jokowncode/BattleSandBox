
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StoryVideo : MonoBehaviour {

	[SerializeField] private GameObject VSkipProgressBar;
	[SerializeField] private Image VSkipProgressImage;
	[SerializeField] private float ProgressSpeed = 0.2f;
	
	private VideoPlayer Player;
	private bool IsSkipVideoClick = false;

	public bool IsPlayVideo => this.enabled;
	public Action OnVideoEnded;
	
	private void Awake() {
		this.Player = this.GetComponent<VideoPlayer>();
		this.Player.loopPointReached += OnVideoPlayEnded;
		this.Player.prepareCompleted += OnVideoPrepare;
		
		this.VSkipProgressBar.SetActive(false);
		this.enabled = false;
	}

	private void Start() {
		this.Player.renderMode = VideoRenderMode.CameraFarPlane;
		this.Player.targetCamera = CameraManager.Instance.UICamera ? CameraManager.Instance.UICamera : CameraManager.Instance.MainCamera;
	}

	public void PlayVideo(VideoClip clip) {
		if (!clip) return;
		this.enabled = true;
		this.gameObject.SetActive(true);
		this.VSkipProgressImage.fillAmount = 0.0f;
		this.Player.clip = clip;
		this.Player.isLooping = false;
		this.Player.Prepare();
	}
	
	private void OnVideoPrepare(VideoPlayer source) {
		source.playbackSpeed = 1.0f;
		source.Play();
	}

	private void OnVideoPlayEnded(VideoPlayer source) {
		if (!this.enabled) return;
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

