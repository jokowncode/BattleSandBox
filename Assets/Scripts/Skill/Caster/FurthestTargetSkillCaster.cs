
using UnityEngine;

[RequireComponent(typeof(Buff))]
public class FurthestTargetSkillCaster : SkillCaster {
    
    public BuffData BuffData;
    
    protected override void Cast(Transform _) {
        ParticleSystem ps = Instantiate(SkillStartParticle, OwnedFighter.transform.position, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        Fighter fighter = BattleManager.Instance.FindFurthestEnemyTarget(OwnedFighter.transform.position);
        
        OwnedFighter.transform.position = fighter.transform.position + Vector3.right * OwnedFighter.AttackRadius;
        ps = Instantiate(SkillStartParticle, fighter.transform.position, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        // TODO: Buff
        this.GetComponent<Buff>().AddBuff(this.OwnedFighter,this.OwnedFighter,BuffData);
    }
}
