
using UnityEngine;

public class HUDButtonUI : MonoBehaviour {

    public void OpenHeroWarehouse() {
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

    public void OpenLoadPanel() {
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        SaveDataManager.Instance.ShowSaveLoadDataUI(false);
    }

    public void OpenCluePanel() {
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        ClueWarehouseManager.Instance.TransitionShowUI(true);
    }

    public void OpenGoodsPanel() {
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        GoodsWarehouseManager.Instance.TransitionGoodsPanel(true);
    }

    public void QuitGame() {
        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap && DialogManager.Instance.IsInDialog) {
            return;
        }
        GameManager.Instance.GoToScene(SceneType.Main);
    }

}


