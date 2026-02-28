
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour{

    private TextMeshProUGUI DamageText;
    
    private void Awake(){
        DamageText = GetComponent<TextMeshProUGUI>();
    }

    public void Show(float damage, bool isCritical) {
        if (!DamageText) return;
        DamageText.text = ((int)damage).ToString();
        
        if (isCritical) {
            DamageText.color = Color.red;
            DamageText.fontSize = 1.2f;
        } else {
            DamageText.color = Color.blue;
            DamageText.fontSize = 1.0f;
        }

        StartCoroutine(DamageUICoroutine(0.5f));
    }

    private IEnumerator DamageUICoroutine(float duration) {
        Vector3 dir = Random.insideUnitSphere;
        dir.z = 0.0f;
        dir.y = Mathf.Abs(dir.y);
        
        for (float t = 0.0f; t < duration; t += Time.unscaledDeltaTime) {
            Vector3 newPos = this.transform.position + t * 0.1f * dir.normalized;
            this.transform.position = newPos;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
