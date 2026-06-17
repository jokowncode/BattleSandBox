
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogOption : MonoBehaviour {

    private TextMeshProUGUI OptionText;
    private Button OptionButton;

    private void Awake() {
        this.OptionText = this.GetComponentInChildren<TextMeshProUGUI>();
        this.OptionButton = this.GetComponent<Button>();
    }

    public void SetOptionData(OptionData data, int index) {
        this.OptionText.text = data.OptionContent;
        this.OptionButton.onClick.AddListener(() => {
            DialogManager.Instance.ClickOption(index, data);
        });    
    }
    
}
