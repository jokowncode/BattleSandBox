
using UnityEngine;

public class VictoryState : BattleState{

    [SerializeField] private AudioClip VictoryMusic;

    public override void Construct(){
        if (VictoryMusic) {
            AudioManager.Instance.SetMainMusic(this.VictoryMusic);
        }
    }
}

