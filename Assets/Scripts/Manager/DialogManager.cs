
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    [Header("UI")] 
    [SerializeField] private GameObject BackGameObject;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private float BackgroundImageFadeInDuration = 0.5f;
    
    [SerializeField] private Image CharacterPortrait;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private TypeWriter DialogText;
    [SerializeField] private StoryReviewUI StoryReview;

    [Header("Dialog Option")] 
    [SerializeField] private Transform DialogOptionContainer;
    [SerializeField] private DialogOption DialogOptionPrefab;
    
    public static DialogManager Instance;
    private int CurrentIndex;
    private CanvasGroup DialogCanvasGroup;
    public bool IsAutoPlay { get; private set; } = false;
    private bool IsChooseOption = true;
    private bool HasSound = false;
    private bool IsFullScreen = false;

    private StoryDialogData[] Dialogs;
    private AudioClip PreBGM;
    private bool HasDialogBGM = false;

    public Action OnDialogEnded;

    private CanvasGroup BackgroundImageCanvasGroup;
    public bool IsInDialog => this.DialogCanvasGroup.alpha >= 0.9f;

    private bool CurrentDialogIsFinished =>
        this.HasSound ? AudioManager.Instance.DialogIsFinished : this.DialogText.IsDelayEnd;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DialogCanvasGroup = GetComponent<CanvasGroup>();
        BackgroundImageCanvasGroup = BackgroundImage.GetComponent<CanvasGroup>();
        Transition(false);
    }

    private void Transition(bool show) {
        DialogCanvasGroup.alpha = show ? 1.0f : 0.0f;
        DialogCanvasGroup.interactable = show;
        DialogCanvasGroup.blocksRaycasts = show;
    }

    private void Update() {
        if (!this.IsAutoPlay) return;
        if (this.CurrentDialogIsFinished) NextDialog();
    }

    public void PlayNewDialog(StoryDialogData[] dialogs, AudioClip dialogBGM, bool isFullScreen = true) {
        this.Dialogs = dialogs;
        this.IsFullScreen = isFullScreen;
        if (this.Dialogs.Length == 0) return;
        this.StoryReview.Reset();
        this.CurrentIndex = 0;
        SetDialog();
        Transition(true);

        this.HasDialogBGM = dialogBGM;
        if (dialogBGM) {
            this.PreBGM = AudioManager.Instance.GetCurrentMainMusic();
            AudioManager.Instance.SetMainMusic(dialogBGM);
        }

        if (this.IsFullScreen) {
            CameraManager.Instance.MainCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
        }
    }

    public void Next() {
        if (this.DialogText.EndText()) {
            NextDialog();
        }
    }

    private void NextDialog() {
        if (!this.IsChooseOption) {
            return;
        }

        this.CurrentIndex = this.Dialogs[this.CurrentIndex].NextDialogIndex;
        if (this.CurrentIndex < 0 || this.CurrentIndex >= this.Dialogs.Length) {
            DialogEnd();
            return;
        }
        SetDialog();
    }

    public void DialogEnd() {
        AudioManager.Instance.StopDialog();
        Transition(false);
        this.IsAutoPlay = false;

        if (this.HasDialogBGM) {
            AudioManager.Instance.StopMainMusic();
            if(this.PreBGM) AudioManager.Instance.SetMainMusic(this.PreBGM);
        }
        
        if (this.IsFullScreen) {
            CameraManager.Instance.MainCamera.cullingMask = ~0;
        }
        
        this.OnDialogEnded?.Invoke();
    }

    public void ShowDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.Transition(true);
        this.IsAutoPlay = false;
    }

    public void HideDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.Transition(false);
    }

    public void AutoPlay() {
        this.IsAutoPlay = !this.IsAutoPlay;
    }

    private IEnumerator BackgroundImageFadeInCoroutine() {
        for (float t = 0.0f; t < this.BackgroundImageFadeInDuration; t += Time.deltaTime) {
            this.BackgroundImageCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t / this.BackgroundImageFadeInDuration);
            yield return null;
        }
        this.BackgroundImageCanvasGroup.alpha = 1.0f;
    }

    private void SetDialog() {
        StoryDialogData data = this.Dialogs[this.CurrentIndex];
        
        this.BackGameObject.SetActive(!data.NotBackground);
        if (!data.NotBackground && data.Background) {
            this.BackgroundImage.sprite = data.Background;
            this.BackgroundImageCanvasGroup.alpha = 0.0f;
            if (data.BackgroundIsFadeIn) {
                StopAllCoroutines();
                StartCoroutine(BackgroundImageFadeInCoroutine());
            } else {
                this.BackgroundImageCanvasGroup.alpha = 1.0f;
            }
        }
        this.CharacterPortrait.color = new Color(1, 1, 1, data.CharacterPortrait ? 1.0f : 0.0f); 
        this.CharacterPortrait.sprite = data.CharacterPortrait;
        this.CharacterName.text = data.CharacterName;
        this.DialogText.Play(data.DialogText);

        this.HasSound = data.DialogAudio;
        if (data.DialogAudio) {
            AudioManager.Instance.SetDialog(data.DialogAudio);
        }

        foreach (Transform option in this.DialogOptionContainer) {
            Destroy(option.gameObject);
        }
        this.IsChooseOption = data.Options == null || data.Options.Length == 0;
        if (data.Options != null) {
            foreach (DialogOptionData optionData in data.Options) {
                DialogOption option = Instantiate(this.DialogOptionPrefab, this.DialogOptionContainer);
                option.SetOptionData(optionData.OptionText, optionData.NextDialogIndex);
            }
        }
        this.StoryReview.AddDialogHistory(data);
    }

    public void ClickOption(int nextDialogIndex, string optionText) {
        if (nextDialogIndex < 0) {
            DialogEnd();
            return;
        }
        this.CurrentIndex = nextDialogIndex;
        this.IsChooseOption = true;
        this.StoryReview.AddDialogOptionHistory(optionText);
        SetDialog();
    }
}

