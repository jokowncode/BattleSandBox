
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public struct BondData {
    public int BondLevel;
    public float CurrentValue;
    public float CurrentLevelValue;
    public float NextLevelValue;
}

public class EntanglementManager : MonoBehaviour {

    [SerializeField] private List<EntanglementData> EntanglementLevelDatas;
    
    public static EntanglementManager Instance;

    private List<float> HeroEntanglementValues;

    private float MinHasTacticEntangleValue = float.MaxValue;
    public int MaxLevel { get; private set; }
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        foreach (EntanglementData data in EntanglementLevelDatas) {
            if (data.CanUseMaxBattleTactic != BattleTacticType.None && data.Value < this.MinHasTacticEntangleValue) {
                this.MinHasTacticEntangleValue = data.Value;
            }
        }
        this.MaxLevel = this.EntanglementLevelDatas.Count;
    }

    private int GetHeroEntanglementIndex(int index1, int index2) {
        int minIndex = index1 < index2 ? index1 : index2;
        int maxIndex = index1 < index2 ? index2 : index1;

        int heroCount = HeroWarehouseManager.Instance.TotalHeroCount;

        int index = 0;
        if (minIndex != 0) {
            int start = heroCount - 1;
            int end = heroCount - minIndex;
            index += (start + end) * minIndex / 2;
        }

        index += maxIndex - (minIndex + 1);
        return index;
    }

    private float GetHeroEntanglementValue(int index1, int index2) {
        int index = GetHeroEntanglementIndex(index1, index2);
        return this.HeroEntanglementValues[index];
    }

    private void LoadHeroEntanglement() {
        this.HeroEntanglementValues = SaveDataManager.Instance.PlayerData.HeroEntanglementValues;
        if (this.HeroEntanglementValues.Count == 0) {
            int count = HeroWarehouseManager.Instance.TotalHeroCount;
            count = count * (count - 1) / 2;
            for (int i = 0; i < count; i++) { this.HeroEntanglementValues.Add(0); }
        }
        
        // TODO: TEMP
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex("Elara");
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex("Bullock");
        int index3 = HeroWarehouseManager.Instance.GetHeroIndex("Dr.Entro");
        int index = GetHeroEntanglementIndex(index1, index2);
        int indexo = GetHeroEntanglementIndex(index2, index3);
        if (this.HeroEntanglementValues == null || this.HeroEntanglementValues[index] == 0.0f) {
            this.HeroEntanglementValues[index] = 35.0f;
            this.HeroEntanglementValues[indexo] = 25.0f;
        }
    }

    private void Start() {
        SaveDataManager.Instance.OnLoadData += LoadHeroEntanglement;
    }

#if TEST_BATTLE
    public void TEMPFORBATTLE() {
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero1);
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero2);
        int index = GetHeroEntanglementIndex(index1, index2);
        if (this.HeroEntanglementValues == null || this.HeroEntanglementValues[index] == 0.0f) {
            this.LoadHeroEntanglement();
        }
    }
#endif
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneTools.IsBattleScene((SceneType)scene.buildIndex)) {
            BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterTheField;
            BattleManager.Instance.OnHeroExitTheField += OnHeroExitTheField;
        }
    }

    private void PropertyChange(Hero hero, bool isUp) {

        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero.Name);
        if (index1 == -1) return;

        foreach (Hero h in BattleManager.Instance.HeroesInBattle) {
            if (h == hero) continue;

            int index2 = HeroWarehouseManager.Instance.GetHeroIndex(h.Name);
            if (index2 == -1) continue;
            float value = GetHeroEntanglementValue(index1, index2);

            foreach (EntanglementData data in this.EntanglementLevelDatas) {
                if (value < data.Value) break;
                if (!data.PropertyChange) continue;

                hero.FighterPropertyChange(data.ChangeProperty, data.ChangeProperty, data.ModifyWay,
                    PropertyRef.Initial, data.ChangeValue, isUp);
                h.FighterPropertyChange(data.ChangeProperty, data.ChangeProperty, data.ModifyWay, PropertyRef.Initial,
                    data.ChangeValue, isUp);
            }
        }
    }

    private void OnHeroExitTheField(Hero hero) {
        PropertyChange(hero, false);
    }

    private void OnHeroEnterTheField(Hero hero) {
        if (BattleManager.Instance.IsBattleStart) return;
        PropertyChange(hero, true);
    }

    public BattleTacticType GetEntangleHeroCanCastMaxBattleTactic(string hero1, string hero2) {
        BattleTacticType maxCanCastBattleTactic = BattleTacticType.None;
        
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero1);
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(hero2);
        if (index1 == -1 || index2 == -1) return maxCanCastBattleTactic;
        
        float entangleValue = GetHeroEntanglementValue(index1, index2);
        foreach (EntanglementData data in this.EntanglementLevelDatas) {
            if (entangleValue < data.Value) break;
            maxCanCastBattleTactic = data.CanUseMaxBattleTactic;
        }
        return maxCanCastBattleTactic;
    }

    public bool AddEntanglementValue(string hero1, string hero2, float value) {
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero1);
        if (index1 < 0) return false;
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(hero2);
        if (index2 < 0) return false;
        int index = GetHeroEntanglementIndex(index1, index2);
        if (GetCurrentLevel(this.HeroEntanglementValues[index]) >= this.MaxLevel) return false;
        this.HeroEntanglementValues[index] += value;
        return true;
    }

    public List<string> GetHasTacticHeroNames(string heroName) {
        List<string> result = new List<string>();
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(heroName);
        if (index1 < 0) return result;

        List<string> currentHeroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string otherHeroName in currentHeroes) {
            int index2 = HeroWarehouseManager.Instance.GetHeroIndex(otherHeroName);
            if (index2 < 0) continue;
            if (index1 == index2) continue;

            float value = GetHeroEntanglementValue(index1, index2);
            if (value >= this.MinHasTacticEntangleValue) {
                result.Add(otherHeroName);
            }
        }
        return result;
    }

    private int GetCurrentLevel(float currentValue) {
        int index = 0;
        while (index < this.EntanglementLevelDatas.Count 
               && currentValue >= this.EntanglementLevelDatas[index].Value) {
            index += 1;
        }
        return index;
    }

    public BondData GetBondData(string hero1, string hero2) {
        BondData result = new BondData();
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero1);
        if (index1 < 0) return result;
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(hero2);
        if (index2 < 0) return result;
        result.CurrentValue = GetHeroEntanglementValue(index1, index2);
        
        int level = GetCurrentLevel(result.CurrentValue);
        result.BondLevel = level;
        result.NextLevelValue = level == this.EntanglementLevelDatas.Count ? result.CurrentValue : this.EntanglementLevelDatas[level].Value;
        result.CurrentLevelValue = level == 0 ? 0.0f : this.EntanglementLevelDatas[level - 1].Value;
        return result;
    }

}


