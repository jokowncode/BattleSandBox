
using UnityEngine;

public class BattleButtonUI : MonoBehaviour {

    public void GoToBigMap(){
        GameManager.Instance.GoToMap(true, false);
    }
    
}

