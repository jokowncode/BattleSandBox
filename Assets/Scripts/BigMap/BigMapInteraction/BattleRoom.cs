
using UnityEngine;

public class BattleRoom : InteractionObject{

    [SerializeField] private BattleData Data;

    private BoxCollider Collider;
    private bool IsEnd;
    
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
            this.IsEnd = true;
            this.InAreaPlayer.TransitionInteractionTip(false);
            return;
        }
    }

    protected override void OnTriggerExit(Collider other){
        base.OnTriggerExit(other);
        GameManager.Instance.ResetBattleFlag();
    }
}

