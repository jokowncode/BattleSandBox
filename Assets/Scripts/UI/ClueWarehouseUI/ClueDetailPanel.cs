
using System.Collections;
using TMPro;
using UnityEngine;

public class ClueDetailPanel : MonoBehaviour {

    [SerializeField] private DetailButton CurrentClueButton;
    [SerializeField] private TextMeshProUGUI ClueDescText;
    [SerializeField] private RectTransform DescContainer;
    
    public void SetClue(string clueName) {
        ClueData data = ClueWarehouseManager.Instance.GetClueByName(clueName);
        if (!data) return;
        CurrentClueButton.SetData(data.ClueName, data.ClueName, 0, false, GoodsType.None);
        CurrentClueButton.SetIcon(ClueWarehouseManager.Instance.ClueIcons[(int)data.Type], null);
        StopAllCoroutines();
        StartCoroutine(SetDescCoroutine(data.ClueDescription));
    }

    private IEnumerator SetDescCoroutine(string desc) {
        this.ClueDescText.text = desc;
        yield return null;
        Vector2 size = DescContainer.sizeDelta;
        size.y = this.ClueDescText.preferredHeight;
        DescContainer.sizeDelta = size;
    }

    public void GoBackToNormal() {
        this.CurrentClueButton.SetData("", "", 0, false, GoodsType.None);
        this.ClueDescText.text = "";
    }
}

