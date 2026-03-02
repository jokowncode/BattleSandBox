
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroBondPropertyChange : MonoBehaviour {

    private readonly Color NormalColor = Color.white;
    private readonly Color DisableColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    
    private void Start() {
        List<EntanglementData> datas =  EntanglementManager.Instance.EntanglementLevelDatas;
        for (int i = 0; i < this.transform.childCount; i++) {
            if (this.transform.GetChild(i).TryGetComponent(out TextMeshProUGUI text)) {
                text.text = i >= datas.Count ? "" : $"LV.{i+1}    {datas[i].LevelDescription}";
                text.color = this.DisableColor;
            }
        }
    }

    public void SetContent(int level) {
        for (int i = 0; i < this.transform.childCount; i++) {
            if (this.transform.GetChild(i).TryGetComponent(out TextMeshProUGUI text)) {
                text.color = i+1 <= level ? this.NormalColor : this.DisableColor;
            }
        }
    }
}

