
using System;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour{

    private TextMeshProUGUI LevelText;
    private Animator LevelAnimator;

    private void Awake(){
        LevelText = GetComponentInChildren<TextMeshProUGUI>();
        LevelAnimator = GetComponent<Animator>();
    }

    public void ShowLevelText(string level){
        LevelText.text = level;
        LevelAnimator.SetTrigger(AnimationParams.Show);
    }
    
}

