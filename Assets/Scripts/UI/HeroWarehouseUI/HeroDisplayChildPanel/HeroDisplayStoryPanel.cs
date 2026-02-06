
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDisplayStoryPanel : HeroDisplayChildPanel {

    [SerializeField] private ScrollRect StoryScrollRect;
    [SerializeField] private RectTransform TextContainer;
    [SerializeField] private TextMeshProUGUI HeroStoryText;
    
    protected override void ShowData(Hero hero) {
        this.HeroStoryText.text = hero.WarehouseData.HeroStory;
        Vector2 currentSize = this.TextContainer.sizeDelta;
        currentSize.y = this.HeroStoryText.preferredHeight;
        this.TextContainer.sizeDelta = currentSize;
        this.StoryScrollRect.verticalNormalizedPosition = 1.0f;
    }
}


