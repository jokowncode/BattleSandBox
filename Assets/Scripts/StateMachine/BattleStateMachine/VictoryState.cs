
using UnityEngine;

public class VictoryState : BattleState{

    [SerializeField] private AudioClip[] VictoryMusics;
    [SerializeField] private Sprite GameVictoryBannarSprite;

    public override void Construct(){
        if (VictoryMusics.Length != 0) AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.VictoryMusics[Random.Range(0, this.VictoryMusics.Length)]);
        BattleUIManager.Instance.GameEnd(this.GameVictoryBannarSprite);
    }
}

