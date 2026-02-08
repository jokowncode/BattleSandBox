
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDisplayStoryPanel : HeroDisplayChildPanel {

    [SerializeField] private ScrollRect StoryScrollRect;
    [SerializeField] private RectTransform TextContainer;
    [SerializeField] private TextMeshProUGUI HeroStoryText;
    
    protected override void ShowData(Hero hero) {
        StopAllCoroutines();
        StartCoroutine(SetContentCoroutine(hero.WarehouseData.HeroStory));
    }

    private IEnumerator SetContentCoroutine(string text) {
        this.HeroStoryText.text = text;
        yield return null;
        Vector2 currentSize = this.TextContainer.sizeDelta;
        currentSize.y = this.HeroStoryText.preferredHeight;
        this.TextContainer.sizeDelta = currentSize;
        this.StoryScrollRect.verticalNormalizedPosition = 1.0f;
    }
}


