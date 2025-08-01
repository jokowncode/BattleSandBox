
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 15f;
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private EffectData BulletDamageMsg;
    private Vector3 MoveVector;

    public void SetDamageMessage(EffectData dm) {
        this.BulletDamageMsg = dm;
    }

    public void SetMoveVector(Vector3 moveVec){
        this.MoveVector = moveVec;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if (flash != null) {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null) {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    private void FixedUpdate(){
        if (speed != 0){
            rb.MovePosition(rb.position + this.speed * Time.fixedDeltaTime * this.MoveVector);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer(BulletDamageMsg.TargetType.ToString())
            && other.gameObject.layer != LayerMask.NameToLayer("Border")) return;
        
        if (hit != null){
            var hitInstance = Instantiate(hit, transform.position, Quaternion.LookRotation(this.MoveVector));
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) {
                Destroy(hitInstance, hitPs.main.duration);
            } else {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        
        if (other.gameObject.TryGetComponent(out Fighter fighter)) {
            fighter.BeDamaged(this.BulletDamageMsg);
        }
        foreach (var detachedPrefab in Detached) {
            if (detachedPrefab != null) {
                detachedPrefab.transform.parent = null;
            }
        }
        Destroy(gameObject);
    }
}
