
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartUI : MonoBehaviour{

    [SerializeField] private Image BattleImage;
    [SerializeField] private TextMeshProUGUI BattleText;

    public void GoToBattle(){
        SceneChangeManager.Instance.GoToScene(SceneType.Battle);
    }

    public void ShowBattleStartUI(Sprite battleImage, string battleText){
        this.BattleImage.sprite = battleImage;
        this.BattleText.text = battleText;
        this.gameObject.SetActive(true);
    }

}

