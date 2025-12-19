
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : StateMachineController{

    [SerializeField] private Color InitialColor = Color.green;
    [SerializeField] private Color FinalColor = Color.red;

    [field: SerializeField] public FighterData InitialData { get; protected set; }
    [SerializeField] protected Canvas FighterCanvas;
    [SerializeField] private Image BloodBarImage;
    [SerializeField] private Image ShieldBarImage;
    [field: SerializeField] public SkillNameUI SkillNameText{ get; private set; }
    [SerializeField] private ParticleSystem BloodParticle;
    [SerializeField] private PoolGO HealParticlePrefab;
    [SerializeField] private DamageUI DamageUIPrefab;
    [field: SerializeField] public Transform Center { get; private set; }
    [field: SerializeField] public Transform AttackCaster { get; private set; }
    [SerializeField] private AudioClip BeDamagedSfx;
    
    protected FighterData CurrentData;
    public Fighter AttackTarget { get; private set; }
    public SkillCaster OriginFighterSkillCaster { get; protected set; }
    public SkillCaster FighterSkillCaster { get; protected set; }

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

    public Action<Fighter> OnDead;
    public Action OnDisappear;

    private float InBattleHealth;
    private float InBattleShield;

    private bool IsBattleStart;
    public bool IsDisappear { get; protected set; } = false;
    
    public float HealthPercentage => this.CurrentData.Health == 0.0f ? 0.0f : this.InBattleHealth / this.CurrentData.Health;

#if DEBUG_MODE
    public float TotalDamage {get; set;}    
#endif

    protected virtual void Awake(){
        GetRendererComponent();
        this.OriginFighterSkillCaster = GetComponentInChildren<SkillCaster>();
        this.FighterSkillCaster = this.OriginFighterSkillCaster;
        
        this.Move = GetComponent<FighterMove>();
        this.FighterPatrol = GetComponent<PatrolState>();
        this.FighterSkill = GetComponent<SkillState>();
        // Clone Fighter Data to Update
        if(this.InitialData) this.CurrentData = Instantiate(this.InitialData);
        this.CurrentFighterType = this.gameObject.layer == LayerMask.NameToLayer("Hero") ? TargetType.Hero : TargetType.Enemy;
        this.BloodBarImage.color = InitialColor;
    }

    public void GetRendererComponent() {
        this.FighterAnimator = GetComponentInChildren<Animator>(false);
        this.FighterAnimator.applyRootMotion = false;
        this.AnimationEvent = GetComponentInChildren<FighterAnimationEvent>(false);
        this.Renderer = GetComponentInChildren<FighterRenderer>(false);
    }

    public void ResetCurrentState() {
        ChangeState(this.CurrentState);
    }

    protected virtual void Start(){
        if (this.FighterSkillCaster){
            this.SkillNameText.SetSkillName(this.FighterSkillCaster.Data.Name);
        }
    }

    public void BattleStart(bool isSummon = false) {
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
        if (SkillNameText) SkillNameText.Hide(true);

        if (!IsBattleStart || isSummon) {
            IsBattleStart = true;
            this.InBattleHealth = this.CurrentData.Health;
            this.InBattleShield = this.CurrentData.Shield;
            UpdateBloodBar();
            UpdateShieldBar();
        }
    }

    private void OnFindAttackTarget(Fighter target){
        if (!target) return; 
        this.AttackTarget = target;
        this.AttackTarget.OnDead += OnTargetDead;
        this.AttackTarget.OnDisappear += OnTargetDisappear;
    }

    private void OnTargetDisappear() {
        if(this.AttackTarget) this.AttackTarget.OnDisappear -= OnTargetDisappear;
        this.AttackTarget = null;
        if (!this.IsDisappear) {
            this.ChangeState(FighterPatrol);
        }
    }

    private void OnTargetDead(Fighter fighter) {
        this.AttackTarget = null;
        if (!this.IsDisappear) {
            this.ChangeState(FighterPatrol);
        }
    }
    
    private void ShowDamage(float damage, bool isCritical){
        if (!DamageUIPrefab) return; 
        DamageUI ui = Instantiate(DamageUIPrefab, FighterCanvas.transform);
        RectTransform rectTrans = (RectTransform)ui.transform;
        Vector3 anchoredPos = rectTrans.anchoredPosition;
        anchoredPos += Center.localPosition;
        rectTrans.anchoredPosition = anchoredPos;
        ui.Show(damage, isCritical);
    }

    public void FighterIdle(){
        this.FighterAnimator.SetTrigger(AnimationParams.Idle);
        this.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
        this.ChangeState(null);
    }

    public void BeDamaged(EffectData effectData){
        if (IsDead) return;

        float finalDamage = effectData.Value;
        if (this.InBattleShield > 0.0f) {
            finalDamage = Mathf.Max(0, effectData.Value - this.InBattleShield);
            this.InBattleShield = Mathf.Max(0, this.InBattleShield - effectData.Value);
            if (this.InBattleShield <= 0){
                foreach (Transform child in this.Center.transform) {
                    if (child.CompareTag("Shield")) {
                        // Destroy(child.gameObject);
                        child.gameObject.SetActive(false);
                    }
                }
            }   
            UpdateShieldBar();
        }

        if (this is Hero hero && hero.ShareDamageHero) {
            finalDamage /= 2;
            hero.ShareDamageHero.BeDamaged(new EffectData() {
                Value = finalDamage
            });
        }

        ShowDamage(finalDamage, effectData.IsCritical);
        
        this.InBattleHealth = Mathf.Min(this.CurrentData.Health, this.InBattleHealth - finalDamage);
        UpdateBloodBar();
        if(this.BloodParticle && !effectData.NotShowParticle) this.BloodParticle.Play();

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
        
        if (this.InBattleHealth <= 0.0f && !IsDead) {
            FighterDead();
        }
    }

    public void FighterDead() {
        IsDead = true;
        OnDead?.Invoke(this);
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

    public void BeHealed(EffectData effectData) {
        if (this.HealParticlePrefab&& !effectData.NotShowParticle) {
            PoolGO go = PoolManager.Instance.GetGameObject(this.HealParticlePrefab);
            go.transform.SetParent(this.transform, false);
            go.transform.localPosition = Vector3.zero;
            PoolManager.Instance.ReleaseGameObject(go, 0.7f);
        }
        
        this.InBattleHealth = Mathf.Min(this.CurrentData.Health, this.InBattleHealth + effectData.Value);
        UpdateBloodBar();
    }

    private void UpdateBloodBar() {
        this.BloodBarImage.fillAmount = this.CurrentData.Health == 0.0f ? 0.0f : this.InBattleHealth / this.CurrentData.Health;
        this.BloodBarImage.color = Color.Lerp(this.InitialColor, this.FinalColor, 1.0f - this.BloodBarImage.fillAmount);
    }

    private void UpdateShieldBar() {
        if (!this.ShieldBarImage) return;
        this.ShieldBarImage.fillAmount = this.CurrentData.Shield == 0.0f ? 0.0f : this.InBattleShield / this.CurrentData.Shield;
    }

    public float FighterPropertyChange(FighterProperty updateProperty, FighterProperty refProperty,
        PropertyModifyWay modifyWay, PropertyRef propertyRef, float value, bool isUp, 
        Fighter refFighter = null){

        float sign = isUp ? 1.0f : -1.0f;
        // TODO: Change Speed
        if (updateProperty == FighterProperty.Speed) {
            if(sign * value < 0.0f) this.Move.StopMove();
            else this.Move.StartMove();
            //Debug.Log("Speed"+value);
            return sign * value;
        }
        
        if (updateProperty == FighterProperty.HealMultiplier) {
            float change = sign * value;
            if (modifyWay == PropertyModifyWay.Percentage) {
                change = sign * value / 100.0f;
            }
            this.HealMultiplier += change;
            return change;
        }
        
        if (updateProperty == FighterProperty.ShieldMultiplier) {
            float change = sign * value;
            if (modifyWay == PropertyModifyWay.Percentage) {
                change = sign * value / 100.0f;
            }
            this.ShieldMultiplier += change;
            return change;
        }
        
        if (updateProperty == FighterProperty.CooldownPercentage){
            float currentMultiplier = FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
            float change = sign * value;
            if (modifyWay == PropertyModifyWay.Percentage) {
                change = sign * value / 100.0f;
            }
            FighterAnimator.SetFloat(AnimationParams.AttackAnimSpeedMultiplier, currentMultiplier + change);
            return change;
        }

        string propertyName = updateProperty.ToString();
        float currentValue = ReflectionTools.GetObjectProperty<float>(propertyName, this);
        float changeValue =  GetPropertyChangeValue(refProperty, modifyWay, propertyRef, value, isUp, refFighter);
        if (updateProperty == FighterProperty.Shield) {
            changeValue *= this.ShieldMultiplier;
        }
        float increasePercentage = currentValue == 0.0f ? 0.0f : changeValue / currentValue;

        float finalValue = currentValue + changeValue;
        ReflectionTools.SetObjectProperty(propertyName, this, finalValue);
        if (updateProperty == FighterProperty.Shield && IsBattleStart) {
            if (finalValue <= 0.0f) {
                this.InBattleShield = 0.0f;
            } else {
                this.InBattleShield = currentValue != 0.0f && this.InBattleShield != 0.0f? 
                    this.InBattleShield + this.InBattleShield * increasePercentage : changeValue;
            }
            this.InBattleShield = Mathf.Max(this.InBattleShield, 0.0f);
            UpdateShieldBar();
        }

        if (updateProperty == FighterProperty.Health && IsBattleStart) {
            if (finalValue <= 0.0f) {
                this.InBattleHealth = 0.0f;
            } else {
                this.InBattleHealth = currentValue != 0.0f ? 
                    this.InBattleHealth + this.InBattleHealth * increasePercentage : changeValue;
            }
            UpdateBloodBar();
        }
        return changeValue;
    }

    public float GetPropertyChangeValue(FighterProperty refProperty, PropertyModifyWay modifyWay, PropertyRef propertyRef, float value, bool isUp, Fighter refFighter) {
        float sign = isUp ? 1.0f : -1.0f;
        switch (modifyWay){
            case PropertyModifyWay.Value:
                return sign * value;
            case PropertyModifyWay.Percentage:
                float percentage = value / 100.0f;
                
                Fighter reference = refFighter ? refFighter : this;
                string refPropertyName = refProperty.ToString();
                string finalName = propertyRef == PropertyRef.Initial ? "Initial" + refPropertyName : refPropertyName;
                float initialValue = ReflectionTools.GetObjectProperty<float>(finalName, reference);
                return sign * initialValue * percentage;
        }
        return -1;
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
    
    public float InitialAttack {
        get => Type == FighterType.Warrior ? InitialData.PhysicsAttack : InitialData.MagicAttack;
        set {
            if (Type == FighterType.Warrior) InitialData.PhysicsAttack = value;
            else InitialData.MagicAttack = value;
        }
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

    public float Attack {
        get => Type == FighterType.Warrior ? CurrentData.PhysicsAttack : CurrentData.MagicAttack;
        set {
            if (Type == FighterType.Warrior) CurrentData.PhysicsAttack = value;
            else CurrentData.MagicAttack = value;
        }
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
    public Sprite DetailPortrait => InitialData.DetailPortrait;
    public Sprite WarehouseHeroPortrait => InitialData.WarehouseHeroPortrait;
    
    #endregion
}

