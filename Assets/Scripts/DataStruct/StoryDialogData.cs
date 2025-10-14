
using System;
using UnityEngine;

[Serializable]
public struct DialogOptionData {
    public string OptionText;
    public int NextDialogIndex;
}

[Serializable]
public struct StoryDialogData {
    public Sprite Background;
    public bool BackgroundIsFadeIn;
    public AudioClip DialogAudio;
    [TextArea] public string DialogText;
    public string CharacterName;
    public Sprite CharacterPortrait;
    public DialogOptionData[] Options;
    public int NextDialogIndex;
}

