
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class TaskCurrentData {
    public int Index;
    public Vector3 Position;
}

public class TaskManager : MonoBehaviour {

    public static TaskManager Instance;

    [SerializeField] private List<TaskData> GameTasks;
    private Dictionary<string, TaskData> GameTaskMap;
    private Dictionary<SceneType, string> DungeonTaskMap;

    private SerializableDictionary<string, TaskCurrentData> CurrentTaskDataMap;
    public string CurrentFollowTaskName { get; private set; } = null;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.GameTaskMap = new Dictionary<string, TaskData>();
        this.DungeonTaskMap = new Dictionary<SceneType, string>();
        foreach (TaskData taskData in this.GameTasks) {
            this.GameTaskMap.Add(taskData.TaskName, taskData);
            if (taskData.BindDungeon != SceneType.None) {
                this.DungeonTaskMap.TryAdd(taskData.BindDungeon, taskData.TaskName);
            }
        }
    }

    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            this.CurrentTaskDataMap = SaveDataManager.Instance.PlayerData.CurrentTaskDataMap;
            this.CurrentFollowTaskName = SaveDataManager.Instance.PlayerData.CurrentFollowTaskName;
            if (string.IsNullOrEmpty(this.CurrentFollowTaskName) && this.CurrentTaskDataMap.Count != 0) {
                this.CurrentFollowTaskName = GetCurrentFirstTask();
            }
        };
    }

    private string GetCurrentFirstTask() {
        if (this.CurrentTaskDataMap.Count == 0) return null;
        foreach (KeyValuePair<string, TaskCurrentData> pair in this.CurrentTaskDataMap) {
            if (!this.GameTaskMap.ContainsKey(pair.Key)) continue;
            return pair.Key;
        }
        return null;
    }

    public string GetTaskDesc() {
        if (string.IsNullOrEmpty(this.CurrentFollowTaskName)) return "无任务";
        TaskCurrentData data = GetCurrentTaskData(this.CurrentFollowTaskName);
        return this.GameTaskMap[this.CurrentFollowTaskName].TaskDescs[data.Index];
    }

    public TaskData GetTask(string taskName) {
        if (this.GameTaskMap.ContainsKey(taskName)) {
            return this.GameTaskMap[taskName];
        }
        return null;
    }

    public TaskCurrentData GetCurrentTaskData(string taskName) {
        if (this.CurrentTaskDataMap.ContainsKey(taskName)) {
            return this.CurrentTaskDataMap[taskName];
        }
        return null;
    }

    public void NextTask(string taskName, Transform nextPosition) {
        if (this.CurrentTaskDataMap.ContainsKey(taskName)) {
            this.CurrentTaskDataMap[taskName].Index += 1;
            int newIndex = this.CurrentTaskDataMap[taskName].Index;
            if (newIndex >= this.GameTaskMap[taskName].TaskDescs.Length) {
                this.CurrentTaskDataMap.Remove(taskName);
                this.CurrentFollowTaskName = GetCurrentFirstTask();
                BigMapUIManager.Instance.TaskUI.UpdateTask();
            } else {
                this.CurrentTaskDataMap[taskName].Position = nextPosition ? nextPosition.position : Vector3.zero;
                BigMapUIManager.Instance.TaskUI.UpdateTask();
            }
        }
    }

    public void RemoveDungeonBindTask(SceneType dungeon) {
        if (!DungeonTaskMap.ContainsKey(dungeon)) return;
        string taskName = DungeonTaskMap[dungeon];
        if (!CurrentTaskDataMap.ContainsKey(taskName)) return;
        this.CurrentTaskDataMap.Remove(taskName);
        this.CurrentFollowTaskName = GetCurrentFirstTask();
        if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskUI.UpdateTask();
    }

    public void AddTask(string taskName, Vector3 position) {
        if (!this.GameTaskMap.ContainsKey(taskName)) return;
        if (this.CurrentTaskDataMap.TryAdd(taskName, new TaskCurrentData() {
                Index = 0,
                Position = position
            })) {
            if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskUI.UpdateTask();
            if (this.CurrentTaskDataMap.Count == 1) {
                this.CurrentFollowTaskName = taskName;
            }
        }
    }

    public void AddDungeonBindTask(SceneType dungeon) {
        if (!this.DungeonTaskMap.ContainsKey(dungeon)) return;
        this.AddTask(this.DungeonTaskMap[dungeon], Vector3.zero);
    }

    public bool HasTask(string taskName) {
        return this.CurrentTaskDataMap.ContainsKey(taskName);
    }
}

