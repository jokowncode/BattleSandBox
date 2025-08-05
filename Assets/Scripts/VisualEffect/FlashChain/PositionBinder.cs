using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PositionBinder : MonoBehaviour
{
    public GameObject startPosition;
    public GameObject endPosition;

    [SerializeField] private VisualEffect vfx;
    
    // Start is called before the first frame update
    void Start()
    {
        vfx = this.GetComponent<VisualEffect>();
    }

    private void Update()
    {
        vfx.SetVector3("StartPos", startPosition.transform.position);
        vfx.SetVector3("EndPos", endPosition.transform.position);
    }
}
