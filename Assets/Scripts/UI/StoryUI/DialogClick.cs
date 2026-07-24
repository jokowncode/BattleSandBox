
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogClick : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (DialogManager.Instance.IsAutoPlay || DialogManager.Instance.IsExplore || DialogManager.Instance.IsVideo) return;
        DialogManager.Instance.Next();
    }
}

