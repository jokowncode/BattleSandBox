using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroDetailUI : MonoBehaviour {
    
    [SerializeField] private Image heroImage;
    
    [Header("Detail")]
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private GameObject StarLevelUI;
    [SerializeField] private GameObject StarLevelPrefab;
    [SerializeField] private TextMeshProUGUI HP;
    [SerializeField] private TextMeshProUGUI PA;
    [SerializeField] private TextMeshProUGUI MA;
    [SerializeField] private TextMeshProUGUI SD;
    [SerializeField] private TextMeshProUGUI CD;

    public void Hide(){
        this.gameObject.SetActive(false);
    }

    public void ChangeHeroDetailUIValue(Sprite sprite)
    {
        heroImage.sprite = sprite;
    }

    public void ChangeDetailUI(Hero hero)
    {
        Name.text = hero.Name;
        Description.text = hero.Description;
        UpdateStarLevelUI(hero);
        HP.text = hero.Health.ToString();
        PA.text = hero.PhysicsAttack.ToString();
        MA.text = hero.MagicAttack.ToString();
        SD.text = hero.Speed.ToString();
        //CD.text = hero.
    }

    private void UpdateStarLevelUI(Hero hero)
    {
        foreach (Transform child in StarLevelUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < hero.StarLevel; i++)
        {
            GameObject go = Instantiate(StarLevelPrefab, StarLevelUI.transform);
        }
        
    }
    
    
}
