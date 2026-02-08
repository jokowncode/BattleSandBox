
using UnityEngine;

public class BigMapHUDButtonUI : MonoBehaviour {

    public void OpenHeroWarehouse() {
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

    public void OpenLoadPanel() {
        SaveDataManager.Instance.ShowSaveLoadDataUI(false);
    }

}


