
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class DialogNode : Node {

    [Input] public Node PreNode;
    
    public Sprite Background;
    public Sprite[] BackgroundCharacterPortraits;
    public bool NotBackground;
    public bool BackgroundIsFadeIn;
    public bool BackgroundIsFadeOut = false;
    
    public AudioClip DialogAudio;
    public AudioClip DialogBGM;
    public bool IsDialogBGMFade = true;
    public float DialogBGMFadeTime = 1.0f;
    public float CharacterAudioVolume = 1.0f;
    public float BGMVolume = 1.0f;
    
    [TextArea] public string DialogText;
    public bool IsConstantVelocity = false;
    public float DialogTypeWriterDuration = 1.0f;
    public string CharacterName;
    public Sprite CharacterPortrait;
    
    public string BeforeDialogInvokeAction;
    public string AfterDialogInvokeAction;
    
    [Output(dynamicPortList = true)] public string[] Options;
    [Output] public Node NextDialog;
    
    public override object GetValue(NodePort port) {
        return null;
    }
}


