
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    // TODO: Link
    [SerializeField] private StoryDialogData[] Dialogs;

    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image CharacterPortrait;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private TypeWriter DialogText;
    [SerializeField] private StoryReviewUI StoryReview;
    
    public static DialogManager Instance;
    private int CurrentIndex;
    private CanvasGroup DialogCanvasGroup;
    public bool IsAutoPlay { get; private set; } = false;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DialogCanvasGroup = GetComponent<CanvasGroup>();
        Transition(false);
    }

    private void Transition(bool show) {
        DialogCanvasGroup.alpha = show ? 1.0f : 0.0f;
        DialogCanvasGroup.interactable = show;
        DialogCanvasGroup.blocksRaycasts = show;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            PlayDialog();
        }
    }

    public void PlayDialog() {
        if (this.Dialogs.Length == 0) return;
        this.CurrentIndex = 0;
        SetDialog();
        Transition(true);
        this.StoryReview.SetDialogs(this.Dialogs);
    }

    public void Next() {
        if (!this.DialogText.IsEnd) {
            this.DialogText.EndText();
            return;
        }
        NextDialog();
    }

    private void NextDialog() {
        this.CurrentIndex++;
        if (this.CurrentIndex >= this.Dialogs.Length) {
            DialogEnd();
            return;
        }
        SetDialog();
    }

    public void DialogEnd() {
        AudioManager.Instance.StopDialog();
        Transition(false);
    }

    public void ShowDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.ShowDialogReview(this.CurrentIndex);
        if (this.IsAutoPlay) {
            AutoPlay();
        }
    }

    public void HideDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.HideDialogReview();
    }

    public void AutoPlay() {
        this.IsAutoPlay = !this.IsAutoPlay;
        if (this.IsAutoPlay) {
            AudioManager.Instance.OnDialogFinished += NextDialog;
            if (AudioManager.Instance.DialogIsFinished) {
                NextDialog();
            }
        } else {
            AudioManager.Instance.OnDialogFinished -= NextDialog;
        }
    }

    private void SetDialog() {
        StoryDialogData data = this.Dialogs[this.CurrentIndex];
        
        if (data.Background) {
            this.BackgroundImage.sprite = data.Background;
        }
        this.CharacterPortrait.sprite = data.CharacterPortrait;
        this.CharacterName.text = data.CharacterName;
        this.DialogText.Play(data.DialogText);
        AudioManager.Instance.SetDialog(data.DialogAudio);
    }
}

