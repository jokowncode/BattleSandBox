
using System;
using UnityEngine;

public enum PlayerInAreaColliderDir {
    Both,
    Left,
    Right
}

public class Player : MonoBehaviour{

    [SerializeField] private GameObject InteractionTip;

    private PlayerMove Move;

    private void Awake(){
        Move = GetComponent<PlayerMove>();
    }

    public void TransitionInteractionTip(bool show){
        InteractionTip.SetActive(show);
    }

    public void SetCollider(BoxCollider inAreaCollider, PlayerInAreaColliderDir dir = PlayerInAreaColliderDir.Both){
        Move.SetInAreaCollider(inAreaCollider, dir);
    }

    public void TransMove(bool canMove){
        this.Move.enabled = canMove;
    }
}

