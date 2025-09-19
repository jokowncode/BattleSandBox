
using System;
using UnityEngine;

[Serializable]
public struct StoryDialogData {
    public Sprite Background;
    public AudioClip DialogAudio;
    [TextArea] public string DialogText;
    public string CharacterName;
    public Sprite CharacterPortrait;
}

