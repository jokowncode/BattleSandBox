
using UnityEngine;

public class RandomBattleRoom : BattleRoom {

    [SerializeField] private float Percentage = 0.5f;
    
    protected override void OnTriggerEnter(Collider other) {
        if (Random.value > Percentage) return;
        base.OnTriggerEnter(other);
    }
}


