
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

    public PlayerMove Move { get; private set; }

    private void Awake(){
        Move = GetComponent<PlayerMove>();
    }

    public void TransitionInteractionTip(bool show, string interactionObjName, bool showE = true){
        InteractionTip.transform.parent.gameObject.SetActive(show);
        string text = showE ? $"E {interactionObjName}" : interactionObjName;
        InteractionTip.text = text;
    }

    public void SetCollider(BoxCollider inAreaCollider, PlayerInAreaColliderDir dir = PlayerInAreaColliderDir.Both){
        Move.SetInAreaCollider(inAreaCollider, dir);
    }

    public void TransMove(bool canMove){
        this.Move.enabled = canMove;
    }
}

