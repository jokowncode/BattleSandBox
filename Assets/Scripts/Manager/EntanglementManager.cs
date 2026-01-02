
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EntanglementManager : MonoBehaviour {

    [SerializeField] private List<EntanglementData> EntanglementLevelDatas;

    [Header("Debug")]  // TODO: TEMP -> FOR DEBUG
    [SerializeField] private string EntanglementHero1 = "Elara";
    [SerializeField] private string EntanglementHero2 = "Bullock";
    [SerializeField] private float EntanglementValue = 30.0f;
    
    public static EntanglementManager Instance;

    private List<float> HeroEntanglementValues;
    private List<string> AllBattleTacticDescs = new List<string>();

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        Array battleTacticArray = Enum.GetValues(typeof(BattleTacticType));
        foreach (object tactic in battleTacticArray) {
            if ((int)tactic < 0) continue;
            BattleTacticType current = (BattleTacticType) tactic;
            this.AllBattleTacticDescs.Add(BattleTacticFactory.GetBattleTacticDescription(current));
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
        if (PlayerPrefs.HasKey("HeroEntanglementValues")) {
            this.HeroEntanglementValues = JsonUtility
                .FromJson<Serialization<float>>(PlayerPrefs.GetString("HeroEntanglementValues")).ToList();
        } else {
            this.HeroEntanglementValues = new List<float>();
            int count = HeroWarehouseManager.Instance.TotalHeroCount;
            count = count * (count - 1) / 2;
            for (int i = 0; i < count; i++) { this.HeroEntanglementValues.Add(0); }
        }

        // TODO: TEMP -> FOR DEBUG
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero1);
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero2);
        int index = GetHeroEntanglementIndex(index1, index2);
        this.HeroEntanglementValues[index] = this.EntanglementValue;

        /*BattleTacticType maxCastTactic = GetEntangleHeroCanCastMaxBattleTactic(this.EntanglementHero1, this.EntanglementHero2);
        for (int i = 0; i <= (int)maxCastTactic; i++) {
            Debug.Log(this.AllBattleTacticDescs[i]);
        }*/
    }

    public void TEMPFORBATTLE() {
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero1);
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex(this.EntanglementHero2);
        int index = GetHeroEntanglementIndex(index1, index2);
        if (this.HeroEntanglementValues == null || this.HeroEntanglementValues[index] == 0.0f) {
            this.LoadHeroEntanglement();
        }
    }

    private void Start() {
        SaveMapManager.Instance.OnSaveData += () => {
            string json = JsonUtility.ToJson(new Serialization<float>(this.HeroEntanglementValues));
            PlayerPrefs.SetString("HeroEntanglementValues", json);
        };

        SaveMapManager.Instance.OnLoadData += LoadHeroEntanglement;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == (int)SceneType.Battle) {
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

}


