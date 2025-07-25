
using System;
using UnityEngine;

// TODO: Game Audio -> Play, Fade Out, Fade In
public class AudioManager : MonoBehaviour{

    public static AudioManager Instance;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

