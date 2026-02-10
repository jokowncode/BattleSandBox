
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI LevelTitleText;
    [SerializeField] private TextMeshProUGUI BattleResultText;
    [SerializeField] private TextMeshProUGUI BattleResultEnglishText;
    [SerializeField] private Button ExitButton;

    [Header("UI Style")] 
    [SerializeField] private Color VictoryColor;
    [SerializeField] private Color DefeatColor;
    
    [Header("Panel")]
    [SerializeField] private BattleEndLeftPanel LeftPanel;
    [SerializeField] private BattleEndRightPanel RightPanel;
    
    public void Show(bool victory) {
        this.gameObject.SetActive(true);
        this.ExitButton.onClick.AddListener(() => {
            AudioManager.Instance.StopDialog();
            GameManager.Instance.GoToMap(true, victory);
        });
        this.BattleResultText.text = victory ? "战斗胜利" : "战斗失败";
        this.BattleResultEnglishText.text = victory ? "Victory" : "Defeat";
        this.BattleResultText.color = victory ? VictoryColor : DefeatColor;
        this.BattleResultEnglishText.color = victory ? VictoryColor : DefeatColor;
        this.LevelTitleText.text = BattleManager.Instance.Data.BattleName;

        this.RightPanel.Show(victory);
        this.LeftPanel.Show(victory);
    }

    public void OpenHeroWarehouse() {
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

}



