using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SingleTargetVFXSkillCaster : SingleTargetSkillCaster {
    
    [SerializeField] private GameObject VFXPrefab;

    protected override void Cast(Transform attackTarget) {
        if (this.VFXPrefab) {
            Instantiate(this.VFXPrefab, this.transform.position, Quaternion.identity);
        }
        base.Cast(attackTarget);
    }
}