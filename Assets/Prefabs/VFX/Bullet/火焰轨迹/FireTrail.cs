using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(Rigidbody))]
public class FireTrail : MonoBehaviour
{
    public float speed = 50.0f;
    public float destroyDelay = 3.5f;

    public List<ParticleSystem> trailParticle;
    
    //private Rigidbody rb;
    private ParticleSystem.EmitParams emitParam;
    
    private Vector3 lastPosition;
    private bool stop;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        stop = false;
    }
    // IEnumerator SpawnProjectile()
    // {
    //     RaycastHit hit;
    //     if (Physics.Raycast(firePoint.transform.position, Vector3.up, out hit))
    //     {
    //         var projectileVFX = Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
    //         
    //     }
    // }
    private void FixedUpdate()
    {
        CheckIfStopped();
        SetGroundTrailPosition();
    }
    
    private void CheckIfStopped()
    {
        float movementThreshold = 0.001f; // 可调节阈值
        float distance = Vector3.Distance(transform.position, lastPosition);
        stop = distance < movementThreshold;
        lastPosition = transform.position;
    }

    public void SetGroundTrailPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (!stop)
            {
                for(int i =0;i<trailParticle.Count;i++)
                {
                    ParticleSystem.EmissionModule emissionModule = trailParticle[i].emission;
                    emitParam.position = hit.point + Vector3.up * 0.1f;
                    emitParam.rotation3D = Quaternion.LookRotation(hit.normal).eulerAngles;

                    emissionModule.enabled = true;
                    trailParticle[i].Emit(emitParam, 1);
                }
            }
        }
    }


}
