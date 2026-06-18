
using UnityEngine;
using UnityEngine.Events;
using XNode;

[Serializable]
public struct OptionEndingFlagsData{
    public EndingFlags Flag;
    public int Value;  
}

[Serializable]
public struct OptionData {
    public string OptionContent;
    public OptionEndingFlagsData[] EndingFlagsDatas;
}

public class DialogNode : Node, ISerializationCallbackReceiver {

    [Input] public Node PreNode;

    [Header("Clue")]
    [ScriptableObjectNameProp(typeof(ClueData), "ClueName")]
    public string[] ClueNames;
    
    [Header("Background")]
    public Sprite Background;
    public Sprite[] BackgroundCharacterPortraits;
    public bool NotBackground;
    public bool BackgroundIsFadeIn;
    public bool BackgroundIsFadeOut = false;
    public DialogUISlot SlotPrefab;
    public string[] DialogTipTexts;
    
    [Header("Music")]
    public AudioClip DialogAudio;
    public AudioClip DialogBGM;
    public bool IsDialogBGMFade = true;
    public float DialogBGMFadeTime = 1.0f;
    public float CharacterAudioVolume = 1.0f;
    public float BGMVolume = 1.0f;
    
    [Header("Text")]
    [TextArea] public string DialogText;
    public bool IsConstantVelocity = false;
    public float DialogTypeWriterDuration = 1.0f;
    public string CharacterName;
    public Sprite CharacterPortrait;

    [Header("ProgressBar")]
    public bool HasProgressBar = false;
    public bool AutoPlayIfNotContent = true;
    
    [Header("Action")]
    public string BeforeDialogInvokeAction;
    public string AfterDialogInvokeAction;
    
    [FormerlySerializedAs("Options")]
    [SerializeField, HideInInspector]
    private string[] _legacyOptions;

    [Output(dynamicPortList = true)] public OptionData[] Options;
    [Output] public Node NextDialog;
    
    public override object GetValue(NodePort port) {
        return null;
    }

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() {
        if (_legacyOptions != null && _legacyOptions.Length > 0) {
            Options = _legacyOptions.Select(s => new OptionData { OptionContent = s }).ToArray();
            _legacyOptions = null;
        }
    }
}


