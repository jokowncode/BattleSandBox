
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class HeroWarehousePanelWithGoods : MonoBehaviour {

    [SerializeField] private GoodsType OpenGoodsWarehouseType;
    [SerializeField] private GameObject RightPanel;
    
    [Header("Button Image")]
    [SerializeField] private Image AddImage;
    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private Sprite ExitSprite;

    [Header("Button Text")] 
    [SerializeField] private TextMeshProUGUI AddText;
    [SerializeField] private string NormalText;
    [SerializeField] private string ExitText;
    
    private ModeHeroWarehouseUI ParentPanel;
    private bool IsOpenGoodsWarehouse = false;
    
    protected virtual void Awake() {
        this.ParentPanel = this.GetComponentInParent<ModeHeroWarehouseUI>();
    }

    protected void TransitionGoodsWarehouse() {
        if (!this.IsOpenGoodsWarehouse) {
            this.OpenGoodsWarehouse();
        } else {
            this.GoBackToNormal();
        }
    }

    protected void GoBackToNormal() {
        this.IsOpenGoodsWarehouse = false;
        this.RightPanel.gameObject.SetActive(true);
        this.ParentPanel.HideGoodsWarehouse();
        if(this.AddImage) this.AddImage.sprite = this.NormalSprite;
        if (this.AddText) this.AddText.text = this.NormalText;
    }

    private void OpenGoodsWarehouse() {
        this.IsOpenGoodsWarehouse = true;
        this.ParentPanel.ShowGoodsWarehouse(this.OpenGoodsWarehouseType);
        this.RightPanel.gameObject.SetActive(false);
        if(this.AddImage) this.AddImage.sprite = this.ExitSprite;
        if (this.AddText) this.AddText.text = this.ExitText;
    }

}



