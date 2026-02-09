
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClueListPanel : MonoBehaviour {

    [SerializeField] private Transform Container;
    [SerializeField] private CategoryList CategoryListUI;
    [SerializeField] private DetailButton DetailButtonPrefab;

    public Action<string> OnClueClicked; 
    
    private void Awake() {
        this.CategoryListUI.OnCategoryClicked += OnCategoryClicked;
    }

    private void OnCategoryClicked(string cName, int index) {
        foreach (Transform child in this.Container) {
            Destroy(child.gameObject);
        }

        ClueType type = (ClueType)index;
        List<ClueData> data = ClueWarehouseManager.Instance.GetOwnedCluesByType(type);
        foreach (ClueData clue in data) {
            DetailButton button = Instantiate(this.DetailButtonPrefab, this.Container);
            button.SetData(clue.ClueName, clue.ClueName, 1, true, GoodsType.None);
            button.SetIcon(ClueWarehouseManager.Instance.ClueIcons[(int)clue.Type], null);
            button.OnButtonClicked += (clueName, _) => OnClueClicked?.Invoke(clueName);
        }
    }

    public void Show() {
        this.CategoryListUI.SelectCategory(0, true);
    }

}



