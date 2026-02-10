
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryListPanel : MonoBehaviour {

    [SerializeField] private CategoryList CategoryListUI;
    [SerializeField] private Transform Container;
    [SerializeField] private DetailButton DetailButtonPrefab;

    public Func<string, int, bool> OnPassiveEntryClicked;
    
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
            
            button.SetData(desc, pName, pair.Value, true, (GoodsType)((int)pair.Key.Data.Rare));
            button.OnButtonClicked += OnButtonClicked;
        }
    }

    private void OnButtonClicked(string passiveEntryName, int count) {
        int requiredCount = PassiveEntryWarehouseManager.Instance.SynthPassiveEntryRequiredCount;
        if (count < requiredCount) {
            AudioManager.Instance.PlayErrorSfx();
            return;
        }
        bool? result = this.OnPassiveEntryClicked?.Invoke(passiveEntryName, requiredCount);
        if (result != null && result.Value) {
            foreach (Transform child in this.Container) {
                if (child.TryGetComponent(out DetailButton button) && button.Name == passiveEntryName) {
                    button.SetCount(count - requiredCount);
                }
            }
        }
    }

    public void Show() {
        this.CategoryListUI.SelectCategory(0, true);
    }

    public void ReturnPassiveEntry(string pName, int pCount) {
        foreach (Transform child in this.Container) {
            if (child.TryGetComponent(out DetailButton button) && button.Name == pName) {
                button.SetCount(button.GetCurrentCount() + pCount);
                return;
            }
        }
        
        PassiveEntry entry = PassiveEntryWarehouseManager.Instance.GetPassiveEntryByName(pName);
        if (!entry) return;
        
        DetailButton newButton = Instantiate(this.DetailButtonPrefab, this.Container);
        string desc = entry.Data.Description;
        newButton.SetData(desc, pName, pCount, true, (GoodsType)((int)entry.Data.Rare));
        newButton.OnButtonClicked += OnButtonClicked;
    }
}



