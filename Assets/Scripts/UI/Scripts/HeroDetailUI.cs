using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroDetailUI : MonoBehaviour
{
    public static HeroDetailUI Instance;
    
    [SerializeField] private Image heroImage;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        if (heroImage != null)
            heroImage.sprite = null;
            
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeHeroDetailUIValue(Sprite sprite)
    {
        heroImage.sprite = sprite;
    }
    
    
}
