
using System;
using System.Collections.Generic;
using UnityEngine;

public class CategoryList : MonoBehaviour {

    [SerializeField] private string[] CategoryNames;
    [SerializeField] private CategoryButton CategoryButtonPrefab;
    
    public Action<string, int> OnCategoryClicked;
    private List<CategoryButton> ChildButtons = new();
    
    private void Awake() {
        if (this.CategoryNames != null && this.CategoryNames.Length != 0) {
            foreach (string category in CategoryNames) {
                CategoryButton button = Instantiate(this.CategoryButtonPrefab, this.transform);
                button.SetCategoryName(category);
                button.OnClicked += OnSelectedCategory;
                this.ChildButtons.Add(button);
            }
        }
    }

    public bool IsSelected(int index) {
        if (index >= this.ChildButtons.Count) return true;
        return this.ChildButtons[index].IsSelected;
    }

    public void SelectCategory(int index, bool isForce = false) {
        if (index >= this.transform.childCount) return;
        this.ChildButtons[index].ClickButton(isForce);
    }

    private void OnSelectedCategory(string categoryName) {
        int selectedIndex = -1;
        for (int i = 0; i < this.ChildButtons.Count; i++) {
            if (this.ChildButtons[i].CategoryName == categoryName) {
                selectedIndex = i;
            }
            this.ChildButtons[i].OtherIsSelected(categoryName); 
        }

        this.OnCategoryClicked?.Invoke(categoryName, selectedIndex);
    }
}



