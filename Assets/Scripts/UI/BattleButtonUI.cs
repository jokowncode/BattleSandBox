
using UnityEngine;

public class BattleButtonUI : MonoBehaviour {

    public void GoToBigMap(){
        BattleManager.Instance.AllHeroRecall();
        GameManager.Instance.GoToMap(true, false);
    }
    
}

