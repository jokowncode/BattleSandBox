
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EntanglementManager : MonoBehaviour {

    [SerializeField] private List<EntanglementData> EntanglementLevelDatas;
    
    public static EntanglementManager Instance;

    private List<float> HeroEntanglementValues;
    public List<string> AllBattleTacticDescs { get; private set; } = new List<string>();

    private float MinHasTacticEntangleValue = float.MaxValue;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;

        Array battleTacticArray = Enum.GetValues(typeof(BattleTacticType));
        foreach (object tactic in battleTacticArray) {
            if ((int)tactic < 0) continue;
            BattleTacticType current = (BattleTacticType) tactic;
            this.AllBattleTacticDescs.Add(BattleTacticFactory.GetBattleTacticDescription(current));
        }

        foreach (EntanglementData data in EntanglementLevelDatas) {
            if (data.CanUseMaxBattleTactic != BattleTacticType.None && data.Value < this.MinHasTacticEntangleValue) {
                this.MinHasTacticEntangleValue = data.Value;
            }
        }
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
            this.HeroEntanglementValues = new List<float>();
            int count = HeroWarehouseManager.Instance.TotalHeroCount;
            count = count * (count - 1) / 2;
            for (int i = 0; i < count; i++) { this.HeroEntanglementValues.Add(0); }
        }
        
        /*int index1 = HeroWarehouseManager.Instance.GetHeroIndex("Elara");
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex("Bullock");
        int index3 = HeroWarehouseManager.Instance.GetHeroIndex("Dr.Entro");
        int index = GetHeroEntanglementIndex(index1, index2);
        int indexo = GetHeroEntanglementIndex(index2, index3);
        if (this.HeroEntanglementValues == null || this.HeroEntanglementValues[index] == 0.0f) {
            this.HeroEntanglementValues[index] = 35.0f;
            this.HeroEntanglementValues[indexo] = 35.0f;
        }*/
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
        if (BattleManager.Instance.IsBattleStart) return;
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

    public void AddEntanglementValue(string hero1, string hero2, float value) {
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero1);
        if (index1 < 0) return;
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(hero2);
        if (index2 < 0) return;
        int index = GetHeroEntanglementIndex(index1, index2);
        this.HeroEntanglementValues[index] += value;
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

}


