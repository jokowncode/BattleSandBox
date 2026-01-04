
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField] private TaskUI TaskUIPrefab;
    [SerializeField] private Transform TaskContainer;
    
    public void UpdateTaskUI() {
        int index = 0;
        foreach (var taskIndexMap in TaskManager.Instance.CurrentTaskDataMap) {
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
    }

    public void UpdateTask(string taskName, string desc, Transform position) {
        foreach (Transform child in TaskContainer) {
            if (child.TryGetComponent(out TaskUI taskUI) && taskUI.TaskName == taskName) {
                taskUI.SetTask(desc, taskName, position ? position.position : Vector3.zero);
                return;
            }
        }
    }

    public void AddTask(string taskName, Vector3 position) {
        TaskUI taskUI = Instantiate(this.TaskUIPrefab, this.TaskContainer);
        TaskData data = TaskManager.Instance.GetTask(taskName);
        taskUI.SetTask(data.TaskDescs[0], taskName, position);
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


