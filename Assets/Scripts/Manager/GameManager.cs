
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    public static GameManager Instance;

    private Player CurrentPlayer;
    private BattleData NextBattleData;
    private Vector3 InBigMapPlayerPosition;

    public bool IsBattleEnd{ get; private set; }
    public bool IsBattleVictory{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneChangeManager.Instance.CurrentScene == SceneType.Battle){
            BattleManager.Instance.SetBattleData(this.NextBattleData);
        }

        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap){
            this.CurrentPlayer = FindAnyObjectByType<Player>();
            if (this.IsBattleEnd)
                this.CurrentPlayer.transform.position = this.InBigMapPlayerPosition;
        }
    }

    public void GoToBattle(BattleData battleData){
        this.NextBattleData = battleData;
        this.InBigMapPlayerPosition = this.CurrentPlayer.transform.position;
        BigMapUIManager.Instance.ShowBattleStartUI(battleData.BattleImage, battleData.BattleText);
        // SceneChangeManager.Instance.GoToScene(SceneType.Battle);
    }

    public void StartGame(){
        GoToMap(false, false);    
    }

    public void GoToMap(bool isBattleEnd, bool isBattleVictory){
        this.IsBattleEnd = isBattleEnd;
        this.IsBattleVictory = isBattleVictory;
        SceneChangeManager.Instance.GoToScene(SceneType.BigMap);
    }

    public void GoToMainMenu(){
        SceneChangeManager.Instance.GoToScene(SceneType.Main);
    }

    public void GoToTutorial(){
        SceneChangeManager.Instance.GoToScene(SceneType.Tutorial);
    }

    public void ResetBattleFlag(){
        this.IsBattleEnd = false;
        this.IsBattleVictory = false;
    }
}
