
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("Buff")] 
    [SerializeField] private BuffData AttackBuff;
    [SerializeField] private int AttackBuffCount;
    [SerializeField] private BuffData CriticalBuff;
    [SerializeField] private int CriticalBuffCount;
    
    public float speed = 15f;
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private EffectData BulletDamageMsg;
    private Transform Target;
    private Vector3 TargetDir = Vector3.right;

    private bool IsHitTarget = false;
    private bool IsSetDir = false;
    private bool IsStart = true;
    
    private Vector3 MoveVec => Target ? (Target.position - transform.position).normalized : TargetDir.normalized;
    
    public void SetDamageMessage(EffectData dm) {
        this.BulletDamageMsg = dm;
    }

    public void SetTarget(Transform target) {
        IsSetDir = false;
        IsHitTarget = false;
        IsStart = true;
        this.Target = target;
    }

    public void SetTargetDir(Vector3 targetDir) {
        IsSetDir = true;
        IsHitTarget = false;
        IsStart = true;
        this.TargetDir = targetDir;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate(){
        if (!this.IsSetDir && !this.Target){
            this.ReleaseBullet();
            return;
        }

        if (IsStart) {
            IsStart = false;
            if (flash) {
                var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
                flashInstance.transform.forward = gameObject.transform.forward;
                if (flashInstance.TryGetComponent(out ParticleSystem flashPs)) {
                    Destroy(flashInstance, flashPs.main.duration);
                } else if (flashInstance.transform.GetChild(0).TryGetComponent(out ParticleSystem flashPsParts)) {
                    Destroy(flashInstance, flashPsParts.main.duration);
                }
            }
        }

        if (speed != 0){
            rb.MovePosition(rb.position + this.speed * Time.fixedDeltaTime * this.MoveVec);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (IsHitTarget) return;
        if (other.gameObject.layer != LayerMask.NameToLayer(BulletDamageMsg.TargetType.ToString())
            && other.gameObject.layer != LayerMask.NameToLayer("Border")) return;
        
        if (hit != null){
            var hitInstance = Instantiate(hit, transform.position, Quaternion.LookRotation(this.MoveVec));
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) {
                Destroy(hitInstance, hitPs.main.duration);
            } else {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        
        if (other.gameObject.TryGetComponent(out Fighter fighter)){
            IsHitTarget = true;
            fighter.BeDamaged(this.BulletDamageMsg);
            
            if (this.CriticalBuff && this.BulletDamageMsg.IsCritical) {
                BuffManager.Instance.AddBuff(fighter, fighter, this.CriticalBuff, this.CriticalBuffCount);
            } else if (this.AttackBuff) {
                BuffManager.Instance.AddBuff(fighter, fighter, this.AttackBuff, this.AttackBuffCount);
            }
        }
        foreach (var detachedPrefab in Detached) {
            if (detachedPrefab != null) {
                detachedPrefab.transform.parent = null;
            }
        }
        this.ReleaseBullet();
    }

    private void ReleaseBullet() {
        if (this.TryGetComponent(out PoolGO poolGO)) {
            PoolManager.Instance.ReleaseGameObject(poolGO);
        } else {
            Destroy(gameObject); 
        }
    }
}
