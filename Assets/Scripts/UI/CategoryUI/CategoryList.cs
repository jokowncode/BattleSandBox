
using System;
using System.Collections.Generic;
using UnityEngine;

public class CategoryList : MonoBehaviour {
    
    public Action<string, int> OnCategoryClicked;
    private List<CategoryButton> ChildButtons = new();
    
    private void Awake() {
        foreach (Transform child in this.transform) {
            if (child.TryGetComponent(out CategoryButton button)) {
                button.OnClicked += OnSelectedCategory;
                this.ChildButtons.Add(button);
            }
        }
    }

    public bool IsSelected(int index) {
        if (index >= this.ChildButtons.Count) return true;
        return this.ChildButtons[index].IsSelected;
    }

    public void SelectCategory(int index) {
        if (index >= this.transform.childCount) return;
        this.ChildButtons[index].ClickButton();
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



