
using UnityEngine;

public class StartButtonUI : MonoBehaviour {

    public void StartGame(){
        GameManager.Instance.StartGame();
    }

    public void Tutorial(){
        GameManager.Instance.GoToScene(SceneType.Tutorial);
    }

    public void AboutUs(){
        GameManager.Instance.GoToScene(SceneType.AboutUs);
    }

    public void Quit(){
        Application.Quit();
    }
        
}

