
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EntanglementManager : MonoBehaviour {

    [SerializeField] private List<EntanglementData> EntanglementLevelDatas;
    
    public static EntanglementManager Instance;
    
    private List<float> HeroEntanglementValues;
    
    private void Awake() {
        if (Instance != null) { 
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            this.HeroEntanglementValues = JsonUtility.FromJson<Serialization<float>>(
                PlayerPrefs.GetString("HeroEntanglementValues")).ToList();
        } else {
            this.HeroEntanglementValues = new List<float>();
            int count = HeroWarehouseManager.Instance.TotalHeroCount;
            count = count * (count - 1) / 2;
            for (int i = 0; i < count; i++) {
                this.HeroEntanglementValues.Add(0);
            }
        }
        
        // TODO: TEMP -> FOR DEBUG
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex("Elara");
        int index2 = HeroWarehouseManager.Instance.GetHeroIndex("Bullock");
        int index = GetHeroEntanglementIndex(index1, index2);
        this.HeroEntanglementValues[index] = 20.0f;
    }

    private void SaveHeroEntanglement() {
        string json = JsonUtility.ToJson(new Serialization<float>(this.HeroEntanglementValues));
        PlayerPrefs.SetString("HeroEntanglementValues", json);
    }

    private void OnDestroy() {
        // TODO: TEMP
        // this.SaveHeroEntanglement();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == (int)SceneType.Battle) {
            BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterTheField;
            BattleManager.Instance.OnHeroExitTheField += OnHeroExitTheField;

            BattleUIManager.Instance.OnUpdateWarehouse += () => {
                this.LoadHeroEntanglement();
                BattleManager.Instance.LoadHeroDeploy();
            };
        }
    }

    private void PropertyChange(Hero hero, bool isUp) {
        
        int index1 = HeroWarehouseManager.Instance.GetHeroIndex(hero.Name);
        if (index1 == -1) return;
        
        foreach (Hero h in BattleManager.Instance.HeroesInBattle) {
            if (h == hero) continue;
            
            int index2 = HeroWarehouseManager.Instance.GetHeroIndex(h.Name);
            if(index2 == -1) continue;
            float value = GetHeroEntanglementValue(index1, index2);
            
            foreach (EntanglementData data in this.EntanglementLevelDatas) {
                if (value < data.Value) break;
                if (!data.PropertyChange) continue;

                hero.FighterPropertyChange(data.ChangeProperty, data.ChangeProperty, data.ModifyWay,
                    PropertyRef.Initial, data.ChangeValue, isUp);
                h.FighterPropertyChange(data.ChangeProperty, data.ChangeProperty, data.ModifyWay,
                    PropertyRef.Initial, data.ChangeValue, isUp);
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
}


