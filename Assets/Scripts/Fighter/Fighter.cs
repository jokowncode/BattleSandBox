
using System;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : StateMachineController {

    [SerializeField] protected FighterData InitialData;
    [SerializeField] private Image BloodBarImage;
    [field: SerializeField] public Transform Center { get; private set; }
    [field: SerializeField] public Transform AttackCaster { get; private set; }

    private FighterData CurrentData;
    public Fighter AttackTarget { get; private set; }
    public SkillCaster FighterSkillCaster { get; private set; }
    public Animator FighterAnimator{ get; private set; }
    public FighterMove Move{ get; private set; }
    public FighterAnimationEvent AnimationEvent { get; private set; }

    private SkillState FighterSkill;
    private PatrolState FighterPatrol;

    protected virtual void Awake(){
        this.FighterSkillCaster = GetComponentInChildren<SkillCaster>();
        this.FighterAnimator = GetComponentInChildren<Animator>();
        this.AnimationEvent = GetComponentInChildren<FighterAnimationEvent>();
        this.Move = GetComponent<FighterMove>();
        this.FighterPatrol = GetComponent<PatrolState>();
        this.FighterSkill = GetComponent<SkillState>();
        // Clone Fighter Data to Update
        this.CurrentData = Instantiate(this.InitialData);
    }

    public void BattleStart() {
        // Turn To Patrol State / Skill State
        if (FighterSkillCaster) {
            FighterSkillCaster.BattleStart();
        }

        if (FighterSkillCaster && FighterSkillCaster.CanCastSkill){
            this.ChangeState(FighterSkill);
        } else{
            this.ChangeState(FighterPatrol);
            if(FighterPatrol) FighterPatrol.OnFindAttackTarget += OnFindAttackTarget;
        }
    }

    private void OnFindAttackTarget(Fighter target) {
        this.AttackTarget = target;
    }

    public void BeDamaged(EffectData effectData) {
        this.CurrentData.Health -= effectData.Value;
        this.BloodBarImage.fillAmount = this.CurrentData.Health / this.InitialData.Health;
        // TODO: Play Fighter Be Attacked Anim
        
        if (this.CurrentData.Health <= 0.0f) {
            // TODO: Fighter Dead
            if (this is Hero hero) {
                BattleManager.Instance.RemoveHero(hero);
            }else if (this is Enemy enemy) {
                BattleManager.Instance.RemoveEnemy(enemy);
            }
            Destroy(this.gameObject);
            return;
        }
    }

    public void BeHealed(EffectData effectData) {
        // TODO : Play Fighter Be Healed Anim
        this.CurrentData.Health = Mathf.Min(this.InitialData.Health, this.CurrentData.Health + effectData.Value);
        this.BloodBarImage.fillAmount = this.CurrentData.Health / this.InitialData.Health;
    }

    // Initial Property
    public float InitialHealth{ 
        get => InitialData.Health;
        set => InitialData.Health=value;
    }
    public float InitialPhysicsAttack{ 
        get => InitialData.PhysicsAttack;
        set => InitialData.PhysicsAttack=value;
    }
    public float InitialMagicAttack{ 
        get => InitialData.MagicAttack;
        set => InitialData.MagicAttack=value;
    }
    public float InitialAttackRadius{ 
        get => InitialData.AttackRadius;
        set => InitialData.AttackRadius=value;
    }
    public float InitialCritical{ 
        get => InitialData.Critical;
        set => InitialData.Critical=value;
    }
    public float InitialSpeed{ 
        get => InitialData.Speed;
        set => InitialData.Speed=value;
    }
    public float InitialForce{ 
        get => InitialData.Force;
        set => InitialData.Force=value;
    }
    
    // Runtime Property
    public float Health{ 
        get => CurrentData.Health;
        set => CurrentData.Health=value;
    }
    public float PhysicsAttack{ 
        get => CurrentData.PhysicsAttack;
        set => CurrentData.PhysicsAttack=value;
    }
    public float MagicAttack{ 
        get => CurrentData.MagicAttack;
        set => CurrentData.MagicAttack=value;
    }
    public float AttackRadius{ 
        get => CurrentData.AttackRadius;
        set => CurrentData.AttackRadius=value;
    }
    public float Critical{ 
        get => CurrentData.Critical;
        set => CurrentData.Critical=value;
    }
    public float Speed{ 
        get => CurrentData.Speed;
        set => CurrentData.Speed=value;
    }
    public float Force{ 
        get => CurrentData.Force;
        set => CurrentData.Force=value;
    }
    public TargetType AttackTargetType {
        get => CurrentData.AttackTargetType;
        set => CurrentData.AttackTargetType = value;
    }
    public FighterType Type {
        get => CurrentData.Type;
        set => CurrentData.Type = value;
    }

    public string Name{
        get => InitialData.Name;
        set => InitialData.Name = value;
    }
}

