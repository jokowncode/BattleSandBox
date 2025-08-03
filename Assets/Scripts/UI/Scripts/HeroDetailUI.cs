using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroDetailUI : MonoBehaviour
{
    public static HeroDetailUI Instance;
    
    [SerializeField] private Image heroImage;
    
    [Header("Detail")]
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI HP;
    [SerializeField] private TextMeshProUGUI PA;
    [SerializeField] private TextMeshProUGUI MA;
    [SerializeField] private TextMeshProUGUI SD;
    [SerializeField] private TextMeshProUGUI CD;
    
    
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

    public void ChangeDetailUI(Hero hero)
    {
        Name.text = hero.Name;
        //Description.text = hero.Description;
        HP.text = hero.Health.ToString();
        PA.text = hero.PhysicsAttack.ToString();
        MA.text = hero.MagicAttack.ToString();
        SD.text = hero.Speed.ToString();
        //CD.text = hero.
    }
    
    
}
