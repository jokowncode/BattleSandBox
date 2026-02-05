
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMergeManager : MonoBehaviour {

    // [SerializeField] private List<PassiveEntry> HeroMergePassiveEntries;
    
    [Header("General")]
    [SerializeField] private float MergeEnergy = 5;
    
    [Header("Merge Version")]
    [SerializeField] private BuffData HeroMergeBuff;
    [SerializeField] private float HeroMergeDuration = 5.0f;

    /*[Header("Tactic Version")] 
    [SerializeField] private BattleTacticType UseTacticType;*/
    
    public static HeroMergeManager Instance;
    // private HeroMergeGroupData[] HeroMergeGroup;
    private WaitForSeconds HeroMergeTimer;
    
    private Dictionary<Hero, float> HeroEnergies = new Dictionary<Hero, float>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        this.HeroMergeBuff.Duration = HeroMergeDuration;
        this.HeroMergeTimer = new WaitForSeconds(this.HeroMergeDuration);
    }

    private void Start() {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
        // this.HeroMergeGroup = new HeroMergeGroupData[BattleManager.Instance.Data.MaxHeroCount / 2];
    }

    private void OnBattleStart() {
        // Hero Merge Version
        /*if (BattleManager.Instance.HeroesInBattle.Count >= 2) {
            HeroMergeGroup[0] = new HeroMergeGroupData {
                MergeHeroes = new List<Hero>(){
                    BattleManager.Instance.HeroesInBattle[0],
                    BattleManager.Instance.HeroesInBattle[1]
                },
                CurrentEnergy = 0,
                CurrentMergeHero = null,
                IsMerge = false
            };
            BattleManager.Instance.HeroesInBattle[0].MergeGroupIndex = 0;
            BattleManager.Instance.HeroesInBattle[1].MergeGroupIndex = 0;
            
            BattleManager.Instance.HeroesInBattle[0].FighterSkillCaster.OnCastSkill += OnCastSkill;
            BattleManager.Instance.HeroesInBattle[1].FighterSkillCaster.OnCastSkill += OnCastSkill;
            
            BattleManager.Instance.HeroesInBattle[0].OnDead += OnFighterDead;
            BattleManager.Instance.HeroesInBattle[1].OnDead += OnFighterDead;
        }*/
        
        // Tactic Version
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.FighterSkillCaster.OnCastSkill += OnCastSkill;
        }
    }

    /*private void OnFighterDead(Fighter fighter) {
        if (fighter is not Hero hero) return;
        HeroMergeGroupData data = HeroMergeGroup[hero.MergeGroupIndex];
        this.HeroMergeGroup[hero.MergeGroupIndex] = null;
        
        data.MergeHeroes[0].FighterSkillCaster.OnCastSkill -= OnCastSkill;
        data.MergeHeroes[1].FighterSkillCaster.OnCastSkill -= OnCastSkill;
        
        data.MergeHeroes[0].OnDead -= OnFighterDead;
        data.MergeHeroes[1].OnDead -= OnFighterDead;

        data.MergeHeroes[0].MergeGroupIndex = -1;
        data.MergeHeroes[1].MergeGroupIndex = -1;
    }*/

    private void OnCastSkill(Fighter fighter) {
        // Hero Merge Version
        /*if (fighter is not Hero hero 
            ||  hero.MergeGroupIndex < 0 || hero.MergeGroupIndex >= this.HeroMergeGroup.Length) return;
        HeroMergeGroupData data = this.HeroMergeGroup[hero.MergeGroupIndex];
        if (data == null || data.IsMerge) return;
        
        data.CurrentEnergy += 1;
        if (data.CurrentEnergy >= this.MergeEnergy) {
            // MergeHero(data);
            MergeHeroTacticVersion(data);
        }*/
    
        if (fighter is not Hero hero) return;
        if (hero.IsMerge) return;
        if (this.HeroEnergies.TryGetValue(hero, out float energy) && energy >= this.MergeEnergy) return;
        if (!this.HeroEnergies.TryAdd(hero, 1)) {
            this.HeroEnergies[hero] += 1;
        }
        float value = this.HeroEnergies[hero] / this.MergeEnergy;
        BattleUIManager.Instance.heroPortraitUI.SetHeroEnergy(hero, value);
    }

    public void MergeHeroTacticVersion(Hero hero1, Hero hero2, BattleTacticType tactic) {
        if (!hero1 || !hero2) return;
        if (hero1.IsMerge || hero2.IsMerge) return;

        if (!this.HeroEnergies.TryGetValue(hero1, out float hero1E) ||
            !this.HeroEnergies.TryGetValue(hero2, out float hero2E) || 
            hero1E < this.MergeEnergy ||
            hero2E < this.MergeEnergy) return;

        StartCoroutine(MergeHeroTacticVersionCoroutine(hero1, hero2, tactic));
    }

    private IEnumerator MergeHeroTacticVersionCoroutine(Hero hero1, Hero hero2, BattleTacticType tacticType) {
        
        BattleTactic tactic = BattleTacticFactory.CreateBattleTactic(tacticType);
        if(tactic == null) yield break;
        
        hero1.IsMerge = true;
        hero2.IsMerge = true;

        this.HeroEnergies[hero1] = 0;
        this.HeroEnergies[hero2] = 0;
        BattleUIManager.Instance.heroPortraitUI.SetHeroEnergy(hero1, 0);
        BattleUIManager.Instance.heroPortraitUI.SetHeroEnergy(hero2, 0);
        
        tactic.CastTactic(hero1, hero2);
        
        // 设置主动技能升级
        // if(groupData.MergeHeroes[0]) groupData.MergeHeroes[0].SkillChange(true);
        // if(groupData.MergeHeroes[1]) groupData.MergeHeroes[1].SkillChange(true);
        
        yield return this.HeroMergeTimer;
        
        // 取消主动技能升级
        // if(groupData.MergeHeroes[0]) groupData.MergeHeroes[0].SkillChange(false);
        // if(groupData.MergeHeroes[1]) groupData.MergeHeroes[1].SkillChange(false);
        
        tactic.StopTactic(hero1, hero2);

        hero1.IsMerge = false;
        hero2.IsMerge = false;
    }

    private void MergeHero(HeroMergeGroupData groupData) {
        if (groupData.IsMerge) return;
        groupData.IsMerge = true;
        StartCoroutine(MergeHeroCoroutine(groupData));
    }

    private IEnumerator MergeHeroCoroutine(HeroMergeGroupData groupData) {
        StartMerge(groupData);
        yield return this.HeroMergeTimer;
        CancelMerge(groupData);
    }

    private void StartMerge(HeroMergeGroupData groupData) {
        Hero firstHero = groupData.MergeHeroes[0];
        Hero secondHero = groupData.MergeHeroes[1];
        
        Hero mergeHero = Instantiate(firstHero);
        FighterData data = Instantiate(firstHero.InitialData);
        data.PhysicsAttack = secondHero.InitialData.PhysicsAttack;
        data.MagicAttack = secondHero.InitialData.MagicAttack;
        data.Shield = 0;
        mergeHero.SetMergeData(data);

        GameObject otherHeroRenderer = Instantiate(secondHero.Renderer.gameObject, mergeHero.Renderer.gameObject.transform);
        otherHeroRenderer.transform.localPosition = new Vector3(-3.0f, 0.0f, 0.0f);
        mergeHero.SetMergeSkill(secondHero.HeroUpdateSkillCaster);
        // All Property Increase 200%
        if (HeroMergeBuff) BuffManager.Instance.AddBuff(mergeHero, mergeHero, HeroMergeBuff);
        groupData.CurrentMergeHero = mergeHero;
        groupData.CurrentMergeHero.OnDead += _ => CancelMerge(groupData);
        foreach (Hero hero in groupData.MergeHeroes) {
            if (!hero) continue;
            hero.FighterIdle();
            hero.Move.StopMove();
            hero.TransitionShow(false);
            BattleManager.Instance.HeroesInBattle.Remove(hero);
            hero.OnDisappear?.Invoke();
        }
        BattleManager.Instance.HeroesInBattle.Add(mergeHero);
        mergeHero.BattleStart(true);
    }

    private void CancelMerge(HeroMergeGroupData groupData) {
        if (!groupData.CurrentMergeHero) return;
        groupData.CurrentMergeHero.gameObject.SetActive(false);
        if (!groupData.CurrentMergeHero.IsDead) {
            groupData.CurrentMergeHero.FighterDead();
            return;
        }

        Vector3 offset = Vector3.zero;
        foreach (Hero hero in groupData.MergeHeroes) {
            if (!hero) continue;
            hero.transform.position = groupData.CurrentMergeHero.transform.position + offset;
            offset += Vector3.left * 3.0f; 
            hero.BattleStart();
            hero.TransitionShow(true);
            BattleManager.Instance.HeroesInBattle.Add(hero);
        }
        groupData.CurrentMergeHero = null;
        groupData.IsMerge = false;
        groupData.CurrentEnergy = 0;
    }
}


