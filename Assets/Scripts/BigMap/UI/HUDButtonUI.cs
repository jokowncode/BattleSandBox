
using UnityEngine;

public class HUDButtonUI : MonoBehaviour {

    public void OpenHeroWarehouse() {
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

    public void OpenLoadPanel() {
        SaveDataManager.Instance.ShowSaveLoadDataUI(false);
    }

    public void OpenCluePanel() {
        ClueWarehouseManager.Instance.TransitionShowUI(true);
    }

    public void OpenGoodsPanel() {
        GoodsWarehouseManager.Instance.TransitionGoodsPanel(true);
    }

}


