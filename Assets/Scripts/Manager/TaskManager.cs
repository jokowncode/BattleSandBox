
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

    [Header("Debug")] 
    [SerializeField] private List<string> DebugExistTasks;
    
    public Dictionary<string, TaskCurrentData> CurrentTaskDataMap { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.GameTaskMap = new Dictionary<string, TaskData>();
        foreach (TaskData taskData in this.GameTasks) {
            this.GameTaskMap.Add(taskData.TaskName, taskData);
        }
    }

    private void Start() {
        SaveMapManager.Instance.OnSaveData += () => {
            string taskIndex = JsonUtility.ToJson(new Serialization<string, TaskCurrentData>(this.CurrentTaskDataMap));
            PlayerPrefs.SetString("CurrentTaskDataMap", taskIndex);
        };

        SaveMapManager.Instance.OnLoadData += () => {
            if (PlayerPrefs.HasKey("CurrentTaskDataMap")) {
                this.CurrentTaskDataMap = JsonUtility.FromJson<Serialization<string, TaskCurrentData>>(PlayerPrefs.GetString("CurrentTaskDataMap"))
                    .ToDictionary();
            } else {
                this.CurrentTaskDataMap = new Dictionary<string, TaskCurrentData>();
            }
        
            // TODO: TEMP -> FOR DEBUG
            if (this.CurrentTaskDataMap.Count == 0) {
                foreach (string taskName in this.DebugExistTasks) {
                    this.AddTask(taskName, Vector3.zero);
                }    
            }
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

    public void AddTask(string taskName, Vector3 position) {
        if (!this.GameTaskMap.ContainsKey(taskName)) return;
        if (this.CurrentTaskDataMap.TryAdd(taskName, new TaskCurrentData() {
                Index = 0,
                Position = position
            })) {
            if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskList.AddTask(taskName, position);
        }
    }

    public bool HasTask(string taskName) {
        return this.CurrentTaskDataMap.ContainsKey(taskName);
    }
}

