
using System;
using UnityEngine;

public class StoryReviewUI : MonoBehaviour {

    [SerializeField] private StoryDialogHistory StoryDialogPrefab;
    [SerializeField] private Transform StoryDialogContainer;

    private CanvasGroup StoryReviewCanvasGroup;
    private StoryDialogData[] Dialogs;

    private int CurrentShowIndex = 0;
    
    private void Awake() {
        this.StoryReviewCanvasGroup = GetComponent<CanvasGroup>();
        Transition(false);
    }

    private void Transition(bool show) {
        StoryReviewCanvasGroup.alpha = show ? 1.0f : 0.0f;
        StoryReviewCanvasGroup.interactable = show;
        StoryReviewCanvasGroup.blocksRaycasts = show;
    }

    public void SetDialogs(StoryDialogData[] dialogs) {
        this.Dialogs = dialogs;
        this.CurrentShowIndex = 0;
        foreach (Transform child in this.StoryDialogContainer) {
            Destroy(child.gameObject);
        }
    }

    public void ShowDialogReview(int lastIndex) {
        for (int i = this.CurrentShowIndex; i <= lastIndex; i++) {
            StoryDialogHistory history = Instantiate(this.StoryDialogPrefab, this.StoryDialogContainer);
            history.SetDialogData(this.Dialogs[i]);
        }
        this.CurrentShowIndex = lastIndex + 1;
        Transition(true);
    }

    public void HideDialogReview() {
        Transition(false);
    }
}


