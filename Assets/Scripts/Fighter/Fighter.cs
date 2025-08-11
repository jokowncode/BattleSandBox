
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : StateMachineController{

    [SerializeField] private Color InitialColor = Color.green;
    [SerializeField] private Color FinalColor = Color.red;
    
    [SerializeField] protected FighterData InitialData;
    [SerializeField] private Canvas FighterCanvas;
    [SerializeField] private Image BloodBarImage;
    [SerializeField] private Image ShieldBarImage;
    [field: SerializeField] public SkillNameUI SkillNameText{ get; private set; }
    [SerializeField] private ParticleSystem BloodParticle;
    [SerializeField] private ParticleSystem HealParticlePrefab;
    [field: SerializeField] public Transform Center { get; private set; }
    [field: SerializeField] public Transform AttackCaster { get; private set; }
    [SerializeField] private AudioClip BeDamagedSfx;
    
    private FighterData CurrentData;
    public Fighter AttackTarget { get; private set; }
    public SkillCaster FighterSkillCaster { get; private set; }
    public Animator FighterAnimator{ get; private set; }
    public FighterMove Move{ get; private set; }
    public FighterAnimationEvent AnimationEvent { get; private set; }

    private SkillState FighterSkill;
    private PatrolState FighterPatrol;
    
    public float HealMultiplier { get; protected set; } = 1.0f;
    public float ShieldMultiplier{ get; protected set; } = 1.0f;

    private TargetType CurrentFighterType;
    private FighterRenderer Renderer;
    public bool IsDead{ get; private set; }

    public Action OnDead;
    
#if DEBUG_MODE
    public float TotalDamage {get; set;}    
#endif

    protected virtual void Awake(){
        this.FighterSkillCaster = GetComponentInChildren<SkillCaster>();
        this.FighterAnimator = GetComponentInChildren<Animator>();
        this.FighterAnimator.applyRootMotion = false;
        
        this.AnimationEvent = GetComponentInChildren<FighterAnimationEvent>();
        this.Move = GetComponent<FighterMove>();
        this.Renderer = GetComponentInChildren<FighterRenderer>();
        this.FighterPatrol = GetComponent<PatrolState>();
        this.FighterSkill = GetComponent<SkillState>();
        // Clone Fighter Data to Update
        this.CurrentData = Instantiate(this.InitialData);
        this.CurrentFighterType = this.gameObject.layer == LayerMask.NameToLayer("Hero") ? TargetType.Hero : TargetType.Enemy;
        this.BloodBarImage.color = InitialColor;
        this.CurrentData.Shield = 0;
    }

    protected virtual void Start(){
        if (this.FighterSkillCaster){
            this.SkillNameText.SetSkillName(this.FighterSkillCaster.Data.Name);
        }
    }

    public void BattleStart() {
        // Turn To Patrol State / Skill State
        if (FighterSkillCaster) {
            FighterSkillCaster.BattleStart();
        }

        if (FighterSkillCaster && FighterSkillCaster.CanCastSkill()){
            this.ChangeState(FighterSkill);
        } else{
            this.ChangeState(FighterPatrol);
        }
        if(FighterPatrol) FighterPatrol.OnFindAttackTarget += OnFindAttackTarget;
    }

    private void OnFindAttackTarget(Fighter target){
        if (!target) return; 
        this.AttackTarget = target;
        this.AttackTarget.OnDead += OnTargetDead;
    }

    private void OnTargetDead(){
        this.ChangeState(FighterPatrol);
    }

    public void FighterIdle(){
        this.FighterAnimator.SetTrigger(AnimationParams.Idle);
        this.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
        this.ChangeState(null);
    }

    public void BeDamaged(EffectData effectData){
        if (IsDead) return;

        float finalHealthValue = effectData.Value;
        if (Shield > 0) {
            finalHealthValue = Mathf.Max(0,effectData.Value - Shield);
            Shield = Mathf.Max(0,Shield - effectData.Value);
            if (Shield <= 0){
                this.GetComponent<Buff>().DestroyShieldParticles();
            }
        }
        this.CurrentData.Health = Mathf.Max(0.0f, this.CurrentData.Health - finalHealthValue);
        this.BloodBarImage.fillAmount = this.CurrentData.Health / this.InitialData.Health;
        this.BloodBarImage.color = Color.Lerp(this.InitialColor, this.FinalColor, 1.0f - this.BloodBarImage.fillAmount);
        
        
        if(this.ShieldBarImage != null && this.InitialData.Shield != 0.0f)
            this.ShieldBarImage.fillAmount = this.CurrentData.Shield / this.InitialData.Shield;
        
        if(this.BloodParticle) this.BloodParticle.Play();

        if (this.CurrentFighterType == TargetType.Enemy) {
            this.Renderer.ChangeColor(Color.red);    
        } else{
            this.Renderer.Flash();
#if DEBUG_MODE
            Debug.Log($"{this.gameObject.name} Be Damaged : {effectData.Value}, Current Health: {this.CurrentData.Health}");
#endif
        }

        if (this.BeDamagedSfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.BeDamagedSfx);
        }
        
        if (this.CurrentData.Health <= 0.0f && !IsDead) {
            IsDead = true;
            OnDead?.Invoke();
            this.Renderer.Dead();
            this.FighterCanvas.gameObject.SetActive(false);
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            this.Move.StopMove();
            this.FighterIdle();
            
#if DEBUG_MODE
        if (this.CurrentFighterType == TargetType.Hero) {
            Debug.Log($"{this.gameObject.name} Dead -> Caused Total Damage: {this.TotalDamage}");    
        }    
#endif
            if (this is Hero hero){
                BattleUIManager.Instance.heroPortraitUI.SetHeroPortraitsGray(hero);
                BattleManager.Instance.RemoveHero(hero);
            }else if (this is Enemy enemy) {
                BattleManager.Instance.RemoveEnemy(enemy);
            }
        }
    }
    
    public void BeHealed(EffectData effectData) {
        if (this.HealParticlePrefab){
            ParticleSystem healParticle = Instantiate(this.HealParticlePrefab, this.transform.position, Quaternion.identity);
            Destroy(healParticle.gameObject, 0.7f);
        }
        this.CurrentData.Health = Mathf.Min(this.InitialData.Health, this.CurrentData.Health + effectData.Value);
        this.BloodBarImage.fillAmount = this.CurrentData.Health / this.InitialData.Health;
    }
    
    public void FighterPropertyChange(FighterProperty property, PropertyModifyWay modifyWay, float value, bool isUp){

        float sign = isUp ? 1.0f : -1.0f;
        // TODO: Change Speed
        if (property == FighterProperty.Speed) {
            if(value < 0.0f) this.Move.StopMove();
            else this.Move.StartMove();
            //Debug.Log("Speed"+value);
            return;
        }
        
        
        if (property == FighterProperty.HealMultiplier) {
            float percentage = value / 100.0f;
            this.HealMultiplier += sign * percentage;
            return;
        }
        
        if (property == FighterProperty.ShieldMultiplier) {
            float percentage = value / 100.0f;
            this.ShieldMultiplier += sign * percentage;
            return;
        }
        
        if (property == FighterProperty.CooldownPercentage){
            float currentMultiplier = FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
            float percentage = value / 100.0f;
            FighterAnimator.SetFloat(AnimationParams.AttackAnimSpeedMultiplier, currentMultiplier + sign * percentage);
            return;
        }

        string propertyName = property.ToString();
        float currentValue = ReflectionTools.GetObjectProperty<float>(propertyName, this);
        switch (modifyWay){
            case PropertyModifyWay.Value:
                currentValue += sign * value;
                break;
            case PropertyModifyWay.Percentage:
                float percentage = value / 100.0f;
                float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+propertyName, this);
                currentValue += sign * initialValue * percentage;
                break;
        }
        ReflectionTools.SetObjectProperty(propertyName, this, currentValue);
        // Debug.Log(propertyName + currentValue);
    }
    
    public float GetPropertyData(FighterProperty property){
        if (property == FighterProperty.HealMultiplier) {
            return this.HealMultiplier;
        }

        if (property == FighterProperty.ShieldMultiplier) {
            return this.ShieldMultiplier;
        }

        if (property == FighterProperty.CooldownPercentage){
            return FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
        }

        string propertyName = property.ToString();
        return ReflectionTools.GetObjectProperty<float>("Initial"+propertyName, this);
    }

    public void UpdateShieldAmount()
    {
        this.ShieldBarImage.fillAmount = this.CurrentData.Shield / this.InitialData.Shield;
        // Debug.Log(this.ShieldBarImage.fillAmount);
        // Debug.Log(this.CurrentData.Shield);
    }
    
    public void UpdateBloodAmount()
    {
        this.BloodBarImage.fillAmount = this.CurrentData.Health / this.InitialData.Health;
        // Debug.Log(this.BloodBarImage.fillAmount);
        // Debug.Log(this.CurrentData.Health);
    }

    #region FighterProperty
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
    public float InitialCritical{ 
        get => InitialData.Critical;
        set => InitialData.Critical=value;
    }
    public float InitialForce{ 
        get => InitialData.Force;
        set => InitialData.Force=value;
    }

    public float InitialShield{
        get => InitialData.Shield;
        set => InitialData.Shield=value;
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
    public float Critical{ 
        get => CurrentData.Critical;
        set => CurrentData.Critical=value;
    }
    public float Force{ 
        get => CurrentData.Force;
        set => CurrentData.Force=value;
    }

    public float Shield{
        get => CurrentData.Shield;
        set => CurrentData.Shield=value;
    }
    public TargetType AttackTargetType => InitialData.AttackTargetType;

    public FighterType Type => InitialData.Type;
    public string Name => InitialData.Name;
    public string Description => InitialData.Description;
    public int StarLevel => InitialData.StarLevel;
    public float AttackRadius => InitialData.AttackRadius;
    public float Speed => InitialData.Speed;
    public Sprite StandingSprite => InitialData.standingSprite;
    public Sprite heroPortraitSprite => InitialData.heroPortraitSprite;
    
    #endregion
}

