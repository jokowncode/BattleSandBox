
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
        
        // TODO: Load Current Player Task Procedure
        ShowNewTask(this.FirstTask);
    }

    public void ShowBattleStartUI(Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(background, battleImage, battleText);
    }

    public void ShowNewTask(TaskData newTask) {
        TaskUI.SetTask(newTask);
    }
}

