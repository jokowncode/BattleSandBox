
using System;
using TMPro;
using UnityEngine;

public class SkillNameUI : MonoBehaviour{

    private TextMeshProUGUI SkillNameText;

    private void Awake(){
        SkillNameText = GetComponentInChildren<TextMeshProUGUI>();
        Hide();
    }

    public void SetSkillName(string skillName){
        SkillNameText.text = skillName;
    }

    public void Show(){ 
        this.gameObject.SetActive(true);
    }

    public void Hide(){
        this.gameObject.SetActive(false);
    }
}
