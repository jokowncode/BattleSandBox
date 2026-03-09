
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartUI : MonoBehaviour{

    [SerializeField] private Image Background;
    [SerializeField] private Image BattleImage;

    private SceneType BattleScene;
    
    public void GoToBattle(){
        SceneChangeManager.Instance.GoToScene(this.BattleScene);
    }

    public void ShowBattleStartUI(SceneType battleScene, Sprite background, Sprite battleImage, string battleText) {
        this.BattleScene = battleScene;
        this.Background.sprite = background;
        this.Background.color = new Color(1, 1, 1, background?1:0);
        
        this.BattleImage.sprite = battleImage;
        this.BattleImage.color = new Color(1, 1, 1, battleImage?1:0);
        this.gameObject.SetActive(true);
    }

}

