
using System;
using UnityEngine;

public class BattleRoom : InteractionObject {

    [SerializeField] protected BattleData Data;
    [SerializeField] private Transform Enemies;
    
    [SerializeField] private bool IsDefeatGameOver = true;

    public Action OnVictory;
    public Action OnDefeat;
    
    private BoxCollider Collider;

    protected override string GetName() {
        if (!this.Data) return "UnknownBattleRoom";
        return this.Data.BattleName;
    }

    private void Awake(){
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
    }

    protected override void Update(){
        base.Update();
        if (!this.IsEnd && this.Collider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.Collider);
        } 
    }

    protected override void OnTriggerEnter(Collider other){
        base.OnTriggerEnter(other);
        if (this.IsEnd || GameManager.Instance.IsBattleEnd){
            if (GameManager.Instance.IsBattleVictory) {
                if (!this.IsEnd) {
                    // this.IsEnd = true;
                    this.EndInteraction();
                }
                this.InAreaPlayer.TransitionInteractionTip(false);
                this.enabled = false;
                if(this.Enemies) Destroy(this.Enemies.gameObject);
                OnVictory?.Invoke();
            } else {
                OnDefeat?.Invoke();
                if (IsDefeatGameOver) {
                    PlayerPrefs.DeleteKey("CurrentDungeon");
                    // TODO: Game Over -> Go Back To ???   
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other){
        base.OnTriggerExit(other);
        GameManager.Instance.ResetBattleFlag();
    }
}

