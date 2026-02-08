
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI TaskNameTextUI;
    
    private Vector3 TaskPos;
    public string TaskName { get; private set; }

    public bool HasTaskPos => this.TaskPos != Vector3.zero;

    public void SetTask(string desc, string taskName, Vector3 position) {
        this.Description.text = desc;
        this.TaskNameTextUI.text = taskName + ":";
        this.TaskName = taskName;
        this.TaskPos = position;
    }

    public Vector2 GetTaskRotation() {
        if (!SaveDataManager.Instance.PlayerInBigMap || this.TaskPos == Vector3.zero) return Vector2.zero;
        Vector3 playerPos = SaveDataManager.Instance.PlayerInBigMap.transform.position;
        Vector3 dir = this.TaskPos - playerPos;
        dir.z = 0.0f;
        dir = dir.normalized;

        float x = Mathf.Abs(dir.x) <= 0.6 ? 0.0f : Mathf.Sign(dir.x);
        float y = Mathf.Abs(dir.y) <= 0.6 ? 0.0f : Mathf.Sign(dir.y);
        return new Vector2(x, y);
    }
}


