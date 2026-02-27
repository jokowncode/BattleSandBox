
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI TaskNameTextUI;

    [SerializeField] private Transform TaskDirs;
    
    private Vector3 TaskPos;
    public string TaskName { get; private set; }

    public bool HasTaskPos => this.TaskPos != Vector3.zero;
    private Vector2 LastDir = Vector2.zero;
    private List<GameObject> TaskDirList = new();

    private void Awake() {
        foreach (Transform child in this.TaskDirs) {
            this.TaskDirList.Add(child.gameObject);
        }
    }
    
    public void SetTask(string desc, string taskName, Vector3 position) {
        this.Description.text = desc;
        this.TaskNameTextUI.text = taskName + ":";
        this.TaskName = taskName;
        this.TaskPos = position;
    }

    private Vector2 GetTaskRotation() {
        if (!SaveDataManager.Instance.PlayerInBigMap || this.TaskPos == Vector3.zero) return Vector2.zero;
        Vector3 playerPos = SaveDataManager.Instance.PlayerInBigMap.transform.position;
        Vector3 dir = this.TaskPos - playerPos;
        dir.z = 0.0f;
        dir = dir.normalized;

        float x = Mathf.Abs(dir.x) <= 0.6 ? 0.0f : Mathf.Sign(dir.x);
        float y = Mathf.Abs(dir.y) <= 0.6 ? 0.0f : Mathf.Sign(dir.y);
        return new Vector2(x, y);
    }

    private void LateUpdate() {
        Vector2 dir = GetTaskRotation();
        if (this.LastDir == dir) return; 
        foreach (Transform child in this.TaskDirs) {
            child.gameObject.SetActive(false);
        }
        
        if (dir.y > 0.0f) {
            this.TaskDirList[0].SetActive(true);
        }else if (dir.y < 0.0f) {
            this.TaskDirList[1].SetActive(true);
        }else if (dir.x > 0.0f) {
            this.TaskDirList[3].SetActive(true);
        }else if (dir.x < 0.0f) {
            this.TaskDirList[2].SetActive(true);
        }
        this.LastDir = dir;
    }
}


