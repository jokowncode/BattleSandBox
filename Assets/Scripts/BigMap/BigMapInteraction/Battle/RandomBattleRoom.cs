
using UnityEngine;

public class RandomBattleRoom : BattleRoom {

    [SerializeField] private float Percentage = 0.5f;

    protected override void PlayerEnter() {
        if (Random.value > Percentage) {
            this.EnableInteraction(false);
            return;
        }

        base.PlayerEnter();
    }
}


