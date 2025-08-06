using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryWarehouseManager : MonoBehaviour {
    
    [SerializeField]private List<PassiveEntry> OwnedSkills;
    
    public static PassiveEntryWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public List<PassiveEntry> GetOwnedHeroes() { return this.OwnedSkills; }
}
