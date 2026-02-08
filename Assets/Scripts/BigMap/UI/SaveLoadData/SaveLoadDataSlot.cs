
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadDataSlot : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI DungeonNameText;
    [SerializeField] private TextMeshProUGUI CreateTimeText;
    [SerializeField] private TextMeshProUGUI PlayTimeText;
    [SerializeField] private Button DeleteButton;

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
            if (texts.Length < 4) {
                this.FileName = null;
                return;
            }
            if(this.DungeonNameText) this.DungeonNameText.text = texts[3];
            if(this.CreateTimeText) 
                this.CreateTimeText.text = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(texts[1])).LocalDateTime.ToString("G");
            if(this.PlayTimeText) this.PlayTimeText.text = GetPlayTimeString(long.Parse(texts[2]));
        } else {
            if(this.DungeonNameText) this.DungeonNameText.text = "";
            if(this.CreateTimeText) this.CreateTimeText.text = "";
            if(this.PlayTimeText) this.PlayTimeText.text = "";
        }
    }

    public void SaveOrLoad() {
        if (this.UIParent.IsSaveData) {
            if (this.Index < 0) return;
            string newFileName = SaveDataManager.Instance.MutualSaveData(Index);
            SetFileName(newFileName);
        } else if (this.FileName != null) {
            SaveDataManager.Instance.LoadData(Path.Combine(Application.persistentDataPath, this.FileName));
            this.UIParent.TransitionShow(false, false);
            GameManager.Instance.EnterGame();
        }
    }

    public void DeleteSaveData() {
        if (this.FileName != null) {
            string deletePath = Path.Combine(Application.persistentDataPath, this.FileName);
            if (File.Exists(deletePath)) {
                File.Delete(deletePath);
            }
            this.SetFileName(null);
        }
    }

    private static string GetPlayTimeString(long seconds) {
        long hour = seconds / 3600;
        long minute = seconds % 60;
        return $"{hour:00}:{minute:00}";
    }

}


