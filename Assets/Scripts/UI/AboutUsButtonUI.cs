
using UnityEngine;

public class AboutUsButtonUI : MonoBehaviour {

    public void GoBackToMainMenu(){
        GameManager.Instance.GoToScene(SceneType.Main);
    }
        
}

