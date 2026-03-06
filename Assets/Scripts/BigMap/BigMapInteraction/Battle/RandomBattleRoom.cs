
using UnityEngine;

public class RandomBattleRoom : BattleRoom {

    [SerializeField] private float Percentage = 0.5f;

    protected override bool EnableInteractionCondition() {
        return base.EnableInteractionCondition() && Random.value <= Percentage;
    }
}


