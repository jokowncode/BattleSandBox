
using System;
using UnityEngine;

public class FollowParentRotation : MonoBehaviour {
    private void LateUpdate() {
        if (!this.transform.parent) return;
        Transform parentTrans = this.transform.parent;
        Vector3 euler = parentTrans.rotation.eulerAngles;
        euler.x = -euler.x;
        this.transform.localRotation = Quaternion.Euler(euler);
    }
}

