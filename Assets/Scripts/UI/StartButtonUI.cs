
using UnityEngine;

public class StartButtonUI : MonoBehaviour {

    public void StartGame(){
        GameManager.Instance.StartGame();
    }

    public void Tutorial(){
        GameManager.Instance.GoToTutorial();
    }

    public void Quit(){
        Application.Quit();
    }
        
}

