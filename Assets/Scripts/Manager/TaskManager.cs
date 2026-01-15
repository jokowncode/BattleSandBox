
using System;
using System.Collections.Generic;
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
    
    public SerializableDictionary<string, TaskCurrentData> CurrentTaskDataMap { get; private set; }

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
        };
    }
    
    public TaskData GetTask(string taskName) {
        if (this.GameTaskMap.ContainsKey(taskName)) {
            return this.GameTaskMap[taskName];
        }
        return null;
    }

    public void NextTask(string taskName, Transform nextPosition) {
        if (this.CurrentTaskDataMap.ContainsKey(taskName)) {
            this.CurrentTaskDataMap[taskName].Index += 1;
            int newIndex = this.CurrentTaskDataMap[taskName].Index;
            if (newIndex >= this.GameTaskMap[taskName].TaskDescs.Length) {
                this.CurrentTaskDataMap.Remove(taskName);
                BigMapUIManager.Instance.TaskList.RemoveTask(taskName);
            } else {
                this.CurrentTaskDataMap[taskName].Position = nextPosition ? nextPosition.position : Vector3.zero;
                BigMapUIManager.Instance.TaskList.UpdateTask(taskName, 
                    this.GameTaskMap[taskName].TaskDescs[newIndex], nextPosition);
            }
        }
    }

    public void RemoveDungeonBindTask(SceneType dungeon) {
        if (!DungeonTaskMap.ContainsKey(dungeon)) return;
        string taskName = DungeonTaskMap[dungeon];
        if (!CurrentTaskDataMap.ContainsKey(taskName)) return;
        this.CurrentTaskDataMap.Remove(taskName);
        if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskList.RemoveTask(taskName);
    }

    public void AddTask(string taskName, Vector3 position) {
        if (!this.GameTaskMap.ContainsKey(taskName)) return;
        if (this.CurrentTaskDataMap.TryAdd(taskName, new TaskCurrentData() {
                Index = 0,
                Position = position
            })) {
            if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskList.AddTask(taskName, position);
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

