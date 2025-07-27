
using System;
using UnityEngine;

public class BulletSkillDelivery : SkillDelivery {

    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private float HitOffset = 0f;
    [SerializeField] private bool UseFirePointRotation;
    [SerializeField] private Vector3 RotationOffset = new Vector3(0, 0, 0);
    [SerializeField] private GameObject Hit;
    [SerializeField] private GameObject Flash;
    [SerializeField] private GameObject[] Detached;
    
    private Rigidbody SkillRigidbody;

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
        Destroy(gameObject,5);
    }

    private void FixedUpdate() {
        if (this.Speed != 0) {
            SkillRigidbody.velocity = this.Speed * this.MoveVec;
            ApplyMiddlePlugin();
        }
    }

    protected override void CollisionTarget(Collision collision) {
        //Lock all axes movement and rotation
        SkillRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * HitOffset;

        if (Hit != null) {
            var hitInstance = Instantiate(Hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (RotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(RotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) {
                Destroy(hitInstance, hitPs.main.duration);
            } else {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached) {
            if (detachedPrefab != null) {
                detachedPrefab.transform.parent = null;
            }
        }

        if (collision.gameObject.TryGetComponent(out Fighter fighter)) {
            this.Effect.ApplyEffect(fighter, this.EffectData);
        }

        Destroy(gameObject);
    }
}

