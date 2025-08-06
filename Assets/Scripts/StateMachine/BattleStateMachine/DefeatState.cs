
using UnityEngine;

public class DefeatState : BattleState {
    
    [SerializeField] private AudioClip[] DefeatMusics;

    public override void Construct(){
        if (DefeatMusics.Length == 0) return; 
        AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.DefeatMusics[Random.Range(0, this.DefeatMusics.Length)]);
    }
    
}

