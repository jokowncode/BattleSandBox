
using System;
using UnityEngine;

public class BattleRoom : InteractionObject {

    [SerializeField] protected BattleData Data;
    [SerializeField] private Transform Enemies;
    
    [SerializeField] private bool IsDefeatGameOver = true;

    public Action OnVictory;
    public Action OnDefeat;
    
    private BoxCollider Collider;

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.Battle;
    }

    protected override void Awake(){
        base.Awake();
        this.Collider = this.GetComponent<BoxCollider>();
    }

    protected override void Interaction(){
        if (this.IsEnd){
            return;
        }
        GameManager.Instance.GoToBattle(this.Data);
    }

    protected override void LoadBigMapData() {
        if(this.IsEnd && this.Enemies) Destroy(this.Enemies.gameObject);
        if (!this.IsActive && this.Enemies) {
            this.Enemies.gameObject.SetActive(false);
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
                }
            } else {
                OnDefeat?.Invoke();
                if (IsDefeatGameOver) {
                    GameManager.Instance.DungeonFail();
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other){
        base.OnTriggerExit(other);
        GameManager.Instance.ResetBattleFlag();
    }
}

