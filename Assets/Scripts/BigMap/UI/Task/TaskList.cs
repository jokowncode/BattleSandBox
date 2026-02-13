
using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField] private TaskUI TaskUIPrefab;
    [SerializeField] private Transform TaskContainer;
    [SerializeField] private Transform MapTaskDirContainer;

    private List<GameObject> TaskDirList = new();
    private TaskUI CurrentTaskDirUI;
    private Vector2 LastDir = Vector2.zero;
    
    private void Awake() {
        foreach (Transform child in this.MapTaskDirContainer) {
            this.TaskDirList.Add(child.gameObject);
        }
    }

    private void LateUpdate() {
        if (!this.CurrentTaskDirUI) return;
        Vector2 dir = this.CurrentTaskDirUI.GetTaskRotation();
        if (this.LastDir == dir) return; 
        foreach (Transform child in MapTaskDirContainer) {
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

    private void UpdateTaskDir() {
        // TODO: DEFAULT Get First Has Dir Task -> Get Player Set Instruct Task
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI)) {
                if (taskUI.HasTaskPos) {
                    this.CurrentTaskDirUI = taskUI;
                    return;
                }
            }
        }
    }

    public void UpdateTaskUI() {
        int index = 0;
        foreach (KeyValuePair<string, TaskCurrentData> taskIndexMap in TaskManager.Instance.CurrentTaskDataMap) {
            TaskUI taskUI = null;
            if (this.TaskContainer.childCount - 1 < index) {
                taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
            }else {
                this.TaskContainer.GetChild(index).TryGetComponent(out taskUI);
            }

            if (taskUI) {
                TaskData data = TaskManager.Instance.GetTask(taskIndexMap.Key);
                if (data) {
                    taskUI.SetTask(data.TaskDescs[taskIndexMap.Value.Index], taskIndexMap.Key, taskIndexMap.Value.Position);
                }
            }
            index += 1;
        }
        UpdateTaskDir();
    }

    public void UpdateTask(string taskName, string desc, Transform position) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                taskUI.SetTask(desc, taskName, position ? position.position : Vector3.zero);
                break;
            }
        }
        UpdateTaskDir();
    }

    public void AddTask(string taskName, Vector3 position) {
        TaskUI taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
        TaskData data = TaskManager.Instance.GetTask(taskName);
        taskUI.SetTask(data.TaskDescs[0], taskName, position);
        UpdateTaskDir();
    }

    public void RemoveTask(string taskName) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                Destroy(child.gameObject);
                break;
            }
        }
        UpdateTaskDir();
    }
}


