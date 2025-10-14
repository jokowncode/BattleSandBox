
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class DialogNode : Node {

    [Input] public Node PreNode;
    
    public Sprite Background;
    public bool NotBackground;
    public bool BackgroundIsFadeIn;
    public AudioClip DialogAudio;
    [TextArea] public string DialogText;
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


