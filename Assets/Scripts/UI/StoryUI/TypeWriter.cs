
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour {

    [SerializeField] private float Duration = 1.0f;
    [SerializeField] private float EndDelayTime = 1.0f;

    private TextMeshProUGUI Text;
    private string CurrentContent;
    private WaitForSeconds EndDelayTimer;

    private bool IsPlayEnd;
    public bool IsDelayEnd { get; private set; }

    private void Awake() {
        this.Text = GetComponent<TextMeshProUGUI>();
        this.EndDelayTimer = new WaitForSeconds(this.EndDelayTime);
    }

    public void Play(string text) {
        StopAllCoroutines();
        IsDelayEnd = false;
        IsPlayEnd = false;
        CurrentContent = text;
        StartCoroutine(PlayCoroutine(text));
    }

    private IEnumerator PlayCoroutine(string content) {
        float interval = this.Duration / content.Length;
        WaitForSeconds wait = new WaitForSeconds(interval);
        for (int end = 0; end < content.Length; end++) {
            int length = end + 1;
            this.Text.text = content.Substring(0, length);
            yield return wait;
        }
        this.IsPlayEnd = true;
        yield return EndDelayTimer;
        this.Text.text = content;
        this.IsDelayEnd = true;
    }

    public bool EndText() {
        StopAllCoroutines();
        bool result = this.IsPlayEnd;
        if (!this.IsPlayEnd) {
            this.Text.text = this.CurrentContent;
            this.IsPlayEnd = true;
        }
        this.IsDelayEnd = true;
        return result;
    }
}

