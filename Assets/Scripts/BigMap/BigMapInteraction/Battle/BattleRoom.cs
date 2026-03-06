
using System;
using System.Collections;
using UnityEngine;

public class BattleRoom : InteractionObject {

    [SerializeField] protected BattleData Data;
    [SerializeField] private Transform Enemies;
    [SerializeField] private bool IsEnemyMove = false;
    
    [SerializeField] private bool IsDefeatGameOver = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsDisappearAfterBattle = false;

    public Action OnVictory;
    public Action OnDefeat;
    
    protected BoxCollider Collider;

    private bool EnemyMove => this.IsEnemyMove && this.Enemies && this.Enemies.gameObject.activeSelf;
    private bool IsInteract = false;

    private int InitialLayer;

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.战斗;
    }

    protected override void Awake(){
        base.Awake();
        this.Collider = this.GetComponent<BoxCollider>();
        this.InitialLayer = this.gameObject.layer;
    }

    protected override void Interaction(){
        if (this.IsEnd && !this.IsEndCanInteract){
            return;
        }
        if (this.IsInteract) return;
        this.IsInteract = true;
        SaveDataManager.Instance.PlayerInBigMap.TransMove(false);
        GameManager.Instance.GoToBattle(this.Data);
    }

    protected override void LoadBigMapData() {
        if(this.IsEnd && this.Enemies) Destroy(this.Enemies.gameObject);
        if (!this.IsActive && this.Enemies) {
            this.Enemies.gameObject.SetActive(false);
        }

        if (this.IsEnd && this.IsDisappearAfterBattle) {
            this.gameObject.SetActive(false);
        }

        if (!this.IsActive) {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void Activate() {
        base.Activate();
        if (this.Enemies) {
            this.Enemies.gameObject.SetActive(true);
        }
        this.gameObject.layer = this.InitialLayer;
    }

    protected override void Update(){
        if (!this.EnemyMove) {
            base.Update();
        }

        if (!this.IsEnd && this.Collider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.Collider);
        } 
    }

    protected override bool EnableInteractionCondition() {
        return !this.EnemyMove && !this.IsForce &&
               (!GameManager.Instance.IsBattleEnd || 
                (!GameManager.Instance.IsBattleVictory && !this.IsDefeatGameOver));
    }

    protected override void PlayerEnter() {
        if (this.EnemyMove) {
            this.enabled = true;
            foreach (Transform child in this.Enemies) {
                if (child.TryGetComponent(out BigMapEnemy enemy)) {
                    enemy.ChasePlayer(this.InAreaPlayer, this.Interaction);
                }
            }
        }

        if (GameManager.Instance.IsBattleEnd){
            if (GameManager.Instance.IsBattleVictory) {
                if(this.Enemies) Destroy(this.Enemies.gameObject);
                OnVictory?.Invoke();
                if (!this.IsEnd) {
                    // this.IsEnd = true;
                    this.EndInteraction();
                    if (this.IsDisappearAfterBattle) {
                        this.gameObject.SetActive(false);
                    }
                }
            } else {
                OnDefeat?.Invoke();
                if (IsDefeatGameOver) {
                    GameManager.Instance.DungeonFail();
                }
            }
        }else if (this.IsForce) {
            OnInteractionPre?.Invoke();
            this.Interaction();
        }
        GameManager.Instance.ResetBattleFlag();
    }
}

