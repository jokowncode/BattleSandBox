
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroMergeManager : MonoBehaviour {

    [SerializeField] private List<PassiveEntry> HeroMergePassiveEntries;
    [SerializeField] private BuffData HeroMergeBuff;
    [SerializeField] private float HeroMergeDuration = 5.0f;
    
    public static HeroMergeManager Instance;

    private Hero[] MergeHeroes = new Hero[2];
    private Hero CurrentMergeHero;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        this.HeroMergeBuff.Duration = HeroMergeDuration;
    }

    private void Start() {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
    }

    private void OnBattleStart() {
        if (BattleManager.Instance.HeroesInBattle.Count >= 2) {
            MergeHeroes[0] = BattleManager.Instance.HeroesInBattle[0];
            MergeHeroes[1] = BattleManager.Instance.HeroesInBattle[1];
        }
    }

    private void Update() {
        if (!BattleManager.Instance.IsBattleStart) return;
        if (Input.GetKeyDown(KeyCode.Space)) {
            MergeHero();
        }
    }

    private void MergeHero() { 
        Hero mergeHero = Instantiate(this.MergeHeroes[0]);
        mergeHero.FighterIdle();
        FighterData data = Instantiate(mergeHero.InitialData);
        data.PhysicsAttack = this.MergeHeroes[1].InitialData.PhysicsAttack;
        data.MagicAttack = this.MergeHeroes[1].InitialData.MagicAttack;
        mergeHero.SetMergeData(data);

        GameObject otherHeroRenderer = Instantiate(this.MergeHeroes[1].HeroRenderer.gameObject, mergeHero.HeroRenderer.gameObject.transform);
        otherHeroRenderer.transform.localPosition = new Vector3(-3.0f, 0.0f, 0.0f);
        mergeHero.SetMergeSkill(this.MergeHeroes[1].HeroUpdateSkillCaster);
        // All Property Increase 200%
        if (HeroMergeBuff) BuffManager.Instance.AddBuff(mergeHero, mergeHero, HeroMergeBuff);
        this.CurrentMergeHero = mergeHero;
        this.CurrentMergeHero.OnDead += CancelMerge;
        foreach (Hero hero in this.MergeHeroes) {
            hero.FighterIdle();
            hero.Move.StopMove();
            hero.TransitionShow(false);
            hero.OnDead?.Invoke();
            BattleManager.Instance.HeroesInBattle.Remove(hero);
        }
        BattleManager.Instance.HeroesInBattle.Add(mergeHero);
        mergeHero.BattleStart(true);
        Invoke(nameof(CancelMerge), this.HeroMergeDuration);
    }

    private void CancelMerge() {
        this.CurrentMergeHero.OnDead -= CancelMerge;
        if (!this.CurrentMergeHero) return;
        Vector3 offset = Vector3.zero;
        foreach (Hero hero in this.MergeHeroes) {
            hero.transform.position = this.CurrentMergeHero.transform.position + offset;
            offset += Vector3.left * 3.0f; 
            hero.BattleStart();
            hero.TransitionShow(true);
            BattleManager.Instance.HeroesInBattle.Add(hero);
        }

        if (!this.CurrentMergeHero.IsDead) {
            this.CurrentMergeHero.OnDead?.Invoke();
            BattleManager.Instance.HeroesInBattle.Remove(this.CurrentMergeHero);
            Destroy(this.CurrentMergeHero.gameObject);
        }
        this.CurrentMergeHero = null;
    }
}


