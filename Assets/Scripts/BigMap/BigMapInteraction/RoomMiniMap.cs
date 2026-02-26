
using UnityEngine;

public class RoomMiniMap : InteractionObject {

    [SerializeField] private SpriteRenderer BackgroundRenderer;
    [SerializeField] private Sprite UnSeenSprite;
    [SerializeField] private Sprite PlayerEnterSprite;

    [Header("Light")] 
    [SerializeField] private GameObject Lights;
    
    protected override void Awake() {
        base.Awake();
        this.IsActive = true;
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.MiniMap;
    }

    protected override void LoadBigMapData() {
        this.SetMiniMap();
    }

    private void SetMiniMap() {
        this.BackgroundRenderer.sprite = this.IsEnd ? this.PlayerEnterSprite : this.UnSeenSprite;
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(this.IsEnd);
        }
        this.Lights.SetActive(this.IsEnd);
    }

    protected override void EnableInteraction(bool enable) { }

    protected override void PlayerEnter() {
        this.EndInteraction();
        this.SetMiniMap();
    }

    protected override void Interaction() { }
}

