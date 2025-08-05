using System;
using UnityEngine;
using System.Collections.Generic;

public class FlashPoint : MonoBehaviour
{
    List<GameObject> flashPointsInRange = new List<GameObject>();

    public void Start()
    {
        FlashManager.Instance.RegisterPoint(this);
    }

    public void OnDestroy()
    {
        if (FlashManager.HasInstance)
            FlashManager.Instance.UnregisterPoint(this);
    }

    // public GameObject GetClosestFlashPoint()
    // {
    //     if (flashPointsInRange.Count > 0)
    //     {
    //         GameObject bestTarget = null;
    //         float closestDistanceSqr = Mathf.Infinity;
    //         Vector3 currentPos = transform.position;
    //
    //         foreach (GameObject closestFlashPoint in flashPointsInRange)
    //         {
    //             Vector3 direcionToTarget = closestFlashPoint.transform.position - currentPos;
    //             float dSqrToTarget = direcionToTarget.sqrMagnitude;
    //
    //             if (dSqrToTarget < closestDistanceSqr)
    //             {
    //                 closestDistanceSqr = dSqrToTarget;
    //                 bestTarget = closestFlashPoint;
    //             }
    //         }
    //         return bestTarget;
    //     }
    //     else
    //     {
    //         return null;
    //     }
    // }

    // public List<GameObject> GetFlashPointsInRange()
    // {
    //     return flashPointsInRange;
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Enter");
    //     if (other.CompareTag("Flash"))
    //     {
    //         if(flashPointsInRange.Count==0)
    //         {
    //             flas
    //             flashPointsInRange.Add(other.gameObject);
    //         }
    //         else if(!flashPointsInRange.Contains(other.gameObject))
    //         {
    //             flashPointsInRange.Add(other.gameObject);
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     Debug.Log("Exit");
    //     if (other.CompareTag("Flash"))
    //     {
    //         if(flashPointsInRange.Count>0)
    //             flashPointsInRange.Remove(other.gameObject);
    //     }
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
