
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressText : DialogUISlot {

    [SerializeField] private ScrollRect ScrollArea;
    [SerializeField] private Transform Container;
    [SerializeField] private TextMeshProUGUI TextPrefab;
    [SerializeField] private string TextResourcesPath;

    private TextAsset CurrentAsset;
    private List<int> Seconds = new List<int>();
    private List<TextMeshProUGUI> TextUI = new List<TextMeshProUGUI>();
    private int CurrentIndex = 0;
    private WaitForSeconds Timer = new WaitForSeconds(1.0f);
    private int CurrentSecond = 0;

    public override void Init() {
        this.CurrentAsset = Resources.Load<TextAsset>(this.TextResourcesPath);
        foreach (Transform child in this.Container) {
            Destroy(child.gameObject);
        }
        this.Seconds.Clear();
        this.TextUI.Clear();
        this.CurrentIndex = 0;
        this.CurrentSecond = 0;

        string[] text = this.CurrentAsset.text.Split("\n");
        foreach (string s in text) {
            int secondsEndIndex = s.IndexOf("]");
            string secondsString = s.Substring(1, secondsEndIndex - 1);
            this.Seconds.Add(int.Parse(secondsString));

            string content = s.Substring(secondsEndIndex + 1);
            TextMeshProUGUI textUI = Instantiate(this.TextPrefab, this.Container);
            textUI.text = content;
            textUI.color = Color.gray;
            this.TextUI.Add(textUI);
        }
        DialogManager.Instance.TransitionPause(true);

        StopAllCoroutines();
        StartCoroutine(ProgressTextCoroutine());
    }

    public override void DialogAudioChange(float seconds) {
        StopAllCoroutines();
        int index = this.CurrentIndex == 0 ? this.CurrentIndex : this.CurrentIndex - 1;
        if (index >= 0 && index < this.TextUI.Count) {
            this.TextUI[index].color = Color.gray;
        }

        this.CurrentIndex = 0;
        for (; this.CurrentIndex < this.Seconds.Count; this.CurrentIndex++) {
            if (this.Seconds[this.CurrentIndex] <= seconds && 
                (this.CurrentIndex == this.Seconds.Count - 1 || this.Seconds[this.CurrentIndex+1] > seconds)) {
                break;
            }
        }

        this.CurrentSecond = (int)seconds;
        StartCoroutine(ProgressTextCoroutine());
    }

    private IEnumerator ProgressTextCoroutine() {
        while (this.CurrentIndex < this.TextUI.Count) {
            if (this.Seconds[this.CurrentIndex] <= this.CurrentSecond) {
                this.TextUI[this.CurrentIndex].color = Color.white;
                if (this.CurrentIndex != 0) {
                    this.TextUI[this.CurrentIndex - 1].color = Color.gray;
                }

                this.ScrollArea.verticalNormalizedPosition = 1.0f - this.CurrentIndex * 1.0f / this.TextUI.Count;
                this.CurrentIndex += 1;
            }
            yield return this.Timer;
            this.CurrentSecond += 1;
        }
        DialogManager.Instance.TransitionPause(false);
    }

    public override void End() {
        StopAllCoroutines();
        Resources.UnloadAsset(this.CurrentAsset);
    }
}



