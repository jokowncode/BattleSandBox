
using UnityEngine;
using UnityEngine.Rendering;

public class DefeatState : BattleState {
    
    [SerializeField] private VFXBase GameDefeatVFX;

    public override void Construct(){
        BattleUIManager.Instance.GameEnd(false);
        GameDefeatVFX?.StartVFX();

#if DEBUG_MODE
        Debug.Log($"Battle Duration : {Time.time - Controller.BattleStartTime}");
#endif
    }
    
}

