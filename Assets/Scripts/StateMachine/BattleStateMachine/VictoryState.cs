
using UnityEngine;

public class VictoryState : BattleState{

    [SerializeField] private Sprite GameVictoryBannarSprite;

    public override void Construct() {
        int count = Controller.HeroesInBattle.Count;
        int randomIndex = Random.Range(0, count);
        Hero hero = Controller.HeroesInBattle[randomIndex];
        HeroAudioData data = hero.WarehouseData.GetHeroAudio(HeroAudioType.胜利);
        if (data != null) {
            AudioManager.Instance.SetDialog(data.Audio);
        }

        BattleUIManager.Instance.GameEnd(this.GameVictoryBannarSprite);
        Controller.ReturnButton.onClick.AddListener(() => {
            AudioManager.Instance.StopDialog();
            this.Controller.AllHeroRecall();
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

