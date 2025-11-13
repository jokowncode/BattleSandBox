
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class DialogManager : MonoBehaviour {

    [Header("UI")] 
    [SerializeField] private GameObject BackGameObject;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private float BackgroundImageFadeInDuration = 0.5f;
    
    [SerializeField] private Image CharacterPortrait;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private TypeWriter DialogText;
    [SerializeField] private StoryReviewUI StoryReview;
    
    [Header("Background Character Portrait")]
    [SerializeField] private Transform CharacterPortraitContainer;

    [Header("Dialog Option")] 
    [SerializeField] private Transform DialogOptionContainer;
    [SerializeField] private DialogOption DialogOptionPrefab;
    
    public static DialogManager Instance;
    // private int CurrentIndex;
    private CanvasGroup DialogCanvasGroup;
    public bool IsAutoPlay { get; private set; } = false;
    private bool IsChooseOption = true;
    private bool HasSound = false;
    private bool IsFullScreen = false;

    // private StoryDialogData[] Dialogs;
    private AudioClip PreBGM;

    public Action OnDialogEnded;

    private CanvasGroup BackgroundImageCanvasGroup;
    public bool IsInDialog => this.DialogCanvasGroup.alpha >= 0.9f;

    private bool CurrentDialogIsFinished =>
        this.HasSound ? AudioManager.Instance.DialogIsFinished : this.DialogText.IsDelayEnd;

    private DialogNode CurrentDialogNode;

    private bool Pause = false;
    
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

    private StartNode FindStartNode(DialogGraph graph) {
        List<Node> nodes = graph.nodes;
        foreach (Node node in nodes) {
            if (node is StartNode startNode) return startNode;
        }
        return null;
    }
    
    public void PlayNewDialog(DialogGraph dialog, bool isFullScreen = true) {
        this.IsFullScreen = isFullScreen;
        this.PreBGM = AudioManager.Instance.GetCurrentMainMusic();
        AudioManager.Instance.StopFootstep();
        
        if (!dialog) return;
        StartNode startNode = FindStartNode(dialog);
        if (!startNode) return;

        NodePort startPort = startNode.GetOutputPort("NextDialog").Connection;
        if (startPort == null || startPort.node is not DialogNode dialogNode) return;
        
        this.CurrentDialogNode = dialogNode;
        this.StoryReview.Reset();
        SetDialog();
        Transition(true);
        
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
        if (this.Pause) {
            return;
        }

        if (!this.IsChooseOption) {
            return;
        }
        
        if (this.CurrentDialogNode.AfterDialogInvokeAction != null) {
            DialogEventManager.Instance.RaiseEvent(this.CurrentDialogNode.AfterDialogInvokeAction);
        }
        
        NodePort nextPort = this.CurrentDialogNode.GetOutputPort("NextDialog").Connection;
        if (nextPort == null || nextPort.node is not DialogNode dialogNode) {
            DialogEnd();
            return;
        }

        this.CurrentDialogNode = dialogNode;
        SetDialog();
    }

    public void DialogEnd() {
        AudioManager.Instance.StopDialog();
        Transition(false);
        this.IsAutoPlay = false;

        AudioManager.Instance.StopMainMusic();
        if(this.PreBGM) AudioManager.Instance.SetMainMusic(this.PreBGM);
        
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
        DialogNode data = this.CurrentDialogNode;
        
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

        foreach (Transform child in this.CharacterPortraitContainer) {
            child.gameObject.SetActive(false);
        }
        if (data.BackgroundCharacterPortraits != null && data.BackgroundCharacterPortraits.Length > 0) {
            for (int i = 0; i < data.BackgroundCharacterPortraits.Length; i++) {
                if(this.CharacterPortraitContainer.GetChild(i).TryGetComponent(out Image portraitImage)){
                    portraitImage.gameObject.SetActive(true);
                    portraitImage.sprite = data.BackgroundCharacterPortraits[i];
                }
            }
        }

        this.CharacterPortrait.color = new Color(1, 1, 1, data.CharacterPortrait ? 1.0f : 0.0f); 
        this.CharacterPortrait.sprite = data.CharacterPortrait;
        this.CharacterName.text = data.CharacterName;
        this.DialogText.Play(data.DialogText);

        this.HasSound = data.DialogAudio;
        if (data.DialogBGM) {
            AudioManager.Instance.FadeMainMusic(data.DialogBGM, 2.0f, data.BGMVolume);
        }
        
        if (data.DialogAudio) {
            AudioManager.Instance.SetDialog(data.DialogAudio, data.CharacterAudioVolume);
        }
        
        foreach (Transform option in this.DialogOptionContainer) {
            Destroy(option.gameObject);
        }
        this.IsChooseOption = data.Options == null || data.Options.Length == 0;
        if (data.Options != null) {
            for (int i = 0; i < data.Options.Length; i++) {
                DialogOption option = Instantiate(this.DialogOptionPrefab, this.DialogOptionContainer);
                option.SetOptionData(data.Options[i], i);
            }
        }
        this.StoryReview.AddDialogHistory(data);
        
        if (data.BeforeDialogInvokeAction != null) {
            DialogEventManager.Instance.RaiseEvent(data.BeforeDialogInvokeAction);
        }
    }

    public void ClickOption(int index, string optionText) {
        
        NodePort optionPort = this.CurrentDialogNode.GetPort($"Options {index}").Connection;
        if (optionPort == null || optionPort.node is not DialogNode dialogNode) {
            DialogEnd();
            return;
        }

        this.CurrentDialogNode = dialogNode;
        this.IsChooseOption = true;
        this.StoryReview.AddDialogOptionHistory(optionText);
        AudioManager.Instance.StopDialog();
        SetDialog();
    }

    public void TransitionPause(bool pause) {
        this.Pause = pause;
    }
}

