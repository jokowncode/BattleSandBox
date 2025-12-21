
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private Image TaskDirImg;

    private Vector3 TaskPos;
    
    public void SetTask(TaskData task) {
        this.Description.text = task.TaskDescription;
        this.TaskPos = task.TaskPosition ? task.TaskPosition.position : Vector3.zero;
    }

    private void Update() {
        if (SaveMapManager.Instance.PlayerInBigMap) {
            Vector3 playerPos = SaveMapManager.Instance.PlayerInBigMap.transform.position;
            Vector3 dir = this.TaskPos - playerPos;
            dir.z = 0.0f;
            dir = dir.normalized;
            
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
            TaskDirImg.transform.rotation = rot;
        }
    }
}


