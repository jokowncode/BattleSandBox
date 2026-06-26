
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShiftSkillCaster : SkillCaster {

    [Header("Shape Shift")] 
    [SerializeField] private GameObject BeforeShapeShiftRenderer;
    [SerializeField] private GameObject AfterShapeShiftRenderer;
    
    [Header("Buff")]
    [SerializeField] private float Duration = 10.0f;
    [SerializeField] private BuffData ShapeShiftBuff;

    private bool IsShapeShift;
    private float RemainTime;
    private BuffData CurrentShapeShiftBuffData;

    protected override void Cast(Transform attackTarget) {
        if (IsShapeShift) return ;
        StopAllCoroutines();
        StartCoroutine(ShapeShiftCoroutine());
    }

    private IEnumerator ShapeShiftCoroutine() {
        IsShapeShift = true;
        BeforeShapeShiftRenderer.SetActive(false);
        AfterShapeShiftRenderer.SetActive(true);
        this.OwnedFighter.GetRendererComponent();
        this.OwnedFighter.AnimationEvent.SkillEnd();
        if(ShapeShiftBuff) {
            this.CurrentShapeShiftBuffData = Instantiate(ShapeShiftBuff);
            this.CurrentShapeShiftBuffData.Duration = this.Duration;
            BuffManager.Instance.AddBuff(this.OwnedFighter, this.OwnedFighter, this.CurrentShapeShiftBuffData);
        }
        this.BeforeShapeShift();

        this.RemainTime = this.Duration + (this.ShapeShiftBuff ? 0.2f : 0.0f);
        while (this.RemainTime > 0.0f) {
            yield return null;    
            this.RemainTime -= Time.deltaTime;
        }

        this.AfterShapeShift();
        AfterShapeShiftRenderer.SetActive(false);
        BeforeShapeShiftRenderer.SetActive(true);
        this.OwnedFighter.GetRendererComponent();
        this.OwnedFighter.ResetCurrentState();
        IsShapeShift = false;
    }

    protected virtual void BeforeShapeShift(){}
    protected virtual void AfterShapeShift(){}

    protected void ExtendShapeShiftTime(float extendTime) {
        if (!this.IsShapeShift) return ;
        extendTime = Mathf.Max(extendTime, 0.0f);
        this.RemainTime += extendTime;
        if(this.CurrentShapeShiftBuffData) this.CurrentShapeShiftBuffData.Duration += extendTime;
    }

    public override bool CanCastSkill() {
        return base.CanCastSkill() && !IsShapeShift;
    }
}

