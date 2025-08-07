
using UnityEngine;

public class DefeatState : BattleState {
    
    [SerializeField] private AudioClip[] DefeatMusics;
    [SerializeField] private Sprite GameDefeatBannarSprite;

    public override void Construct(){
        if (DefeatMusics.Length != 0) AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.DefeatMusics[Random.Range(0, this.DefeatMusics.Length)]);
        BattleUIManager.Instance.GameEnd(this.GameDefeatBannarSprite);
        
#if DEBUG_MODE
        Debug.Log($"Battle Duration : {Time.time - Controller.BattleStartTime}");
#endif
    }
    
}

