
using System;
using System.Collections;
using UnityEngine;

public class NextCorridorArea : MonoBehaviour {

    [SerializeField] private Transform Edge;
    [SerializeField] private Transform CurrentAreaEnemyParent;
    [SerializeField] private NextCorridorArea NextArea;
    [SerializeField] private Transform StartAreaRef;
    
    private bool IsActive;
    private bool IsEnd;
    private Vector3 Offset = Vector3.zero;

    private int ArriveHeroCount = 0;
    private MoveCamera MoveCam;
    
    private void Awake() {
        if (StartAreaRef) {
            this.Offset = this.transform.position - StartAreaRef.position;
        }
    }

    private void Start() {
        BattleManager.Instance.OnEnemyBeClear += OnEnemyBeClear;
        MoveCam = CameraManager.Instance.MainCamera.GetComponent<MoveCamera>();
    }

    private void OnEnemyBeClear() {
        if (!IsActive || IsEnd) return;
        IsEnd = true;
        if (!NextArea) {   // Victory
            BattleManager.Instance.BattleVictory();
            return;
        }

        if (MoveCam) {
            MoveCam.MoveToInXDir(this.Offset.x);
            MoveCam.OnArrive += OnCameraArrive;
        }
    }

    private void OnCameraArrive() {
        MoveCam.OnArrive -= OnCameraArrive;
        StartCoroutine(NextAreaCoroutine());
    }

    private IEnumerator NextAreaCoroutine() {
        // Move Fighter To Here Like Start Formation
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.transform.position = Edge.transform.position - new Vector3(10.0f, 0.0f, 0.0f);
            hero.Move.StartMove();
        }
        yield return null;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.Move.MoveTo(hero.StartPosition + this.Offset);
            hero.Move.OnArriveDestination += OnArriveDestination;
        }
    }

    private void OnArriveDestination(Fighter fighter) {
        fighter.Move.OnArriveDestination -= OnArriveDestination;
        fighter.FighterIdle();
        fighter.Move.StopMove();
        
        ArriveHeroCount += 1;
        if (ArriveHeroCount == BattleManager.Instance.HeroesInBattle.Count) {
            NextArea.Active();
        }
    }

    public void Active() {
        if (IsActive || IsEnd) return;
        IsActive = true;
        IsEnd = false;
        BattleManager.Instance.AddEnemiesInParent(this.CurrentAreaEnemyParent);
        foreach (Enemy enemy in BattleManager.Instance.EnemiesInBattle){
            enemy.BattleStart();
        }
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle){
            hero.BattleStart();
        }
    }
}

