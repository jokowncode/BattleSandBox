using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 15f;
    public float hitOffset;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset;
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private EffectData BulletDamageMsg;
    private Vector3 TargetPos;
    private Vector3 MoveVector;

    public void SetDamageMessage(EffectData dm) {
        this.BulletDamageMsg = dm;
    }

    public void SetTargetPos(Vector3 pos) {
        this.TargetPos = pos;
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
        Destroy(this.gameObject, 2.0f);
        // StartCoroutine(BulletMove());
    }

    private void FixedUpdate(){
        if (speed != 0){
            rb.velocity = this.MoveVector * speed;
        }
    }
    
    /*private IEnumerator BulletMove() {
        float distance = Vector3.Distance(transform.position,TargetPos);
        float dur = distance / speed;
        
        Vector3 startPos = transform.position;
        for (float t = 0.0f; t <= dur; t += Time.deltaTime) {
            Vector3 newPos = Vector3.Lerp(startPos, TargetPos, t);
            rb.MovePosition(newPos);               
            yield return null;
        }
        // rb.MovePosition(TargetPos);
        rb.position = TargetPos;
        // TODO: If Enemy Move -> Destroy ?
    }*/

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.layer != LayerMask.NameToLayer(BulletDamageMsg.TargetType.ToString())) return;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null) {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
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
            fighter.BeDamaged(this.BulletDamageMsg);
        }
        Destroy(gameObject);
    }
}
