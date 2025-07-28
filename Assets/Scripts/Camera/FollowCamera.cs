using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [SerializeField] private Transform FollowTarget;
    
    private void LateUpdate() {
        if (!this.FollowTarget) return;
        Vector3 pos = this.transform.position;
        pos.x = this.FollowTarget.position.x;
        this.transform.position = pos;
    }
}