using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeroLinkUI : MonoBehaviour
{
    public static HeroLinkUI Instance;
    
    [Header("UI References")]
    public GameObject panel;
    public List<GameObject> HeroSlots;
    public Transform availableHeroesContent;
    [HideInInspector]public List<Hero> AllHeroes;
    [HideInInspector]public List<Hero> HeroesInSlots;
    //public Text groupInfoText; 
    
    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject availableHeroesPrefab;
    
    [Header("Debug")]
    public bool isDebugging = false;
    
    private int selectedSlotIndex = -1;
    //private List<FormationSlot> slots = new List<FormationSlot>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        HeroesInSlots = new List<Hero>();
        for (int i = 0; i < 6; i++)
        {
            HeroesInSlots.Add(null);
        }
    }

    void Start()
    {

        HideUI();

        InitializeSlots();
    }
    
    void InitializeSlots()
    {
        // for (int i = 0; i < 9; i++)
        // {
        //     GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
        //     FormationSlot slot = slotObj.GetComponent<FormationSlot>();
        //     slot.Initialize(i, this);
        //     slots.Add(slot);
        // }
    }
    
    public void ShowUI()
    {
        panel.SetActive(true);
        //UpdateUI();
    }
    
    public void HideUI()
    {
        panel.SetActive(false);
        selectedSlotIndex = -1; // 重置选中的槽位
    }
    
    // CHangeVisibility
    public void ChangeUIVisibility()
    {
        Debug.Log("Change UI Visibility");
        if(panel.activeSelf)
            HideUI();
        else
            ShowUI();
    }
    
    // public void UpdateUI()
    // {
    //     UpdateSlots();
    //     
    //     UpdateAllHeroes();
    //     
    //     UpdateGroupInfo();
    // }
    
    void UpdateSlots()
    {
        // for (int i = 0; i < slots.Count; i++)
        // {
        //     // 检查BattleManager中是否有对应位置的英雄
        //     if (i < BattleManager.Instance.HeroesInBattle.Count && 
        //         BattleManager.Instance.HeroesInBattle[i] != null)
        //     {
        //         slots[i].SetHero(BattleManager.Instance.HeroesInBattle[i]);
        //     }
        //     else
        //     {
        //         slots[i].ClearSlot();
        //     }
        //     
        //     // 高亮选中的槽位
        //     slots[i].SetSelected(i == selectedSlotIndex);
        // }
    }
    
    public void UpdateAllHeroes()
    {
        Debug.Log("Update All Heroes");
        List<Hero> allHeroes = BattleManager.Instance.HeroesInBattle;
        
        if (HasRemovedHeroes(allHeroes))
        {
            HandleRemovedHeroes(allHeroes);
        }
        
        if (HasAddedHeroes(allHeroes))
        {
            HandleAddedHeroes(allHeroes);
        }
        
        //UpdateUI();
    }
    
    bool HasRemovedHeroes(List<Hero> newHeroList)
    {
        return AllHeroes.Any(hero => !newHeroList.Contains(hero));
    }
    
    void HandleRemovedHeroes(List<Hero> newHeroList)
    {
        var heroesToRemove = AllHeroes.Where(hero => !newHeroList.Contains(hero)).ToList();
        
        foreach (var hero in heroesToRemove)
        {
            AllHeroes.Remove(hero);
            Debug.Log($"英雄 {hero.Name} 已从可用列表中移除");
        }
    }
    
    bool HasAddedHeroes(List<Hero> newHeroList)
    {
        return newHeroList.Any(hero => !AllHeroes.Contains(hero));
    }
    
    void HandleAddedHeroes(List<Hero> newHeroList)
    {
        var heroesToAdd = newHeroList.Where(hero => !AllHeroes.Contains(hero)).ToList();
        
        foreach (var hero in heroesToAdd)
        {
            AllHeroes.Add(hero);
            GameObject heroCard = Instantiate(availableHeroesPrefab, availableHeroesContent);
            heroCard.GetComponent<HeroLinkClickableUI>().Hero =  hero;
            heroCard.GetComponent<Image>().sprite = hero.heroPortraitSprite;
            Debug.Log($"英雄 {hero.Name} 已添加到可用列表");
        }
    }
    
    void UpdateGroupInfo()
    {
        // List<List<Hero>> groups = BattleManager.Instance.GetFormationGroups();
        // groupInfoText.text = "队伍组成:\n";
        //
        // for (int i = 0; i < groups.Count; i++)
        // {
        //     groupInfoText.text += $"队伍 {i + 1}: ";
        //     if (groups[i].Count > 0)
        //     {
        //         groupInfoText.text += string.Join(", ", groups[i].Select(h => h.Name));
        //     }
        //     else
        //     {
        //         groupInfoText.text += "空";
        //     }
        //     groupInfoText.text += "\n";
        // }
    }
    
    public void SelectSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        //UpdateUI();
        Debug.Log($"选中槽位: {slotIndex}");
    }
    
    public bool AssignHeroToSelectedSlot(Hero hero)
    {
        if (selectedSlotIndex == -1)
        {
            Debug.Log("请先选择一个槽位");
            return false;
        }else if (!CanAssignToSlot())
        {
            Debug.Log("请将前面槽位填满");
            return false;
        }

        Hero tempHero = HeroSlots[selectedSlotIndex].GetComponent<HeroLinkSlotUI>().Hero;
        if(tempHero != null)
            AddHeroToAvailable(tempHero);
        HeroSlots[selectedSlotIndex].GetComponent<HeroLinkSlotUI>().Hero = hero;
        HeroSlots[selectedSlotIndex].GetComponent<Image>().sprite = hero.heroPortraitSprite;
        HeroSlots[selectedSlotIndex].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        Debug.Log("s: "+selectedSlotIndex);
        Debug.Log("c: " + HeroesInSlots.Count);
        HeroesInSlots[selectedSlotIndex] = hero;
        return true;
    }
    
    public void RemoveHeroFromSlot(Hero hero)
    {
        int index = HeroesInSlots.FindIndex(h => h == hero);
        if (index >= 0)
        {
            HeroesInSlots[index] = null;
            AddHeroToAvailable(hero);
        }
    }

    public bool CanAssignToSlot()
    {
        int groupIndex = selectedSlotIndex % 2;
    
        // 如果当前是组内的第一个槽位，无需检查前面的槽位
        if (groupIndex == 0)
            return true;
    
        // 检查组内前面的所有槽位
        for (int i = 0; i < groupIndex; i++)
        {
            int slotToCheck = selectedSlotIndex - (groupIndex - i);
            if (slotToCheck < 0 || slotToCheck >= HeroSlots.Count)
                continue;
            
            Hero heroInSlot = HeroSlots[slotToCheck].GetComponent<HeroLinkSlotUI>().Hero;
            if (heroInSlot == null)
                return false;
        }
    
        return true;
    }

    public void AddHeroToAvailable(Hero hero)
    {
        GameObject heroCard = Instantiate(availableHeroesPrefab, availableHeroesContent);
        heroCard.GetComponent<HeroLinkClickableUI>().Hero =  hero;
        heroCard.GetComponent<Image>().sprite = hero.heroPortraitSprite;
        if(isDebugging)
            Debug.Log($"英雄 {hero.Name} 已添加到可用列表");
    }
    
    public List<List<Hero>> GetAllNonEmptyGroups()
    {
        List<List<Hero>> result = new List<List<Hero>>();
    
        for (int group = 0; group < 3; group++)
        {
            List<Hero> groupHeroes = GetGroupData(group);
            if (groupHeroes.Count > 0)
            {
                result.Add(groupHeroes);
                // 输出该组数据
                string groupInfo = $"组 {group} 包含英雄: ";
                foreach (Hero hero in groupHeroes)
                {
                    groupInfo += hero != null ? hero.Name + ", " : "null, ";
                }
                Debug.Log(groupInfo.TrimEnd(',', ' '));
            }
        }
    
        return result;
    }
    
    public void GetAllNonEmptyGroupsTest()
    {
        List<List<Hero>> result = new List<List<Hero>>();
    
        for (int group = 0; group < 3; group++)
        {
            List<Hero> groupHeroes = GetGroupData(group);
            if (groupHeroes.Count > 0)
            {
                result.Add(groupHeroes);
                // 输出该组数据
                string groupInfo = $"组 {group} 包含英雄: ";
                foreach (Hero hero in groupHeroes)
                {
                    groupInfo += hero != null ? hero.Name + ", " : "null, ";
                }
                Debug.Log(groupInfo.TrimEnd(',', ' '));
            }
        }
    }
    
    public List<Hero> GetGroupData(int groupIndex)
    {
        if (groupIndex < 0 || groupIndex > 2) 
            return new List<Hero>();
    
        int startIndex = groupIndex * 2;
        List<Hero> groupData = new List<Hero>();
        
        bool foundNonEmpty = false;
        for (int i = startIndex + 1; i >= startIndex; i--)
        {
            if (i < HeroesInSlots.Count && HeroesInSlots[i] != null)
            {
                foundNonEmpty = true;
            }
        
            if (foundNonEmpty && i < HeroesInSlots.Count)
            {
                groupData.Insert(0, HeroesInSlots[i]);
            }
        }
    
        return groupData;
    }
    
    public List<Hero> GetAllNonEmptyHeroes()
    {
        List<Hero> result = new List<Hero>();
    
        for (int group = 0; group < 3; group++)
        {
            List<Hero> groupHeroes = GetGroupData(group);
            result.AddRange(groupHeroes);
        }
    
        return result;
    }
    
}
