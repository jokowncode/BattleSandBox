

using System;
using System.Collections.Generic;
using UnityEngine;

public class GoodsWarehousePanel : MonoBehaviour {

    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private DetailButton GoodsButtonPrefab;
    [SerializeField] private CategoryList CategoryUI;
    
    public Func<string, bool> OnClickGoods;
    private GoodsType CurrentShowGoodsType = GoodsType.None;

    private void Awake() {
        if (this.CategoryUI) {
            this.CategoryUI.OnCategoryClicked += OnCategoryClicked;
        }
    }

    private void OnCategoryClicked(string cName, int index) {
        GoodsType type = GoodsType.None;
        if (cName == "词条") {
            type = GoodsType.普通词条;
        } else {
            Enum.TryParse(cName, true, out type);
        }

        if (type == GoodsType.None) return;
        Show(type, false);
    }

    public void Show(GoodsType type, bool canUse) {
        if (type != this.CurrentShowGoodsType) {
            this.CurrentShowGoodsType = type;
            foreach (Transform child in GoodsContainer) {
                Destroy(child.gameObject);
            }

            List<GoodsData> goods = GoodsWarehouseManager.Instance.GetGoodsByType(type);
            foreach (GoodsData data in goods) {
                DetailButton button = Instantiate(GoodsButtonPrefab, GoodsContainer);
                button.SetData("", data.Name, data.GoodsCount, canUse, data.Type);
                button.OnButtonClicked += OnButtonClicked;
            }
        }
        this.gameObject.SetActive(true);
    }

    private void OnButtonClicked(string gName, int currentCount) {
        bool? result = OnClickGoods?.Invoke(gName);
        if (result != null && result.Value) {
            foreach (Transform child in this.GoodsContainer) {
                if (child.TryGetComponent(out DetailButton button) && button.Name == gName) {
                    button.SetCount(currentCount - 1);
                }
            }
        }
    }

    public void Hide() {
        this.CurrentShowGoodsType = GoodsType.None;
        this.gameObject.SetActive(false);
    }

    public void Show() {
        if (!CategoryUI) return;
        CategoryUI.SelectCategory(0, true);
    }
}




