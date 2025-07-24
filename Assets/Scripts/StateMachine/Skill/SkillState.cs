
using UnityEngine;

public class SkillState : State{

    private SkillData Data;
    private Transform AttackTarget;

    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }

    public void SetCastSkill(int index){
        // this.Data = Controller.Data.PetSkills[index];
    }

    private void CastSkill(Vector3 position){
        // TODO: Cast Skill
        /*SkillParticle skillParticlePrefab = this.Data.skillParticlePrefab;
        SkillParticle skillParticle = Instantiate(skillParticlePrefab, position, Quaternion.identity);
        skillParticle.Play(new DamageMessage {
            Damage = this.Data.Damage,
        });
        GameManager.Instance.ShakeCamera(0.2f, 0.3f, 0.2f);*/
    }
    
    public override void Construct(){
        /*this.AttackTarget = Controller.AttackTarget.transform;
        if (!this.AttackTarget) return;
        
        if (this.Data.IsAttackEveryEnemy){
            // TODO: get better
            Enemy[] enemies = GameManager.Instance.Enemies;
            foreach (Enemy enemy in enemies){
                CastSkill(enemy.transform.position);
            }
        }else if (this.Data.IsCastInEnemyLocation) {
            CastSkill(this.AttackTarget.position);
        }*/
    }

    public override void Transition(){
        // TODO: After Cast Skill -> Change To Attack State
        // Controller.ChangeState(FighterAttack);
    }
}

