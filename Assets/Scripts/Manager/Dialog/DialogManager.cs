
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XNode;

public class DialogManager : MonoBehaviour {

    [Header("UI")] 
    [SerializeField] private GameObject BackGameObject;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private float BackgroundImageFadeInDuration = 0.5f;
    [SerializeField] private Transform SlotContainer;
    
    [SerializeField] private Image CharacterPortrait;
    [SerializeField] private TextMeshProUGUI CharacterName;
    // [SerializeField] private GameObject CharacterNameShadow;
    
    [SerializeField] private CanvasGroup DialogContentCanvasGroup;
    [SerializeField] private TypeWriter DialogText;
    [SerializeField] private StoryReviewUI StoryReview;

    [SerializeField] private Button SkipButton;
    // [SerializeField] private GameTipContainer TipContainer;
    
	[Header("AutoPlay")]
	[SerializeField] private Sprite AutoPlaySprite;
    [SerializeField] private Sprite NotAutoPlaySprite;
    [SerializeField] private Image AutoPlayImage;
    
    [Header("Background Character Portrait")]
    [SerializeField] private Transform CharacterPortraitContainer;

    [Header("Dialog Option")] 
    [SerializeField] private Transform DialogOptionContainer;
    [SerializeField] private DialogOption DialogOptionPrefab;

    [Header("Progress Bar")] 
    [SerializeField] private DialogAudioProgressBar ProgressBar;
    
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
    public bool IsInDialog => this.DialogCanvasGroup.alpha >= 0.1f;

    private bool CurrentDialogIsFinished => AudioManager.Instance.DialogIsFinished && this.DialogText.IsDelayEnd;

    private DialogNode CurrentDialogNode;

    private bool Pause = false;

    private Animator DialogAnimator;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DialogCanvasGroup = GetComponent<CanvasGroup>();
        BackgroundImageCanvasGroup = BackgroundImage.GetComponent<CanvasGroup>();
        DialogAnimator = GetComponent<Animator>();
        StartCoroutine(Transition(false, true));

        if (this.ProgressBar) {
            this.ProgressBar.OnProgress += () => {
                this.DialogContentCanvasGroup.interactable = false;
                this.DialogContentCanvasGroup.blocksRaycasts = false;
            };

            this.ProgressBar.OnEndProgress += () => {
                this.DialogContentCanvasGroup.interactable = true;
                this.DialogContentCanvasGroup.blocksRaycasts = true;
            };

            this.ProgressBar.OnChangeProgress += (seconds) => {
                AudioManager.Instance.SetDialogPlayPos(seconds);
                foreach (Transform child in this.SlotContainer) {
                    if (child.TryGetComponent(out DialogUISlot dus)) {
                        dus.DialogAudioChange(seconds);
                    }
                }
            };
        }
    }

    private void Start() {
        DialogEventManager.Instance.AddEvent("ShakeCamera", () => {
            this.DialogAnimator.SetTrigger(AnimationParams.Shake);
        });
        
        DialogEventManager.Instance.AddEvent("TurnRed", () => {
            this.DialogAnimator.SetTrigger(AnimationParams.Red);
        });
        
        DialogEventManager.Instance.AddEvent("GameOver", () => {
            GameManager.Instance.DungeonFail();
        });
    }

    private IEnumerator Transition(bool show, bool quick) {
        if (!show) {
            DialogCanvasGroup.interactable = false;
            DialogCanvasGroup.blocksRaycasts = false;
        }
        
        if (quick) {
            DialogCanvasGroup.alpha = show ? 1.0f : 0.0f;
        } else {
            float start = show ? 0.0f : 1.0f;
            float end = 1.0f - start;
            for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime) {
                this.DialogCanvasGroup.alpha = Mathf.Lerp(start, end, t / 0.5f);
                yield return null;
            }
            this.DialogCanvasGroup.alpha = end;
        }
        
        if (show) {
            DialogCanvasGroup.interactable = true;
            DialogCanvasGroup.blocksRaycasts = true;
        }
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
        SetAutoPlay(false);
        
        if (!dialog) return;
        StartNode startNode = FindStartNode(dialog);
        if (!startNode) return;
        this.SkipButton.gameObject.SetActive(startNode.CanSkip);

        NodePort startPort = startNode.GetOutputPort("NextDialog").Connection;
        if (startPort == null || startPort.node is not DialogNode dialogNode) return;
        
        this.CurrentDialogNode = dialogNode;
        this.StoryReview.Reset();
        // this.TipContainer.StartTip();
        SetDialog();
        StartCoroutine(Transition(true, false));
        
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
        StartCoroutine(Transition(false, false));
        SetAutoPlay(false);

        AudioManager.Instance.StopMainMusic();
        if(this.PreBGM) AudioManager.Instance.SetMainMusic(this.PreBGM);
        
        if (this.IsFullScreen) {
            CameraManager.Instance.MainCamera.cullingMask = ~LayerMask.GetMask("UI", "Map");
        }

        if (this.ProgressBar) {
            this.ProgressBar.StopProgressBar();
        }
        
        this.UnloadSlot();
        // this.TipContainer.EndTip();

        if (this.CurrentDialogNode.AfterDialogInvokeAction != "GameOver") {
            this.OnDialogEnded?.Invoke();
        }
    }

    private void UnloadSlot() {
        foreach (Transform child in this.SlotContainer) {
            if (child.TryGetComponent(out DialogUISlot dus)) {
                dus.End();
            }
            Destroy(child.gameObject);
        }
    }

    public void ShowDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.Transition(true);
        SetAutoPlay(false);
    }

    public void HideDialogReview() {
        AudioManager.Instance.StopDialog();
        this.StoryReview.Transition(false);
    }

    public void AutoPlay() {
        SetAutoPlay(!this.IsAutoPlay);
    }

    private void SetAutoPlay(bool isAutoPlay) {
        this.IsAutoPlay = isAutoPlay;
        if (this.IsAutoPlay) {
            this.AutoPlayImage.sprite = this.AutoPlaySprite;
        } else {
            this.AutoPlayImage.sprite = this.NotAutoPlaySprite;
        }
    }

    private IEnumerator BackgroundImageCoroutine(Sprite newBG, bool isFadeIn, bool isFadeOut) {
        if (isFadeOut) {
            yield return BackgroundImageFadeCoroutine(1.0f, 0.0f);
        }

        this.BackgroundImageCanvasGroup.alpha = 0.0f;
        this.BackgroundImage.sprite = newBG;

        if (isFadeIn) {
            yield return BackgroundImageFadeCoroutine(0.0f, 1.0f);
        }
        this.BackgroundImageCanvasGroup.alpha = 1.0f;
    }

    private IEnumerator BackgroundImageFadeCoroutine(float start, float end) {
        for (float t = 0.0f; t < this.BackgroundImageFadeInDuration; t += Time.deltaTime) {
            this.BackgroundImageCanvasGroup.alpha = Mathf.Lerp(start, end, t / this.BackgroundImageFadeInDuration);
            yield return null;
        }
        this.BackgroundImageCanvasGroup.alpha = end;
    }

    private void SetDialog() {
        DialogNode data = this.CurrentDialogNode;
        
        this.BackGameObject.SetActive(!data.NotBackground);
        if (!data.NotBackground && data.Background) {
            StopAllCoroutines();
            StartCoroutine(BackgroundImageCoroutine(data.Background, data.BackgroundIsFadeIn, data.BackgroundIsFadeOut));
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

        this.UnloadSlot();
        if (data.SlotPrefab) {
            DialogUISlot dus = Instantiate(data.SlotPrefab, this.SlotContainer);
            dus.Init();
        }

        this.CharacterPortrait.color = new Color(1, 1, 1, data.CharacterPortrait ? 1.0f : 0.0f); 
        this.CharacterPortrait.sprite = data.CharacterPortrait;
        this.CharacterName.text = data.CharacterName;
        // this.CharacterNameShadow.SetActive(this.CharacterName.text != "");

        this.HasSound = data.DialogAudio;
        bool notDialog = data.DialogText == "" || (data.HasProgressBar && data.DialogAudio);
        bool hasProgressBar = data.HasProgressBar && this.HasSound;
        
        DialogContentCanvasGroup.alpha = notDialog ? 0.0f : 1.0f;
        this.DialogText.Play(data.DialogText, data.DialogTypeWriterDuration, data.IsConstantVelocity, data.AutoPlayIfNotContent);

        if (data.DialogTipTexts != null && data.DialogTipTexts.Length > 0) {
            foreach (string tip in data.DialogTipTexts) {
                // this.TipContainer.AddTip(tip);
                SceneChangeManager.Instance.AddGameTip(tip);
            }
        }

        if (data.ClueNames != null && data.ClueNames.Length > 0) {
            foreach (string clueName in data.ClueNames) {
                if (string.IsNullOrWhiteSpace(clueName)) continue;
                // this.TipContainer.AddTip($"获得线索：{clueName}");
                SceneChangeManager.Instance.AddGameTip($"获得线索：{clueName}");
            }
        }

        if (data.DialogBGM) {
            if (data.IsDialogBGMFade) {
                AudioManager.Instance.FadeMainMusic(data.DialogBGM, data.DialogBGMFadeTime, data.BGMVolume);
            } else {
                AudioManager.Instance.SetMainMusic(data.DialogBGM, data.BGMVolume);   
            }
        }
        
        this.ProgressBar.StopProgressBar();
        this.ProgressBar.gameObject.SetActive(hasProgressBar);
        if (hasProgressBar) {
            this.ProgressBar.StartProgressBar(data.DialogAudio.length);
        }
        
        AudioManager.Instance.StopDialog();
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

        if (data.DialogText != "") {
            this.StoryReview.AddDialogHistory(data);
        }
        
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

