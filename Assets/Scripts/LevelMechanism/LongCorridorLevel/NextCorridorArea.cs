
using System;
using System.Collections;
using UnityEngine;

public enum NextCorridorMoveWay {
    Disappear,
    HeroMove,
    AreaMove
}

public class NextCorridorArea : MonoBehaviour {

    [SerializeField] private Transform Edge;
    [SerializeField] private Transform CurrentAreaEnemyParent;
    [SerializeField] private Transform GroundParent;
    [SerializeField] private NextCorridorArea NextArea;
    [SerializeField] private Transform StartAreaRef;
    [SerializeField] private NextCorridorMoveWay MoveWay = NextCorridorMoveWay.Disappear;
    
    [Header("Move Area")]
    [SerializeField] private MoveArea MoveArea;
    [SerializeField] private float MoveDuration = 2.0f;
    
    [Header("Ground Disappear")]
    [SerializeField] private float DisappearDuration = 1.0f;
    [SerializeField] private float DisappearDistance = 3.0f;
    
    private bool IsActive;
    private bool IsEnd;
    private Vector3 Offset = Vector3.zero;
    private WaitForSecondsRealtime DisappearWaitTimer = new(0.25f);

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
        
        if (this.MoveWay != NextCorridorMoveWay.Disappear) {
            if (MoveCam) {
                MoveCam.MoveToInXDir(this.Offset.x);
                MoveCam.OnArrive += OnCameraArrive;
            }
        }else{
            StartCoroutine(DisappearAreaCoroutine());
        }
    }

    private IEnumerator DisappearAreaCoroutine() {
        // Move Fighter To Here Like Start Formation
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.Move.StartMove();
            if (hero.FighterSkillCaster is SummonSkillCaster summonSkill) {
                summonSkill.ClearPet();
            }
        }
        yield return this.DisappearWaitTimer;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            Vector3 targetPos = this.MoveArea.GetWorldPosition(hero.Name);
            hero.Move.OnArriveDestination += OnArriveDestination_Disappear;
            hero.Move.MoveTo(targetPos);
        }
    }

    private void OnArriveDestination_Disappear(Fighter fighter) {
        fighter.Move.OnArriveDestination -= OnArriveDestination;
        fighter.transform.parent = this.MoveArea.transform;
        fighter.FighterIdle();
        fighter.Move.StopMove();
        
        ArriveHeroCount += 1;
        if (ArriveHeroCount == BattleManager.Instance.HeroesInBattle.Count) {
            StartCoroutine(GroundDisappearCoroutine());
        }
    }

    private IEnumerator GroundDisappearCoroutine() {
        Vector3 startPos = this.GroundParent.position;
        Vector3 endPos = this.GroundParent.position - new Vector3(0.0f, this.DisappearDistance, 0.0f);
        for (float t = 0.0f; t <= DisappearDuration; t += Time.deltaTime) {
            this.GroundParent.position = Vector3.Lerp(startPos, endPos, t / DisappearDuration);
            yield return null;
        }
        this.GroundParent.position = endPos;
        StartCoroutine(MoveAreaCoroutine_Disappear());
    }

    private IEnumerator MoveAreaCoroutine_Disappear() {
        Vector3 startPos = this.MoveArea.transform.position;
        for (float t = 0.0f; t <= this.MoveDuration; t += Time.deltaTime) {
            this.MoveArea.transform.position = Vector3.Lerp(startPos, this.transform.position, t / this.MoveDuration);
            yield return null;
        }
        this.MoveArea.transform.position = this.transform.position;
        NextArea.Active();
    }

    private void OnCameraArrive() {
        MoveCam.OnArrive -= OnCameraArrive;
        if (MoveWay == NextCorridorMoveWay.HeroMove) {
            StartCoroutine(NextAreaCoroutine());    
        }else if (MoveWay == NextCorridorMoveWay.AreaMove) {
            StartCoroutine(MoveAreaCoroutine());
        }
    }

    private IEnumerator MoveAreaCoroutine() {
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.transform.parent = this.MoveArea.transform;
            hero.transform.localPosition = this.MoveArea.GetLocalPosition(hero.Name);
        }

        this.MoveArea.transform.position = this.Edge.position - new Vector3(15.0f, 0.0f, 0.0f);
        Vector3 startPos = this.MoveArea.transform.position;
        for (float t = 0.0f; t <= this.MoveDuration; t += Time.deltaTime) {
            this.MoveArea.transform.position = Vector3.Lerp(startPos, this.transform.position, t / this.MoveDuration);
            yield return null;
        }

        this.MoveArea.transform.position = this.transform.position;
        NextArea.Active();
    }

    private IEnumerator NextAreaCoroutine() {
        // Move Fighter To Here Like Start Formation
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.transform.position = Edge.transform.position - new Vector3(10.0f, 0.0f, 0.0f);
            hero.Move.StartMove();
        }
        yield return null;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            hero.Move.ChangeSpeed(10.0f);
            hero.Move.MoveTo(hero.StartPosition + this.Offset);
            hero.Move.OnArriveDestination += OnArriveDestination;
        }
    }

    private void OnArriveDestination(Fighter fighter) {
        fighter.Move.OnArriveDestination -= OnArriveDestination;
        fighter.FighterIdle();
        fighter.Move.ChangeSpeed(fighter.Speed);
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
        BattleManager.Instance.StartBattleInRound();
    }
}

