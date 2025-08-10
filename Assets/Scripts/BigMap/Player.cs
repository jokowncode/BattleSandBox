
using System;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private GameObject InteractionTip;

    private PlayerMove Move;

    private void Awake(){
        Move = GetComponent<PlayerMove>();
    }

    public void TransitionInteractionTip(bool show){
        InteractionTip.SetActive(show);
    }

    public void SetCollider(BoxCollider inAreaCollider){
        Move.SetInAreaCollider(inAreaCollider);
    }

    public void TransMove(bool canMove){
        this.Move.enabled = canMove;
    }
}

