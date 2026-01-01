
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField] private TaskUI TaskUIPrefab;
    [SerializeField] private Transform TaskContainer;
    
    public void UpdateTaskUI() {
        int index = 0;
        foreach (var taskIndexMap in TaskManager.Instance.CurrentTaskIndexMap) {
            TaskUI taskUI = null;
            if (this.TaskContainer.childCount - 1 < index) {
                taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
            }else {
                this.TaskContainer.GetChild(index).TryGetComponent(out taskUI);
            }

            if (taskUI) {
                TaskData data = TaskManager.Instance.GetTask(taskIndexMap.Key);
                if (data) {
                    taskUI.SetTask(data.TaskDescs[taskIndexMap.Value], taskIndexMap.Key);
                }
            }
            index += 1;
        }
    }

    public void UpdateTask(string taskName, string desc) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                taskUI.SetTask(desc, taskName);
                return;
            }
        }
    }

    public void AddTask(string taskName) {
        TaskUI taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
        TaskData data = TaskManager.Instance.GetTask(taskName);
        taskUI.SetTask(data.TaskDescs[0], taskName);
    }

    public void RemoveTask(string taskName) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                Destroy(child.gameObject);
                return;
            }
        }
    }
}


