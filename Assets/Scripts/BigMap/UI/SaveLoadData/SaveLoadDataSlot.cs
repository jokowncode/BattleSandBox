
using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadDataSlot : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI DungeonNameText;
    [SerializeField] private TextMeshProUGUI CreateTimeText;
    [SerializeField] private TextMeshProUGUI PlayTimeText;
    [SerializeField] private TextMeshProUGUI CurrentTaskText;
    [SerializeField] private Button DeleteButton;

    [SerializeField] private Image[] TimeImages;
    [SerializeField] private Sprite HasFileTimeBackgroundSprite;
    [SerializeField] private Sprite NotFileTimeBackgroundSprite;

    private int Index;
    private SaveLoadDataUI UIParent;
    private string FileName;

    public void SetFileName(string fileName, int index, SaveLoadDataUI parent) {
        this.Index = index;
        this.UIParent = parent;
        SetFileName(fileName);
    }

    private void SetFileName(string fileName) {
        this.FileName = fileName;
        this.DeleteButton.gameObject.SetActive(this.Index != -1 && fileName != null);
        
        if (fileName != null) {
            string[] texts = fileName.Split('_');
            if (texts.Length < 5) {
                this.FileName = null;
                return;
            }

            string prefix = this.Index == -1 ? "自动存档" : "手动存档";
            if (this.CurrentTaskText) {
                this.CurrentTaskText.gameObject.SetActive(true);
                this.CurrentTaskText.text = texts[4];
            }

            if(this.DungeonNameText) this.DungeonNameText.text = $"{prefix}：{texts[3]}";
            if(this.CreateTimeText) 
                this.CreateTimeText.text = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(texts[1])).LocalDateTime.ToString("G");
            if(this.PlayTimeText) this.PlayTimeText.text = GetPlayTimeString(long.Parse(texts[2]));

            foreach (Image timeImage in this.TimeImages) {
                timeImage.sprite = this.HasFileTimeBackgroundSprite;
            }

        } else {
            if(this.DungeonNameText) this.DungeonNameText.text = "新存档";
            if(this.CreateTimeText) this.CreateTimeText.text = "";
            if(this.PlayTimeText) this.PlayTimeText.text = "";
            if(this.CurrentTaskText) this.CurrentTaskText.gameObject.SetActive(false);
            
            foreach (Image timeImage in this.TimeImages) {
                timeImage.sprite = this.NotFileTimeBackgroundSprite;
            }
        }
    }

    private void SaveData() {
        string newFileName = SaveDataManager.Instance.MutualSaveData(Index);
        SetFileName(newFileName);
    }

    public void SaveOrLoad() {
        if (this.UIParent.IsSaveData) {
            if (this.Index < 0) return;
            if (SaveDataManager.Instance.HasMutualSaveData(this.Index)) {
                this.UIParent.ShowConfirmDialog(this.SaveData, "确定覆盖存档？");
            } else {
                this.SaveData();
            }
        } else if (this.FileName != null) {
            SaveDataManager.Instance.LoadData(Path.Combine(Application.persistentDataPath, this.FileName));
            this.UIParent.TransitionShow(false, false);
            GameManager.Instance.EnterGame();
        }
    }

    public void DeleteSaveData() {
        if (this.Index < 0) return;
        this.UIParent.ShowConfirmDialog(() => {
            SaveDataManager.Instance.DeleteMutualSaveData(this.Index);
            this.SetFileName(null);
        }, "确定删除存档？");
    }

    private static string GetPlayTimeString(long seconds) {
        long hour = seconds / 3600;
        long minute = seconds / 60 - hour * 60;
        long second = seconds % 60;
        return $"{hour:00}:{minute:00}:{second:00}";
    }

}


