
using UnityEngine;

public abstract class DialogUISlot : MonoBehaviour {

    public abstract void Init();
    public abstract void End();
    public virtual void DialogAudioChange(float seconds) { }
}


