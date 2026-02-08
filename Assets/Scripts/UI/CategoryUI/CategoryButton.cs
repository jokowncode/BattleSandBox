
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI CategoryNameText;
    [SerializeField] private Sprite NotSelectBackground;
    [SerializeField] private Sprite SelectedBackground;

    public Action<string> OnClicked;
    private Image BackgroundImage;
    public bool IsSelected { get; private set; } = false;
    public string CategoryName => this.CategoryNameText.text;

    private void Awake() {
        this.BackgroundImage = this.GetComponent<Image>();
        if (this.TryGetComponent(out Button button)) {
            button.onClick.AddListener(ClickButton);
        }
    }

    public void ClickButton() {
        if (this.IsSelected) return;
        this.BackgroundImage.sprite = this.SelectedBackground;
        this.OnClicked?.Invoke(this.CategoryNameText.text);
        this.IsSelected = true;
    }

    public void OtherIsSelected(string selectedName) {
        if (this.CategoryNameText.text == selectedName) return;
        if (!this.IsSelected) return;
        this.IsSelected = false;
        this.BackgroundImage.sprite = this.NotSelectBackground;
    }
}



