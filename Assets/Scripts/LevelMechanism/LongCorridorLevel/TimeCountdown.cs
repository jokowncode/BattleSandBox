
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeCountdown : MonoBehaviour {

    [SerializeField] private float CountdownDuration = 60.0f;
    [SerializeField] private TextMeshProUGUI CountdownText;

    private readonly WaitForSeconds CountdownTimer = new WaitForSeconds(1.0f);
    
    private void Start() {
        this.CountdownText.text = this.CountdownDuration.ToString();
        BattleManager.Instance.OnBattleStartInRound += OnBattleStartInRound;
        BattleManager.Instance.OnEnemyBeClear += OnEnemyBeClear;
    }

    private void OnEnemyBeClear() {
        StopAllCoroutines();
    }

    private void OnBattleStartInRound() {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine() {
        this.CountdownText.text = this.CountdownDuration.ToString();
        for (float t = this.CountdownDuration; t > 0.0f; t -= 1.0f) {
            this.CountdownText.text = ((int)t).ToString();
            yield return this.CountdownTimer;
        }
        this.CountdownText.text = "0";
        BattleManager.Instance.BattleDefeat();
    }
}

