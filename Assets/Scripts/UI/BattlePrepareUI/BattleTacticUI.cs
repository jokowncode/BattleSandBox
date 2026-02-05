
using UnityEngine;

public class BattleTacticUI : MonoBehaviour {

    [SerializeField] private Transform Container;
    [SerializeField] private BattleTacticButton BattleTacticButtonPrefab;
    
    public void Show(string hero1, string hero2) {
        this.gameObject.SetActive(true);
        foreach (Transform child in Container) {
            Destroy(child.gameObject);
        }

        BattleTacticType maxTactic = EntanglementManager.Instance.GetEntangleHeroCanCastMaxBattleTactic(hero1, hero2);
        for (int i = 0; i <= (int) maxTactic; i++) {
            BattleTacticButton tactic = Instantiate(this.BattleTacticButtonPrefab, this.Container);
            tactic.SetContent(EntanglementManager.Instance.AllBattleTacticDescs[i], (BattleTacticType)i);
        }
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

}

