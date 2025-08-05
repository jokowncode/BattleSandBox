
using UnityEngine;

public class DefeatState : BattleState {
    
    [SerializeField] private AudioClip DefeatMusic;

    public override void Construct(){
        if (DefeatMusic) {
            AudioManager.Instance.SetMainMusic(this.DefeatMusic);
        }
    }
    
}

