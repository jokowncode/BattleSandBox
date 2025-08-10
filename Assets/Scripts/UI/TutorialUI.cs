
using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour{

    [SerializeField] private Sprite[] TutorialImages;

    private Image Tutorial;
    private int Index = 0;

    private void Awake(){
        Tutorial = GetComponent<Image>();
        if(TutorialImages.Length != 0) Tutorial.sprite = TutorialImages[Index];
    }

    public void GoBack(){
        GameManager.Instance.GoToMainMenu();
    }

    public void NextPage(){
        if (TutorialImages.Length <= 0) return;
        Index++;
        Tutorial.sprite = TutorialImages[Index % TutorialImages.Length];
    }

    public void PrePage(){
        if (TutorialImages.Length <= 0) return;
        Index--;
        Tutorial.sprite = TutorialImages[Index % TutorialImages.Length];
    }

}

