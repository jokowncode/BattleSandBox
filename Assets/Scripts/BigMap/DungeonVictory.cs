
using System;
using UnityEngine;

public class DungeonVictory : MonoBehaviour {

    private InteractionObject obj;

    private void Awake() {
        obj = this.GetComponent<InteractionObject>();
        if (obj) {
            obj.OnInteractionEnded += () => {
                // TODO: Dungeon Victory Do Something....
                
                GameManager.Instance.DungeonEnd(true);
            };
        }
    }
}


