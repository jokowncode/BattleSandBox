
using System;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public static TaskManager Instance;

    [SerializeField] private TaskData[] GameTasks;

    private int CurrentTaskIndex = 0;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // SaveMapManager.Instance.OnLoadMap += OnLoadMap;
    }

    private void Start() {
        this.CurrentTaskIndex = SaveMapManager.Instance.CurrentTaskIndex;
        this.SetTask();
    }

    private void OnLoadMap() {
        // SaveMapManager.Instance.OnLoadMap -= OnLoadMap;
    }

    public void NextTask() {
        this.CurrentTaskIndex++;
        SaveMapManager.Instance.CurrentTaskIndex = this.CurrentTaskIndex;
        this.SetTask();
    }

    private void SetTask() {
        if (this.CurrentTaskIndex < this.GameTasks.Length) {
            BigMapUIManager.Instance.TaskUI.SetTask(this.GameTasks[this.CurrentTaskIndex]);
        } else {
            BigMapUIManager.Instance.TaskUI.gameObject.SetActive(false);
        }
    }
}

