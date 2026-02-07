
using UnityEngine;

public class BigMapHUDButtonUI : MonoBehaviour {

    public void OpenHeroWarehouse() {
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }

}


