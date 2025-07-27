
using System;
using UnityEngine;

public abstract class SkillDelivery : MonoBehaviour {

    protected SkillEffectData SkillEffect;
    protected Transform Target;
    protected SkillEffect Effect; 
    
    public void StartDelivery(Transform target, SkillEffectData skillEffect) {
        this.Target = target;
        this.SkillEffect = skillEffect;
    }

    protected virtual void Awake() {
        Effect = GetComponent<SkillEffect>();
    }

    private void Update() {
        Delivery();
    }

    protected abstract void Delivery();

    protected virtual void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag(this.SkillEffect.TargetType.ToString())) {
            Effect.ApplyEffect(other.gameObject.GetComponent<Fighter>(), this.SkillEffect);
        }
    }
}
