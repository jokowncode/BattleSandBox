
using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    private float StartX;

    private void Awake() {
        StartX = this.transform.position.x;
    }

    private void LateUpdate() {
        if (!BattleManager.Instance.IsBattleStart) return;
        List<Hero> heroes =  BattleManager.Instance.HeroesInBattle;
        if (heroes == null || heroes.Count == 0) return;
        
        float x = 0.0f;
        foreach (Hero hero in heroes) {
            x += hero.transform.position.x;
        }

        Vector3 position = this.transform.position;
        position.x = Mathf.Max(StartX, x / heroes.Count);
        this.transform.position = position;
    }
}

