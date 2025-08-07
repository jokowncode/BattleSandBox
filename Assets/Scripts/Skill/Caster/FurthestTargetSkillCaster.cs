
using UnityEngine;

[RequireComponent(typeof(Buff))]
public class FurthestTargetSkillCaster : SkillCaster {
    
    [SerializeField] private BuffData BuffData;
    
    protected override void Cast(Transform _){
        if (SkillStartParticle){
            ParticleSystem ps = Instantiate(SkillStartParticle, OwnedFighter.transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        
        Fighter fighter = BattleManager.Instance.FindFurthestEnemyTarget(OwnedFighter.transform.position);
        OwnedFighter.transform.position = fighter.transform.position + Vector3.right * OwnedFighter.AttackRadius;

        if (SkillStartParticle){
            ParticleSystem ps = Instantiate(SkillStartParticle, fighter.transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        
        if(BuffData) this.GetComponent<Buff>().AddBuff(this.OwnedFighter,this.OwnedFighter,BuffData);
    }
}
