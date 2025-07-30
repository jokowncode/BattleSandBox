using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWarehouseManager : MonoBehaviour
{
    private List<string> OwnedSkills;
    
    public static SkillWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // TODO
    public void AddHero(string heroRef) { }
    public void RemoveHero(string heroRef) { }

    public List<string> GetOwnedHeroes() { return this.OwnedSkills; }
}
