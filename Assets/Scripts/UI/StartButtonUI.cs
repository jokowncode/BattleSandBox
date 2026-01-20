
using System;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonUI : MonoBehaviour {

    [SerializeField] private Button ContinueGameButton;

    private void Start() {
        this.ContinueGameButton.enabled = SaveDataManager.Instance.HasSaveData;
    }

    public void StartGame(){
        GameManager.Instance.StartGame();
    }

    public void ContinueGame() {
        GameManager.Instance.ContinueGame();
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

