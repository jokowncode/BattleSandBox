
using System;
using TMPro;
using UnityEngine;

public enum PlayerInAreaColliderDir {
    Both,
    Left,
    Right
}

public class Player : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI InteractionTip;

    private PlayerMove Move;

    private void Awake(){
        Move = GetComponent<PlayerMove>();
    }

    public void TransitionInteractionTip(bool show, string interactionObjName){
        InteractionTip.transform.parent.gameObject.SetActive(show);
        InteractionTip.text = $"E {interactionObjName}";
    }

    public void SetCollider(BoxCollider inAreaCollider, PlayerInAreaColliderDir dir = PlayerInAreaColliderDir.Both){
        Move.SetInAreaCollider(inAreaCollider, dir);
    }

    public void TransMove(bool canMove){
        this.Move.enabled = canMove;
    }
}

