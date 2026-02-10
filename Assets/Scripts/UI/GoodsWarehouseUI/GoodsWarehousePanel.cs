

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsWarehousePanel : MonoBehaviour {

    [SerializeField] private GridLayoutGroup Layout;
    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private DetailButton GoodsButtonPrefab;
    [SerializeField] private DetailButton HasDescButtonPrefab;
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
                DetailButton button;
                if (data.Type is GoodsType.战术 or GoodsType.普通词条 or GoodsType.特殊词条) {
                    button = Instantiate(HasDescButtonPrefab, GoodsContainer);
                    this.Layout.cellSize = new Vector2(600, 100);
                } else {
                    button = Instantiate(GoodsButtonPrefab, GoodsContainer);
                    this.Layout.cellSize = new Vector2(300, 100);
                }
                button.SetData(data.Desc, data.ShowName, data.GoodsCount, canUse, data.Type, data.Name);
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




