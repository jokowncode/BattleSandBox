
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI StartGameText;
    [SerializeField] private Button ContinueGameButton;

    private void Start() {
        this.ContinueGameButton.enabled = SaveDataManager.Instance.HasSaveData;
        this.StartGameText.text = SaveDataManager.Instance.HasAutoSaveData ? "继续游戏" : "开始游戏";
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

