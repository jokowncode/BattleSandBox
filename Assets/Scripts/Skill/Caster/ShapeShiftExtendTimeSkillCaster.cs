
using UnityEngine;

public enum ExtendTimeWay {
    KillOther
}

public class ShapeShiftExtendTimeSkillCaster : ShapeShiftSkillCaster {
    
    [SerializeField] private ExtendTimeWay ExtendTimeWay;
    [SerializeField] private float ExtendTime = 2.0f;

    protected override void BeforeShapeShift() {
        if(this.ExtendTimeWay == ExtendTimeWay.KillOther) this.OwnedFighter.OnKillOther += OnFighterKillOther;
    }

    private void OnFighterKillOther() {
        this.ExtendShapeShiftTime(this.ExtendTime);
    }

    protected override void AfterShapeShift() {
        if(this.ExtendTimeWay == ExtendTimeWay.KillOther) this.OwnedFighter.OnKillOther -= OnFighterKillOther;
    }

}
