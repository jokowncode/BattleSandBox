
using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField] private TaskUI TaskUIPrefab;
    [SerializeField] private Transform TaskContainer;
    [SerializeField] private Transform MapTaskDirContainer;

    private TaskUI CurrentInstructTask;
    
    private void LateUpdate() {
        if (this.TaskContainer.childCount == 0) return;
        if (!this.CurrentInstructTask) return;

        foreach (Transform child in MapTaskDirContainer) {
            child.gameObject.SetActive(false);
        }
        
        Vector2 dir = this.CurrentInstructTask.GetTaskRotation();
        if (dir.x > 0.0f) {
            this.MapTaskDirContainer.GetChild(3).gameObject.SetActive(true);
        }else if (dir.x < 0.0f) {
            this.MapTaskDirContainer.GetChild(2).gameObject.SetActive(true);
        }
        
        if (dir.y > 0.0f) {
            this.MapTaskDirContainer.GetChild(0).gameObject.SetActive(true);
        }else if (dir.y < 0.0f) {
            this.MapTaskDirContainer.GetChild(1).gameObject.SetActive(true);
        }
    }

    private TaskUI GetCurrentFirstTask() {
        // TODO: DEFAULT Get First Has Dir Task -> Get Player Set Instruct Task
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI)) {
                if (taskUI.HasTaskPos) {
                    return taskUI;
                }
            }
        }
        return null;
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
        this.CurrentInstructTask = GetCurrentFirstTask();
    }

    public void UpdateTask(string taskName, string desc, Transform position) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                taskUI.SetTask(desc, taskName, position ? position.position : Vector3.zero);
                break;
            }
        }
        this.CurrentInstructTask = GetCurrentFirstTask();
    }

    public void AddTask(string taskName, Vector3 position) {
        TaskUI taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
        TaskData data = TaskManager.Instance.GetTask(taskName);
        taskUI.SetTask(data.TaskDescs[0], taskName, position);
        this.CurrentInstructTask = GetCurrentFirstTask();
    }

    public void RemoveTask(string taskName) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                Destroy(child.gameObject);
                break;
            }
        }
        this.CurrentInstructTask = GetCurrentFirstTask();
    }
}


