
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour{
    

    private Material LoadingMaterial;

    [SerializeField] private float Duration = 1.0f;

    private float time = 0.0f;
    
    private void Awake(){
        Image img = this.GetComponent<Image>();
        Material mat = img.material;
        this.LoadingMaterial = new Material(mat);
        img.material = this.LoadingMaterial;
    }

    private void Update(){
        this.LoadingMaterial.SetFloat(MaterialProperty.LoadingPoint, this.time / this.Duration);
        time += Time.deltaTime;
        if (this.time >= this.Duration){
            GameManager.Instance.GoToMap(false, false);
        }
    }
}

