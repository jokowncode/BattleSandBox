
using UnityEngine;

public class VictoryState : BattleState{

    [SerializeField] private AudioClip[] VictoryMusics;

    public override void Construct(){
        if (VictoryMusics.Length == 0) return;
        AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.VictoryMusics[Random.Range(0, this.VictoryMusics.Length)]);
    }
}

