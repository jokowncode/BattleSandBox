
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private Image TaskDirImg;
    [SerializeField] private TextMeshProUGUI TaskNameTextUI;

    private Vector3 TaskPos;
    public string TaskName { get; private set; }

    public void SetTask(string desc, string taskName, Vector3 position) {
        this.Description.text = desc;
        this.TaskNameTextUI.text = taskName + ":";
        this.TaskName = taskName;
        this.TaskPos = position;
        
        this.TaskDirImg.gameObject.SetActive(this.TaskPos != Vector3.zero);
    }

    private void Update() {
        if (SaveDataManager.Instance.PlayerInBigMap && this.TaskPos != Vector3.zero) {
            Vector3 playerPos = SaveDataManager.Instance.PlayerInBigMap.transform.position;
            Vector3 dir = this.TaskPos - playerPos;
            dir.z = 0.0f;
            dir = dir.normalized;
            
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
            TaskDirImg.transform.rotation = rot;
        }
    }
}


