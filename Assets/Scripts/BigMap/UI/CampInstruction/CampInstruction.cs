

using UnityEngine;

public abstract class CampInstruction : MonoBehaviour {

    [SerializeField] private float ShowTime = 1.5f;
    
    private void Awake() {
        this.gameObject.SetActive(false);
        if (!ShowCondition()) return;
        this.gameObject.SetActive(true);
        if (this.ShowTime > 0.0f) Invoke(nameof(Disappear), this.ShowTime);
        AfterShow();
    }
    
    protected abstract bool ShowCondition();
    protected virtual void AfterShow() { }

    protected void Disappear() {
        this.gameObject.SetActive(false);
    }
}




