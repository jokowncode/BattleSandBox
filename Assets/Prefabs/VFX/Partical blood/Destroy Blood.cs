using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlood : MonoBehaviour
{
    [SerializeField] private float time = 0.3f;
    void Start()
    {
        Destroy(gameObject, time);
    }

    // Update is called once per frame
   
}
