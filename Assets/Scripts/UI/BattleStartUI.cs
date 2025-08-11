
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartUI : MonoBehaviour{

    [SerializeField] private Image Background;
    [SerializeField] private Image BattleImage;
    [SerializeField] private TextMeshProUGUI BattleText;

    public void GoToBattle(){
        SceneChangeManager.Instance.GoToScene(SceneType.Battle);
    }

    public void ShowBattleStartUI(Sprite background, Sprite battleImage, string battleText){
        this.Background.sprite = background;
        this.Background.color = new Color(1, 1, 1, background?1:0);
        
        this.BattleImage.sprite = battleImage;
        this.BattleText.text = battleText;
        this.gameObject.SetActive(true);
    }

}

