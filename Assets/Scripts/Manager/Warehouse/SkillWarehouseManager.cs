using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWarehouseManager : MonoBehaviour
{
    [SerializeField]private List<SkillData> OwnedSkills;
    
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

    public List<SkillData> GetOwnedHeroes() { return this.OwnedSkills; }
}
