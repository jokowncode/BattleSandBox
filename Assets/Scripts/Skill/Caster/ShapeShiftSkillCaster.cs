
using UnityEngine;

public class ShapeShiftSkillCaster : SkillCaster {

    [Header("Shape Shift")] 
    [SerializeField] private GameObject BeforeShapeShiftRenderer;
    [SerializeField] private GameObject AfterShapeShiftRenderer;
    
    [Header("Buff")]
    [SerializeField] private float Duration = 10.0f;
    [SerializeField] private BuffData ShapeShiftBuff;
    
    [Header("Prefab Settings")]
    [SerializeField] private GameObject prefabToCreate;  // 要创建的Prefab
    [SerializeField] private Transform targetChild;     // 目标子物体的名称

    private BuffData CurrentBuffData;
    private bool IsShapeShift;
    
    protected override void Awake() {
        base.Awake();
        if (ShapeShiftBuff) {
            this.CurrentBuffData = Instantiate(ShapeShiftBuff);
            this.CurrentBuffData.Duration = this.Duration;
        }
    }

    protected override void Cast(Transform attackTarget) {
        IsShapeShift = true;
        BeforeShapeShiftRenderer.SetActive(false);
        AfterShapeShiftRenderer.SetActive(true);
        this.OwnedFighter.GetRendererComponent();
        this.OwnedFighter.AnimationEvent.SkillEnd();
        if(ShapeShiftBuff) BuffManager.Instance.AddBuff(this.OwnedFighter, this.OwnedFighter, this.CurrentBuffData);
        float delay = ShapeShiftBuff ? 0.2f : 0.0f;
        CreatePrefabOnTargetChild();
        Invoke(nameof(Recover), this.Duration + delay);
    }

    private void Recover() {
        IsShapeShift = false;
        AfterShapeShiftRenderer.SetActive(false);
        BeforeShapeShiftRenderer.SetActive(true);
        this.OwnedFighter.GetRendererComponent();
        this.OwnedFighter.ResetCurrentState();
    }

    public override bool CanCastSkill() {
        return base.CanCastSkill() && !IsShapeShift;
    }
    
    public void CreatePrefabOnTargetChild()
    {
        if (prefabToCreate == null || targetChild == null)
            return;
        
        GameObject instantiatedPrefab = Instantiate(prefabToCreate, targetChild);
        instantiatedPrefab.transform.localPosition = Vector3.zero;
        instantiatedPrefab.transform.rotation = prefabToCreate.transform.rotation;
    }
}

