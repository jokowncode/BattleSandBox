
using UnityEngine;

public class TransportSkillCaster : SkillCaster {
    
    [SerializeField] private BuffData BuffData;
    
    /*[Header("Trail")]
    [SerializeField] private GameObject SkillTrailPrefab;*/
    
    [Header("Summon")]
    [SerializeField] private int SummonCount = 0;
    [SerializeField] private float SummonDuration = 5.0f;
    [SerializeField] private float SummonAngle = 30.0f;
    
    [Header("material Props")]
    private string propertyName = "_Outer";

    private void Summon(Vector3 centerPos, float angle) {
        Vector3 rotVector = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
        Vector3 summonPos = centerPos + rotVector * OwnedFighter.AttackRadius;
        Fighter summon = Instantiate(OwnedFighter, summonPos, Quaternion.identity);
        summon.SetCurrentData(OwnedFighter.CurrentData);
        summon.BattleStart(true);
        summon.TransitionShow(true);

        // Outer
        SpriteRenderer targetSpriteRenderer = summon.transform.GetComponentInChildren<SpriteRenderer>();
        if (targetSpriteRenderer != null)
        {
            //Material materialInstance = new Material(targetSpriteRenderer.material);
            //targetSpriteRenderer.material = materialInstance;
            Material materialInstance = targetSpriteRenderer.material;
            if (materialInstance.HasProperty(propertyName))
            {
                materialInstance.SetFloat(propertyName, 1.0f);
            }
        }
        
        Destroy(summon.gameObject, this.SummonDuration);
    }

    protected override void Cast(Transform _){
        if (SkillStartParticle){
            ParticleSystem ps = Instantiate(SkillStartParticle, OwnedFighter.transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        Vector3 targetPos = this.OwnedFighter.transform.position;
        if (this.CurrentSkillCastCount <= 1) {
            Fighter fighter = null;
            if (OwnedFighter.AttackTargetType == TargetType.Enemy) {
                fighter = BattleManager.Instance.FindFurthestEnemyTarget(OwnedFighter.transform.position);
            } else {
                fighter = BattleManager.Instance.FindFurthestHeroTarget(OwnedFighter.transform.position);
            }
        
            Vector3 dir = OwnedFighter.AttackTargetType == TargetType.Enemy ? Vector3.right : Vector3.left;
            targetPos = fighter.transform.position + dir * OwnedFighter.AttackRadius;
            OwnedFighter.ChangePositionWithTrail(targetPos);
            
            if (SkillStartParticle){
                ParticleSystem ps = Instantiate(SkillStartParticle, fighter.transform.position, Quaternion.identity);
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
        
        if (SummonCount != 0) {
            for (int i = 0; i < SummonCount / 2; i++) {
                Summon(targetPos, -(i + 1) * SummonAngle);
                Summon(targetPos, (i + 1) * SummonAngle);
            }    
        }
        if(BuffData) BuffManager.Instance.AddBuff(this.OwnedFighter,this.OwnedFighter,BuffData);
    }
}
