
using System;
using System.Collections.Generic;
using UnityEngine;

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private BattleStartUI BattleStartBannar;
    
    [Header("Task")]
    [SerializeField] private TaskData FirstTask;
    [SerializeField] private TaskUI TaskUI;
    
    public static BigMapUIManager Instance;

    private Store CurrentShowStore;
    public bool IsOpenStore => this.CurrentShowStore;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        if (SaveMapManager.Instance.IsFirstLoad) {
            SaveMapManager.Instance.OnLoadMap += OnLoadMap;
        }
    }

    private void OnLoadMap() {
        if (SaveMapManager.Instance.CurrentTask.TaskPosition) {
            ShowNewTask(SaveMapManager.Instance.CurrentTask);    
        } else {
            ShowNewTask(this.FirstTask);   
        }
    }

    public void ShowBattleStartUI(Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(background, battleImage, battleText);
    }

    public void ShowNewTask(TaskData newTask) {
        SaveMapManager.Instance.CurrentTask = newTask;
        TaskUI.SetTask(newTask);
    }
}

