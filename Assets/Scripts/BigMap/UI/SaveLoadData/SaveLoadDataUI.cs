
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveLoadDataUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private int MaxMutualSaveSlots = 5;
    [SerializeField] private Transform Container;
    [SerializeField] private SaveLoadDataSlot SaveLoadDataSlotPrefab;
    
    private CanvasGroup UICanvasGroup;

    public bool IsSaveData { get; private set; } = false;

    private void Awake() {
        this.UICanvasGroup = this.GetComponent<CanvasGroup>();
        this.Close();
    }

    private void Close() {
        this.TransitionShow(false, false);
    }

    public void TransitionShow(bool show, bool isSaveData) {
        if (show && this.UICanvasGroup.alpha >= 0.9f) return;
        this.IsSaveData = isSaveData;
        if(show) this.InitSaveLoadDataSlot();
        this.UICanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.UICanvasGroup.interactable = show;
        this.UICanvasGroup.blocksRaycasts = show;
        this.TitleText.text = isSaveData ? "系统存档" : "系统读档";
    }

    private void InitSaveLoadDataSlot() {
        foreach (Transform child in this.Container) {
            Destroy(child.gameObject);
        }

        if (!this.IsSaveData) {
            foreach (string fileName in SaveDataManager.Instance.AutoSaveDataPaths) {
                SaveLoadDataSlot slot = Instantiate(this.SaveLoadDataSlotPrefab, this.Container);
                slot.SetFileName(fileName, -1, this);
            }    
        }

        for (int i = 0; i < this.MaxMutualSaveSlots; i++) {
            SaveLoadDataSlot slot = Instantiate(this.SaveLoadDataSlotPrefab, this.Container);
            string fileName = SaveDataManager.Instance.MutualSaveDataPathMap.GetValueOrDefault(i);
            slot.SetFileName(fileName, i, this);
        }
    }
}


