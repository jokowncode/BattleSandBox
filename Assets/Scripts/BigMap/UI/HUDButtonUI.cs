
using UnityEngine;

public class HUDButtonUI : MonoBehaviour {

    public void OpenHeroWarehouse() {
        if (DialogManager.Instance && DialogManager.Instance.IsInDialog) return;
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

    public void OpenLoadPanel() {
        if (DialogManager.Instance && DialogManager.Instance.IsInDialog) return;
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        SaveDataManager.Instance.ShowSaveLoadDataUI(false);
    }

    public void OpenCluePanel() {
        if (DialogManager.Instance && DialogManager.Instance.IsInDialog) return;
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        ClueWarehouseManager.Instance.TransitionShowUI(true);
    }

    public void OpenGoodsPanel() {
        if (DialogManager.Instance && DialogManager.Instance.IsInDialog) return;
        if (GoodsWarehouseManager.Instance.IsOpen) return;
        GoodsWarehouseManager.Instance.TransitionGoodsPanel(true);
    }

    public void QuitGame() {
        if (DialogManager.Instance && DialogManager.Instance.IsInDialog) return;
        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap && DialogManager.Instance.IsInDialog) {
            return;
        }

        if (SceneChangeManager.Instance.CurrentScene == SceneType.Camp ||
            (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap 
            && SceneChangeManager.Instance.DungeonScene != SceneType.Dungeons_Newbie)) {
            SaveDataManager.Instance.AutoSaveData();
        }
        GameManager.Instance.GoToScene(SceneType.Main);
    }

}


