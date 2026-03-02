
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleEndUI : MonoBehaviour, IPointerClickHandler {

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

    private bool CurrentIsVictory;
    
    public void Show(bool victory) {
        this.gameObject.SetActive(true);
        this.CurrentIsVictory = victory;
        this.ExitButton.onClick.AddListener(this.ExitBattle);
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

    public void OnPointerClick(PointerEventData eventData) {
        this.ExitBattle();
    }

    private void ExitBattle() {
        AudioManager.Instance.StopDialog();
        GameManager.Instance.BattleEndGoBack(this.CurrentIsVictory);
    }
}



