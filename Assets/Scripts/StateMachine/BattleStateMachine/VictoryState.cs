
using UnityEngine;

public class VictoryState : BattleState{

    [SerializeField] private AudioClip[] VictoryMusics;
    [SerializeField] private Sprite GameVictoryBannarSprite;

    public override void Construct(){
        if (VictoryMusics.Length != 0) AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.VictoryMusics[Random.Range(0, this.VictoryMusics.Length)]);
        BattleUIManager.Instance.GameEnd(this.GameVictoryBannarSprite);
        Controller.ReturnButton.onClick.AddListener(() => {
            GameManager.Instance.GoToMap(true, true);
        });
        
#if DEBUG_MODE
        float duration = Time.time - Controller.BattleStartTime;
        Debug.Log($"Battle Duration : {duration}");
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle){
            Debug.Log($"{hero.gameObject.name} Survive -> Caused Total Damage: {hero.TotalDamage}, DPS: {hero.TotalDamage / duration}");
        }
#endif
    }
}

