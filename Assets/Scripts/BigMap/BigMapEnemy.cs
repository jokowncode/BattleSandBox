

using System;
using System.Collections;
using UnityEngine;

public class BigMapEnemy : MonoBehaviour {

    [SerializeField] private float Speed = 2.0f;
    
    private Animator EnemyAnimator;
    private Vector3 StartPos;

    private void Awake() {
        this.StartPos = this.transform.position;
        this.EnemyAnimator = this.GetComponentInChildren<Animator>();
    }

    public void ChasePlayer(Player player, Action onChaseEnd) {
        StopAllCoroutines();
        StartCoroutine(ChaseCoroutine(player, onChaseEnd));
    }

    private IEnumerator ChaseCoroutine(Player player, Action onChaseEnd) {
        this.EnemyAnimator.SetFloat(AnimationParams.Velocity, 1.0f);
        Vector3 diff = player.transform.position - this.transform.position;
        float distance = Vector3.SqrMagnitude(diff);
        float maxDistance = distance + this.Speed;
        
        while (distance > 1.0f && distance < maxDistance) {
            Vector3 dir = diff.normalized;
            this.transform.position += this.Speed * Time.deltaTime * dir;

            float sign = dir.x > 0.0f ? -1.0f : 1.0f; 
            this.transform.localRotation = Quaternion.Euler(0.0f, sign * 90.0f, 0.0f);
            yield return null;
            diff = player.transform.position - this.transform.position;
            distance = Vector3.SqrMagnitude(diff);
        }

        if (distance <= 1.0f) {
            this.EnemyAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            onChaseEnd?.Invoke();
        } else {
            yield return GoBackCoroutine();
        }
    }

    private IEnumerator GoBackCoroutine() {
        Vector3 dir = (this.StartPos - this.transform.position).normalized;
        while (Vector3.SqrMagnitude(this.StartPos - this.transform.position) > 0.5f) {
            this.transform.position += this.Speed * Time.deltaTime * dir;
            float sign = dir.x > 0.0f ? -1.0f : 1.0f; 
            this.transform.localRotation = Quaternion.Euler(0.0f, sign * 90.0f, 0.0f);
            yield return null;
        }
        this.transform.position = this.StartPos;
    }
}


