
using System;
using UnityEngine;

public class BulletSkillDelivery : SkillDelivery{

    [SerializeField] private int BulletPenetrateCount = 1;
    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private GameObject Hit;
    [SerializeField] private GameObject Flash;
    [SerializeField] private GameObject[] Detached;
    
    private Rigidbody SkillRigidbody;
    private int CurrentBulletPenetrateCount;

    protected override void Awake() {
        base.Awake();
        SkillRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        if (Flash != null) {
            var flashInstance = Instantiate(Flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null) {
                Destroy(flashInstance, flashPs.main.duration);
            } else {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    private void FixedUpdate(){
        if (this.Speed != 0){
            // SkillRigidbody.velocity = this.Speed * this.MoveVec;
            SkillRigidbody.MovePosition(SkillRigidbody.position + this.Speed * Time.fixedDeltaTime * this.MoveVec);
            ApplyMiddlePlugin();
        }
    }

    private void DestroyBullet(){
        foreach (var detachedPrefab in Detached) {
            if (detachedPrefab != null) {
                detachedPrefab.transform.parent = null;
            }
        }
        Destroy(gameObject);  
    }

    protected override void TriggerTargetIn(Collider other){
        if (Hit != null){
            var hitInstance = Instantiate(Hit, transform.position, Quaternion.LookRotation(this.MoveVec));
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) {
                Destroy(hitInstance, hitPs.main.duration);
            } else {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Border")) {
            DestroyBullet();
            return;
        }

        if (other.gameObject.TryGetComponent(out Fighter fighter)) {
            this.Effect.ApplyEffect(fighter, this.EffectData);
        }

        BulletPenetrateCount--;
        if (BulletPenetrateCount <= 0) {
            DestroyBullet();
        }
    }
}

