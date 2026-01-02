
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour {

    public static TaskManager Instance;

    [SerializeField] private List<TaskData> GameTasks;
    private Dictionary<string, TaskData> GameTaskMap;

    [Header("Debug")] 
    [SerializeField] private List<string> DebugExistTasks;
    
    public Dictionary<string, int> CurrentTaskIndexMap { get; private set; }

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
            string taskIndex = JsonUtility.ToJson(new Serialization<string, int>(this.CurrentTaskIndexMap));
            PlayerPrefs.SetString("CurrentTaskIndexMap", taskIndex);
        };

        SaveMapManager.Instance.OnLoadData += () => {
            if (PlayerPrefs.HasKey("CurrentTaskIndexMap")) {
                this.CurrentTaskIndexMap = JsonUtility.FromJson<Serialization<string, int>>(PlayerPrefs.GetString("CurrentTaskIndexMap"))
                    .ToDictionary();
            } else {
                this.CurrentTaskIndexMap = new Dictionary<string, int>();
            }
        
            // TODO: TEMP -> FOR DEBUG
            if (this.CurrentTaskIndexMap.Count == 0) {
                foreach (string taskName in this.DebugExistTasks) {
                    this.AddTask(taskName);
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

    public void NextTask(string taskName) {
        if (this.CurrentTaskIndexMap.ContainsKey(taskName)) {
            this.CurrentTaskIndexMap[taskName]++;
            int newIndex = this.CurrentTaskIndexMap[taskName];
            if (newIndex >= this.GameTaskMap[taskName].TaskDescs.Length) {
                this.CurrentTaskIndexMap.Remove(taskName);
                BigMapUIManager.Instance.TaskList.RemoveTask(taskName);
            } else {
                BigMapUIManager.Instance.TaskList.UpdateTask(taskName, this.GameTaskMap[taskName].TaskDescs[newIndex]);
            }
        }
    }

    public void AddTask(string taskName) {
        if (!this.GameTaskMap.ContainsKey(taskName)) return;
        if (this.CurrentTaskIndexMap.TryAdd(taskName, 0)) {
            if(BigMapUIManager.Instance) BigMapUIManager.Instance.TaskList.AddTask(taskName);
        }
    }

    public bool HasTask(string taskName) {
        return this.CurrentTaskIndexMap.ContainsKey(taskName);
    }
}

