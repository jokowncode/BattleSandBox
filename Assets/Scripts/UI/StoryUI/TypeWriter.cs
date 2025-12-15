
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour {

    [SerializeField] private float EndDelayTime = 1.0f;
    [SerializeField] private float ShowNextCharacterInterval = 0.5f;

    private TextMeshProUGUI Text;
    private string CurrentContent;
    private WaitForSeconds EndDelayTimer;

    private bool IsPlayEnd;
    public bool IsDelayEnd { get; private set; }

    private void Awake() {
        this.Text = GetComponent<TextMeshProUGUI>();
        this.EndDelayTimer = new WaitForSeconds(this.EndDelayTime);
    }

    public void Play(string text, float duration, bool isConstantVelocity, bool autoNextIfNotContent) {
        StopAllCoroutines();
        IsDelayEnd = false;
        IsPlayEnd = false;
        CurrentContent = text;
        StartCoroutine(PlayCoroutine(text, duration, isConstantVelocity, autoNextIfNotContent));
    }

    private IEnumerator PlayCoroutine(string content, float duration, bool isConstantVelocity, bool autoNextIfNotContent) {
        if (content != "") {
            float interval = isConstantVelocity ? this.ShowNextCharacterInterval : duration / content.Length;
            WaitForSeconds wait = new WaitForSeconds(interval);
            for (int end = 0; end < content.Length; end++) {
                int length = end + 1;
                this.Text.text = content.Substring(0, length);
                yield return wait;
            }    
        }
        this.IsPlayEnd = true;
        yield return EndDelayTimer;
        this.Text.text = content;
        this.IsDelayEnd = true;
        
        if (content == "" && !DialogManager.Instance.IsAutoPlay && autoNextIfNotContent) {
            DialogManager.Instance.Next();
        }
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

