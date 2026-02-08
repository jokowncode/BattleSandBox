
using UnityEngine;

public class TacticPanelUI : MonoBehaviour {

    [SerializeField] private Transform Container;
    [SerializeField] private HeroTacticUI BattleTacticButtonPrefab;

    public void ClearTactic() {
        foreach (Transform child in Container) {
            Destroy(child.gameObject);
        }
    }

    public void Show(string hero1, string hero2, bool canUseTactic) {
        this.gameObject.SetActive(true);
        this.ClearTactic();
        BattleTacticType maxTactic = EntanglementManager.Instance.GetEntangleHeroCanCastMaxBattleTactic(hero1, hero2);
        for (int i = 0; i <= (int) maxTactic; i++) {
            HeroTacticUI tactic = Instantiate(this.BattleTacticButtonPrefab, this.Container);
            tactic.SetContent((BattleTacticType)i, canUseTactic);
        }
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

}

