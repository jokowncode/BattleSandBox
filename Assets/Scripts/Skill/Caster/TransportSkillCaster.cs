
using UnityEngine;

public class TransportSkillCaster : SkillCaster {
    
    [SerializeField] private BuffData BuffData;
    
    [Header("Summon")]
    [SerializeField] private int SummonCount = 0;
    [SerializeField] private float SummonDuration = 5.0f;
    [SerializeField] private float SummonAngle = 30.0f;

    private void Summon(float angle) {
        Vector3 rotVector = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
        Vector3 summonPos = OwnedFighter.transform.position + rotVector * OwnedFighter.AttackRadius;
        Fighter summon = Instantiate(OwnedFighter, summonPos, Quaternion.identity);
        summon.FighterSkillCaster.SetSkillCastCount(this.CurrentSkillCastCount);
        summon.BattleStart(true);
        Destroy(summon.gameObject, this.SummonDuration);
    }

    protected override void Cast(Transform _){
        if (SkillStartParticle){
            ParticleSystem ps = Instantiate(SkillStartParticle, OwnedFighter.transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        
        Fighter fighter = BattleManager.Instance.FindFurthestEnemyTarget(OwnedFighter.transform.position);
        OwnedFighter.transform.position = fighter.transform.position + Vector3.right * OwnedFighter.AttackRadius;
        if (SummonCount != 0) {
            for (int i = 0; i < SummonCount / 2; i++) {
                Summon(-(i + 1) * SummonAngle);
                Summon((i + 1) * SummonAngle);
            }    
        }
        
        if (SkillStartParticle){
            ParticleSystem ps = Instantiate(SkillStartParticle, fighter.transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        
        if(BuffData) BuffManager.Instance.AddBuff(this.OwnedFighter,this.OwnedFighter,BuffData);
    }
}
