
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour {

    [SerializeField] private float Duration = 1.0f;
    
    private TextMeshProUGUI Text;
    private string CurrentContent;
    
    public bool IsEnd { get; private set; }

    private void Awake() {
        this.Text = GetComponent<TextMeshProUGUI>();
    }

    public void Play(string text) {
        StopAllCoroutines();
        IsEnd = false;
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
        this.Text.text = content;
        this.IsEnd = true;
    }

    public void EndText() {
        StopAllCoroutines();
        this.Text.text = this.CurrentContent;
        this.IsEnd = true;
    }
}

