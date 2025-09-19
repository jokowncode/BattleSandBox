
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialogHistory : MonoBehaviour {

    [SerializeField] private Button PlaySoundButton;
    [SerializeField] private TextMeshProUGUI DialogText;
    
    public void SetDialogData(StoryDialogData data) {
        this.DialogText.text = $"{data.CharacterName}：{data.DialogText}";
        this.PlaySoundButton.gameObject.SetActive(data.DialogAudio);
        if (data.DialogAudio) {
            this.PlaySoundButton.onClick.AddListener(() => {
                AudioManager.Instance.SetDialog(data.DialogAudio);
            });
        }
    }

}

