
using UnityEngine;
using UnityEngine.VFX;

public class VFXControl : MonoBehaviour{

    [SerializeField] private VisualEffect[] VFXs;

    public void Play(){
        foreach (VisualEffect vfx in VFXs){
            vfx.Play();
        }
    }

    public void Stop(){
        foreach (VisualEffect vfx in VFXs){
            vfx.Stop();
        }
    }
}

