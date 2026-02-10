
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndRightPanel : MonoBehaviour {

    [SerializeField] private Image HeroImage;
    [SerializeField] private HeroAudioDialog AudioDialog;

    public void Show(bool victory) {
        Hero hero = null;
        if (victory) {
            int count = BattleManager.Instance.HeroesInBattle.Count;
            int randomIndex = Random.Range(0, count);
            hero = BattleManager.Instance.HeroesInBattle[randomIndex];
        } else {
            int count = BattleManager.Instance.BeforeBattleHeroes.Count;
            int randomIndex = Random.Range(0, count);
            hero = BattleManager.Instance.BeforeBattleHeroes[randomIndex]; 
        }

        if (!hero) return;
        this.HeroImage.sprite = hero.WarehouseData.MiddleSpriteAnims[0];
        this.AudioDialog.Show(hero.WarehouseData.GetHeroAudio(victory ? HeroAudioType.胜利 : HeroAudioType.失败));
    }
}




