
using UnityEngine;

public class PrepareState : BattleState{

    [SerializeField] private AudioClip PrepareMusic;
    
    public override void Construct(){
        if(PrepareMusic)
            AudioManager.Instance.SetMainMusic(PrepareMusic);
    }
}
