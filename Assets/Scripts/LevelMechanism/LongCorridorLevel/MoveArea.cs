
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveArea : MonoBehaviour {

    private Dictionary<string, Vector3> HeroLocalPositions;

    private void Awake() {
        HeroLocalPositions = new Dictionary<string, Vector3>();
    }

    private void Start() {
        BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterTheField;
        BattleManager.Instance.OnHeroExitTheField += OnHeroExitTheField;
    }

    private void OnHeroExitTheField(Hero hero) {
        HeroLocalPositions.Remove(hero.Name);
    }

    private void OnHeroEnterTheField(Hero hero) {
        Vector3 localPos = this.transform.InverseTransformPoint(hero.transform.position);
        HeroLocalPositions.Add(hero.Name, localPos);
    }

    public Vector3 GetWorldPosition(string heroName) {
        Vector3 localPos = GetLocalPosition(heroName);
        return this.transform.TransformPoint(localPos);
    }

    public Vector3 GetLocalPosition(string heroName) {
        return HeroLocalPositions.TryGetValue(heroName, out Vector3 position) ? position : Vector3.zero;
    }
}

