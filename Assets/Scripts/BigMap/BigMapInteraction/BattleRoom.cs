
using UnityEngine;

public class BattleRoom : InteractionObject{

    [SerializeField] protected BattleData Data;
    [SerializeField] private Transform Enemies;
    [SerializeField] private Dialogue NextActiveDialogue;
    
    private BoxCollider Collider;
    
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

    protected override void Update(){
        base.Update();
        if (!this.IsEnd && this.Collider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.Collider);
        } 
    }

    protected override void OnTriggerEnter(Collider other){
        base.OnTriggerEnter(other);
        if (this.IsEnd || (GameManager.Instance.IsBattleEnd && GameManager.Instance.IsBattleVictory)){
            if (!this.IsEnd) {
                if (NextActiveDialogue) {
                    NextActiveDialogue.Activate();
                }
                // this.IsEnd = true;
                this.EndInteraction();
            }
            this.InAreaPlayer.TransitionInteractionTip(false);
            this.enabled = false;
            if(this.Enemies) Destroy(this.Enemies.gameObject);
            return;
        }
    }

    protected override void OnTriggerExit(Collider other){
        base.OnTriggerExit(other);
        GameManager.Instance.ResetBattleFlag();
    }
}

