
using System;
using TMPro;
using UnityEngine;

public class StoryReviewUI : MonoBehaviour {

    [SerializeField] private StoryDialogHistory StoryDialogPrefab;
    [SerializeField] private TextMeshProUGUI DialogOptionPrefab;
    [SerializeField] private Transform StoryDialogContainer;

    private CanvasGroup StoryReviewCanvasGroup;
    
    
    private void Awake() {
        this.StoryReviewCanvasGroup = GetComponent<CanvasGroup>();
        Transition(false);
    }

    public void Transition(bool show) {
        StoryReviewCanvasGroup.alpha = show ? 1.0f : 0.0f;
        StoryReviewCanvasGroup.interactable = show;
        StoryReviewCanvasGroup.blocksRaycasts = show;
    }

    public void Reset() {
        foreach (Transform child in this.StoryDialogContainer) {
            Destroy(child.gameObject);
        }
    }

    public void AddDialogHistory(DialogNode data) {
        StoryDialogHistory history = Instantiate(this.StoryDialogPrefab, this.StoryDialogContainer);
        history.SetDialogData(data);
    }

    public void AddDialogOptionHistory(string optionText) {
        TextMeshProUGUI history = Instantiate(this.DialogOptionPrefab, this.StoryDialogContainer);
        history.text = $"选择“{optionText}”";
    }
}


