
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class HeroWarehousePanelWithGoods : MonoBehaviour {

    [SerializeField] private GoodsType OpenGoodsWarehouseType;
    [SerializeField] private GameObject RightPanel;
    [SerializeField] private ModeHeroWarehouseUI ParentPanel;
    
    [Header("Button Image")]
    [SerializeField] private Image AddImage;
    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private Sprite ExitSprite;

    [Header("Button Text")] 
    [SerializeField] private TextMeshProUGUI AddText;
    [SerializeField] private string NormalText;
    [SerializeField] private string ExitText;
    
    private bool IsOpenGoodsWarehouse = false;

    protected void TransitionGoodsWarehouse() {
        if (!this.IsOpenGoodsWarehouse) {
            this.OpenGoodsWarehouse();
        } else {
            this.GoBackToNormal();
        }
    }

    public virtual void GoBackToNormal() {
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

    public virtual void Hide() {
        this.GoBackToNormal();
        this.gameObject.SetActive(false);
    }

}



