
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartUI : MonoBehaviour{

    [SerializeField] private Image Background;
    [SerializeField] private Image BattleImage;
    [SerializeField] private TextMeshProUGUI BattleText;

    private SceneType BattleScene;
    
    public void GoToBattle(){
        SceneChangeManager.Instance.GoToScene(this.BattleScene);
    }

    public void ShowBattleStartUI(SceneType battleScene, Sprite background, Sprite battleImage, string battleText) {
        this.BattleScene = battleScene;
        this.Background.sprite = background;
        this.Background.color = new Color(1, 1, 1, background?1:0);
        
        this.BattleImage.sprite = battleImage;
        this.BattleText.text = battleText;
        this.gameObject.SetActive(true);
    }

}

