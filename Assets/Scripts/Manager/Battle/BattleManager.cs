using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : StateMachineController {

    public static BattleManager Instance;
    
    // TODO: Get BattleData From World Scene
    [SerializeField] private BattleData Data;
    [SerializeField] private EnemyDepartmentArea EnemyArea;
    
    [Header("Deploy Place Settings")]
    [SerializeField] private GameObject deployPlace;
    public int width;
    public int height;
    
    //private List<Hero> HeroesInBattle;
    public List<GameObject> HeroesInBattle;
    private List<Enemy> EnemiesInBattle;
    
    public Dictionary<GameObject,SkillData> Skills1InBattle;
    public Dictionary<GameObject,SkillData> Skills2InBattle;

    // TODO: eg:Support Passive Entry Register Action to Change Hero Property
    public Action OnHeroEnterTheField;
    public Action OnHeroExitTheField;
    
    [Header("Selected Hero")]
    public static GameObject selectedHero;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        HeroesInBattle = new List<GameObject>();
        Skills1InBattle = new Dictionary<GameObject,SkillData>();
        Skills2InBattle = new Dictionary<GameObject,SkillData>();
        // EnemiesInBattle = EnemyArea.InitializeEnemy(Data.EnemiesInBattle);
        // HeroesInBattle = new List<Hero>();
        //
        // // Turn To Prepare State
        // ChangeState(GetComponent<PrepareState>());
    }

    private void Start()
    {
        BattleUIManager.Instance.SetHeroWarhouseActive(true);
        BattleUIManager.Instance.SetHeroPanelActive(true);
        BattleUIManager.Instance.SetHeroPanelActive(false);
    }

    public void StartBattle()
    {
        foreach (GameObject hero in HeroesInBattle)
        {
            hero.GetComponent<Hero>().BattleStart();
        }
        BattleUIManager.Instance.SetHeroWarhouseActive(false); 
        BattleUIManager.Instance.SetHeroPanelActive(false);
    }

    public void AddHero(GameObject hero)
    {
        HeroesInBattle.Add(hero);
    }
    
    public bool IsWithinArea(Vector3 targetPos)
    {
        Vector3 checkPos = deployPlace.transform.position;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // 判断 checkPos 是否在 targetPos 所定义的矩形范围内
        bool withinX = checkPos.x >= targetPos.x - halfWidth && checkPos.x <= targetPos.x + halfWidth;
        bool withinZ = checkPos.z >= targetPos.z - halfHeight && checkPos.z <= targetPos.z + halfHeight;

        return withinX && withinZ;
    }
    
    private void Update()
    {
        GetSelectObject();
    }
    
    public void SelectObject(GameObject so)
    {
        Debug.Log("current so: "+so);
        selectedHero = so;
        if(selectedHero ==null)
        {
            BattleUIManager.Instance.SetHeroPanelActive(false);
        }
        else
        {
            BattleUIManager.Instance.SetHeroPanelActive(true);
            UpdateSkillUI();
            HeroDetailUI.Instance.ChangeHeroDetailUIValue(selectedHero.GetComponentInChildren<SpriteRenderer>().sprite);
        }
        return;
    }

    public void GetSelectObject()
    {
        // 忽略点击UI的情况
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("try get so");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,maxDistance:100f,layerMask:LayerMask.GetMask("Hero")))
            {
                SelectObject(hit.collider.gameObject);
            }
            else
            {
                SelectObject(null);
            }
        }
    }

    /// <summary>
    /// 召回英雄
    /// </summary>
    public void RecallSelectedHero()
    {
        RemoveSkill();
        HeroWarehouseUI.Instance.AddItem(selectedHero.GetComponentInChildren<SpriteRenderer>().sprite,selectedHero.name);
        HeroesInBattle.Remove(selectedHero);
        Destroy(selectedHero);
        BattleUIManager.Instance.SetHeroPanelActive(false);
        
    }
    
    /// <summary>
    /// 添加技能到空槽位，成功返回true，失败返回false。
    /// </summary>
    public int AddSkill(SkillData data)
    {
        if (!Skills1InBattle.ContainsKey(selectedHero))
        {
            Skills1InBattle.Add(selectedHero, data);
            UpdateSkillUI();
            Debug.Log("添加到 Skills1InBattle");
            return 0;
        }
        else if (!Skills2InBattle.ContainsKey(selectedHero))
        {
            Skills2InBattle.Add(selectedHero, data);
            UpdateSkillUI();
            Debug.Log("添加到 Skills2InBattle");
            return 1;
        }
        else
        {
            Debug.LogWarning("两个技能槽都已存在该GameObject，无法添加。");
            return -1;
        }
    }
    
    /// <summary>
    /// 清除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkill()
    {
        RemoveSkillFromSlot1();
        RemoveSkillFromSlot2();
    }
    
    /// <summary>
    /// 只从第一个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot1()
    {
        SkillData removedSkillData;
        if (Skills1InBattle.TryGetValue(selectedHero, out removedSkillData))
        {
            Skills1InBattle.Remove(selectedHero);
            RecallSelectedSkill(removedSkillData);
            UpdateSkillUI();
            Debug.Log("已从 Skills1InBattle 移除");
        }
        else
        {
            Debug.Log("Skills1InBattle 中未找到该 GameObject");
        }
    }
    
    /// <summary>
    /// 只从第二个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot2()
    {
        SkillData removedSkillData;
        if (Skills2InBattle.TryGetValue(selectedHero, out removedSkillData))
        {
            Skills2InBattle.Remove(selectedHero);
            RecallSelectedSkill(removedSkillData);
            UpdateSkillUI();
            Debug.Log("已从 Skills2InBattle 移除");
        }
        else
        {
            Debug.Log("Skills2InBattle 中未找到该 GameObject");
        }
    }

    public void RecallSelectedSkill(SkillData skill)
    {
        SkillWarehouseUI.Instance.AddItem(skill);
    }
    
    /// <summary>
    /// 根据 selectedHero 查找其两个技能，并更新 skill1UI 和 skill2UI 上的文本
    /// </summary>
    public void UpdateSkillUI()
    {
        string skill1Name = "";
        string skill2Name = "";

        if (Skills1InBattle.TryGetValue(selectedHero, out SkillData skill1))
        {
            skill1Name = skill1.Name;
        }
        else
        {
            skill1Name = "无技能1";
        }

        if (Skills2InBattle.TryGetValue(selectedHero, out SkillData skill2))
        {
            skill2Name = skill2.Name;
        }
        else
        {
            skill2Name = "无技能2";
        }

        BattleUIManager.Instance.SetSkill1UIText(skill1Name);
        BattleUIManager.Instance.SetSkill2UIText(skill2Name);
    }
    
    
    
}

