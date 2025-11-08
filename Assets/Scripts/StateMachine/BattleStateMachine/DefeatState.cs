
using UnityEngine;
using UnityEngine.Rendering;

public class DefeatState : BattleState {
    
    [SerializeField] private AudioClip[] DefeatMusics;
    [SerializeField] private Sprite GameDefeatBannarSprite;
    
    [SerializeField] private VFXBase GameDefeatVFX;

    public override void Construct(){
        if (DefeatMusics.Length != 0) AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.DefeatMusics[Random.Range(0, this.DefeatMusics.Length)]);
        BattleUIManager.Instance.GameEnd(this.GameDefeatBannarSprite);
        Controller.ReturnButton.onClick.AddListener(() => {
            GameManager.Instance.GoToMap(true, false);
        });
        GameDefeatVFX?.StartVFX();

#if DEBUG_MODE
        Debug.Log($"Battle Duration : {Time.time - Controller.BattleStartTime}");
#endif
    }
    
}

