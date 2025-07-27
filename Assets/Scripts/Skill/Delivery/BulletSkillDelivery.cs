
using UnityEngine;

public class BulletSkillDelivery : SkillDelivery {

    [SerializeField] private float Speed = 5.0f;
    
    private Rigidbody SkillRigidbody;

    protected override void Awake() {
        base.Awake();
        SkillRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Delivery() {
        SkillRigidbody.velocity = this.Speed * Time.deltaTime * this.transform.forward;
    }
}

