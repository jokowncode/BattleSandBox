
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryListPanel : MonoBehaviour {

    [SerializeField] private CategoryList CategoryListUI;
    [SerializeField] private Transform Container;
    [SerializeField] private DetailButton DetailButtonPrefab;

    private void Awake() {
        this.CategoryListUI.OnCategoryClicked += SelectCategory;
    }
    
    private void SelectCategory(string categoryName, int index) {
        foreach (Transform child in this.Container) {
            Destroy(child.gameObject);
        }

        Dictionary<PassiveEntry, int> entries =  PassiveEntryWarehouseManager.Instance.GetPassiveEntryByStar(index+1);
        foreach (var pair in entries) {
            DetailButton button = Instantiate(this.DetailButtonPrefab, this.Container);
            string desc = pair.Key.Data.Description;
            string pName = pair.Key.Data.Name;
            button.SetData(desc, pName, pair.Value, true);
            button.OnButtonClicked += OnButtonClicked;
        }
    }

    private void OnButtonClicked(string passiveEntryName) {
        
    }

    public void Show() {
        if (!this.CategoryListUI.IsSelected(0)) {
            this.CategoryListUI.SelectCategory(0);
        }
    }
}



