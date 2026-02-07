
using UnityEngine;
using UnityEngine.Rendering;

public class DefeatState : BattleState {
    
    [SerializeField] private Sprite GameDefeatBannarSprite;
    [SerializeField] private VFXBase GameDefeatVFX;

    public override void Construct(){
        if (Controller.DefeatHeroAudio) {
            AudioManager.Instance.SetDialog(Controller.DefeatHeroAudio);
        }
        BattleUIManager.Instance.GameEnd(this.GameDefeatBannarSprite);
        Controller.ReturnButton.onClick.AddListener(() => {
            AudioManager.Instance.StopDialog();
            this.Controller.AllHeroRecall();
            GameManager.Instance.GoToMap(true, false);
        });
        GameDefeatVFX?.StartVFX();

#if DEBUG_MODE
        Debug.Log($"Battle Duration : {Time.time - Controller.BattleStartTime}");
#endif
    }
    
}

