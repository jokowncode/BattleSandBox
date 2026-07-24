
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
using XNode;

public class DialogManager : MonoBehaviour {

    [Header("UI")] 
    [SerializeField] private GameObject BackGameObject;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private float BackgroundImageFadeInDuration = 0.5f;
    [SerializeField] private Transform SlotContainer;
    
    [SerializeField] private Image CharacterPortrait;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private GameObject NameGameObject;
    // [SerializeField] private GameObject CharacterNameShadow;
    
    [SerializeField] private CanvasGroup DialogContentCanvasGroup;
    [SerializeField] private TypeWriter DialogText;
    [SerializeField] private StoryReviewUI StoryReview;

    [SerializeField] private Button SkipButton;
    [SerializeField] private GameObject ClickArea;
    // [SerializeField] private GameTipContainer TipContainer;

    [SerializeField] private StoryVideo Video;
    [SerializeField] private ExploreArea Explore;
    
	[Header("AutoPlay")]
	[SerializeField] private Sprite AutoPlaySprite;
    [SerializeField] private Sprite AutoPlayHighlightSprite;
    [SerializeField] private Sprite NotAutoPlaySprite;
    [SerializeField] private Sprite NotAutoPlayHighlightSprite;
    [SerializeField] private Image AutoPlayImage;
    [SerializeField] private Button AutoPlayButton;
    
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

    private Node CurrentNode;

    private bool Pause = false;

    private Animator DialogAnimator;

    public bool IsExplore => this.CurrentNode is ExploreNode;
    public bool IsVideo => this.Video && this.Video.IsPlayVideo;
    
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

        if (this.Video) this.Video.OnVideoEnded += this.NextDialog;
        if (this.Explore) {
            this.Explore.OnExploreAllGoods += this.NextDialog;
            this.Explore.OnClickExplore += this.OnClickExplore;
        }
    }

    private void OnClickExplore(ExploreMapping mapping) {
        if (mapping.Type == ExploreType.Goods) {
            if (mapping.GoodsData) {
                if (!GoodsWarehouseManager.Instance || GoodsWarehouseManager.Instance.AddGoods(mapping.GoodsData)) {
                    SceneChangeManager.Instance.AddGameTip($"获得物品：{mapping.GoodsData.GoodsShowName}");
                }
            }
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
        this.enabled = false;
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

    public void TransitionClickArea(bool show) {
        if (this.ClickArea) this.ClickArea.SetActive(show);
    }

    private void Update() {
        if (this.IsVideo || this.IsExplore) return;
        if (!this.IsAutoPlay && Input.GetKeyDown(KeyCode.Space)) {
            this.Next();
        }
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

    private bool GetNextNode(NodePort port, out Node nextNode) {
        nextNode = null;
        if (port == null) return false;

        if (port.node is DialogNode node) {
            nextNode = node;
            return true;
        }

        if (port.node is ExploreNode node2) {
            nextNode = node2;
            return true;
        }

        if(port.node is EndingFlagsConditionNode conditionNode) {
            int value = SaveDataManager.Instance.GetCurrentEndingFlagsValue(conditionNode.ReferenceFlags);
            bool flag = false;
            switch (conditionNode.Comparator) {
            case Comparator.等于:
                flag = value == conditionNode.CompareValue;
                break;
            case Comparator.小于:
                flag = value < conditionNode.CompareValue;
                break;
            case Comparator.小于等于:
                flag = value <= conditionNode.CompareValue;
                break;
            case Comparator.大于:
                flag = value > conditionNode.CompareValue;
                break;
            case Comparator.大于等于:
                flag = value >= conditionNode.CompareValue;
                break;    
            }
            NodePort nextPort = conditionNode.GetOutputPort(flag ? "TrueNode" : "FalseNode").Connection;
            bool result = GetNextNode(nextPort, out Node resultNode);
            nextNode = resultNode;
            return result;
        }
        return false;
    }
    
    public void PlayNewDialog(DialogGraph dialog, bool isFullScreen = true) {
        this.enabled = true;
        this.IsFullScreen = isFullScreen;
        this.PreBGM = AudioManager.Instance.GetCurrentMainMusic();
        AudioManager.Instance.StopFootstep();
        SetAutoPlay(false);
        
        if (!dialog) return;
        StartNode startNode = FindStartNode(dialog);
        if (!startNode) return;
        this.SkipButton.gameObject.SetActive(startNode.CanSkip);

        NodePort startPort = startNode.GetOutputPort("NextDialog").Connection;
        if (startPort == null || !GetNextNode(startPort, out Node nextNode)) return;
        
        this.CurrentNode = nextNode;
        this.StoryReview.Reset();
        // this.TipContainer.StartTip();
        SetNode();
        StartCoroutine(Transition(true, false));
        
        if (this.IsFullScreen) {
            CameraManager.Instance.MainCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
        }
    }

    private void SetNode() {
        if (this.Explore) this.Explore.Hide();
        if (this.CurrentNode is DialogNode) {
            this.SetDialog();
        } else if (this.CurrentNode is ExploreNode) {
            this.SetExplore();
        } else {
            throw new Exception($"Invalid Node Type：{this.CurrentNode.GetType().Name}");
        }
    }

    private void SetExplore() {
        ExploreNode data = this.CurrentNode as ExploreNode;
        if (!data) throw new Exception("Node Type is Wrong");
        if (!data.ExploreCG || data.Mappings == null || data.Mappings.Count == 0) throw new Exception("Explore Node is not complete!");
        this.Explore.Show(data);
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
        
        if (this.CurrentNode is DialogNode { AfterDialogInvokeAction: not null } dialogNode) {
            DialogEventManager.Instance.RaiseEvent(dialogNode.AfterDialogInvokeAction);
        }
        
        NodePort nextPort = this.CurrentNode.GetOutputPort("NextDialog").Connection;
        if (nextPort == null || !GetNextNode(nextPort, out Node nextNode)) {
            DialogEnd();
            return;
        }

        this.CurrentNode = nextNode;
        SetNode();
    }

    public void DialogEnd() {
        this.enabled = false;
        AudioManager.Instance.StopDialog();
        bool notQuick = this.CurrentNode is DialogNode dialog && !dialog.Video;
        StartCoroutine(Transition(false, !notQuick));
        SetAutoPlay(false);
        
        if (this.PreBGM && this.PreBGM != AudioManager.Instance.GetCurrentMainMusic()) {
            AudioManager.Instance.StopMainMusic();
            AudioManager.Instance.SetMainMusic(this.PreBGM);
        }

        if (this.IsFullScreen) {
            CameraManager.Instance.MainCamera.cullingMask = ~LayerMask.GetMask("UI", "Map");
        }

        if (this.ProgressBar) {
            this.ProgressBar.StopProgressBar();
        }
        
        this.UnloadSlot();
        if (this.Video) this.Video.StopVideo();
        // this.TipContainer.EndTip();

        if (this.CurrentNode is DialogNode dialogNode && dialogNode.AfterDialogInvokeAction != "GameOver") {
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
        SpriteState currentSpriteState = AutoPlayButton.spriteState;
        if (this.IsAutoPlay) {
            this.AutoPlayImage.sprite = this.AutoPlaySprite;
            currentSpriteState.disabledSprite = this.AutoPlaySprite;
            currentSpriteState.highlightedSprite = this.AutoPlayHighlightSprite;
        } else {
            this.AutoPlayImage.sprite = this.NotAutoPlaySprite;
            currentSpriteState.disabledSprite = this.NotAutoPlaySprite;
            currentSpriteState.highlightedSprite = this.NotAutoPlayHighlightSprite;
        }
        AutoPlayButton.spriteState = currentSpriteState;
        EventSystem.current.SetSelectedGameObject(null);
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
        DialogNode data = this.CurrentNode as DialogNode;
        if (!data) throw new Exception("Node Type is Wrong");

        if (this.Video) {
            if (data.Video) {
                this.Video.PlayVideo(data.Video);
                return;
            }
            else this.Video.StopVideo();
        }
        
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
        this.NameGameObject.SetActive(!String.IsNullOrWhiteSpace(data.CharacterName));
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
            AudioManager.Instance.SetDialog(data.DialogAudio, false, data.CharacterAudioVolume);
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

    public void ClickOption(int index, OptionData data) {
        NodePort optionPort = this.CurrentNode.GetPort($"Options {index}").Connection;
        if (optionPort == null || !GetNextNode(optionPort, out Node nextNode)) {
            DialogEnd();
            return;
        }

        this.CurrentNode = nextNode;
        this.IsChooseOption = true;
        this.StoryReview.AddDialogOptionHistory(data.OptionContent);
        AudioManager.Instance.StopDialog();
        SetNode();

        if (data.EndingFlagsDatas != null && data.EndingFlagsDatas.Length != 0) {
            foreach (OptionEndingFlagsData flagDatas in data.EndingFlagsDatas) {
                SaveDataManager.Instance.AddCurrentEndingFlagsValue(flagDatas.Flag, flagDatas.Value);
            }
        }
    }

    public void TransitionPause(bool pause) {
        this.Pause = pause;
    }
}

