
using System;
using UnityEngine;

public class BattleRoom : InteractionObject {

    [SerializeField] protected BattleData Data;
    [SerializeField] private Transform Enemies;
    
    [SerializeField] private bool IsDefeatGameOver = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsDisappearAfterBattle = false;

    public Action OnVictory;
    public Action OnDefeat;
    
    protected BoxCollider Collider;

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.战斗;
    }

    protected override void Awake(){
        base.Awake();
        this.Collider = this.GetComponent<BoxCollider>();
    }

    protected override void Interaction(){
        if (this.IsEnd && !this.IsEndCanInteract){
            return;
        }
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
    }

    public override void Activate() {
        base.Activate();
        if (this.Enemies) {
            this.Enemies.gameObject.SetActive(true);
        }
    }

    protected override void Update(){
        base.Update();
        if (!this.IsEnd && this.Collider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.Collider);
        } 
        
        // TODO: MYSTERIOUS BUG
        this.EnableInteraction(true);
    }

    protected override void PlayerEnter() {
        if (GameManager.Instance.IsBattleEnd){
            if (GameManager.Instance.IsBattleVictory) {
                this.EnableInteraction(false);
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

