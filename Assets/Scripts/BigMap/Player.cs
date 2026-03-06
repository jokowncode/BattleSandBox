
using System;
using TMPro;
using UnityEngine;

public enum PlayerInAreaColliderDir {
    Both,
    Left,
    Right
}

public class Player : MonoBehaviour{

    [SerializeField] private InteractionTip InteractionTip;

    public PlayerMove Move { get; private set; }

    private void Awake(){
        Move = GetComponent<PlayerMove>();
    }

    public void TransitionInteractionTip(bool show, string interactionObjName, bool canInteract = true){
        if (show) this.InteractionTip.Show(interactionObjName, canInteract);
        else this.InteractionTip.Hide();
    }

    public void SetCollider(BoxCollider inAreaCollider, PlayerInAreaColliderDir dir = PlayerInAreaColliderDir.Both){
        Move.SetInAreaCollider(inAreaCollider, dir);
    }

    public void TransMove(bool canMove){
        this.Move.enabled = canMove;
    }
}

