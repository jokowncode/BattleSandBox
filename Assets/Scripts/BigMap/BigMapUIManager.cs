
using System;
using UnityEngine;

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private CanvasGroup StoreCanvasGroup;
    
    public static BigMapUIManager Instance;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void HideCanvasGroup(CanvasGroup group){
        group.alpha = 0.0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    private void ShowCanvasGroup(CanvasGroup group){
        group.alpha = 1.0f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void TransitionStore(bool isShow){
        if (isShow){
            ShowCanvasGroup(StoreCanvasGroup);
        } else {
            HideCanvasGroup(StoreCanvasGroup);
        }
    }
}

