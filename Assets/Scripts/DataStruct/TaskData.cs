
using System;
using UnityEngine;

[Serializable]
public struct TaskData {
    public string TaskDescription;
    public Transform TaskPosition;
    public InteractionObject[] ActivateInteractionObjects;
}

