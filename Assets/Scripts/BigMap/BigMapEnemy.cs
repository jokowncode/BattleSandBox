

using System;
using System.Collections;
using UnityEngine;

public class BigMapEnemy : MonoBehaviour {

    [SerializeField] private float Speed = 2.0f;
    
    private Animator EnemyAnimator;

    private void Awake() {
        this.EnemyAnimator = this.GetComponentInChildren<Animator>();
    }

    public void ChasePlayer(Player player, Action onChaseEnd) {
        StopAllCoroutines();
        StartCoroutine(ChaseCoroutine(player, onChaseEnd));
    }

    private IEnumerator ChaseCoroutine(Player player, Action onChaseEnd) {
        this.EnemyAnimator.SetFloat(AnimationParams.Velocity, 1.0f);
        Vector3 diff = player.transform.position - this.transform.position;
        while (Vector3.SqrMagnitude(diff) > 1.0f) {
            Vector3 dir = diff.normalized;
            this.transform.position += this.Speed * Time.deltaTime * dir;

            float sign = dir.x > 0.0f ? -1.0f : 1.0f; 
            this.transform.localRotation = Quaternion.Euler(0.0f, sign * 90.0f, 0.0f);
            yield return null;
            diff = player.transform.position - this.transform.position;
        }
        this.EnemyAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
        onChaseEnd?.Invoke();
    }
}


