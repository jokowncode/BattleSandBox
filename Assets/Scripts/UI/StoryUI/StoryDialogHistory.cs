
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialogHistory : MonoBehaviour {

    [SerializeField] private Button PlaySoundButton;
    [SerializeField] private TextMeshProUGUI DialogText;
    
    public void SetDialogData(DialogNode data) {
        string prefix = data.CharacterName == "" ? "" : $"{data.CharacterName}：";
        this.DialogText.text = $"{prefix}{data.DialogText}";
        this.PlaySoundButton.gameObject.SetActive(data.DialogAudio);
        if (data.DialogAudio) {
            this.PlaySoundButton.onClick.AddListener(() => {
                AudioManager.Instance.SetDialog(data.DialogAudio);
            });
        }
    }

}

