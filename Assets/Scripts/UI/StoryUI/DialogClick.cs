
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogClick : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (DialogManager.Instance.IsAutoPlay) return;
        DialogManager.Instance.Next();
    }
}

