using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine.VFX;

public class FlashManager : MonoBehaviour
{
    public static FlashManager Instance;
    public static bool HasInstance => Instance != null;
    
    [Header("VFX Settings")]
    public GameObject vfxPrefab;
    [SerializeField]private List<GameObject> vfxConnections = new List<GameObject>();
    [SerializeField]private List<FlashPoint> flashPoints = new List<FlashPoint>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Debug.Log("FlashPointCount: "+flashPoints.Count);

        // if (refreshTimer >= refreshInterval)
        // {
        //     refreshTimer = 0f;
        //     RefreshConnections();
        // }
    }

    public void RegisterPoint(FlashPoint point)
    {
        if (!flashPoints.Contains(point))
        {
            flashPoints.Add(point);
            RefreshConnections();
        }
    }

    public void UnregisterPoint(FlashPoint point)
    {
        if (flashPoints.Contains(point))
        {
            flashPoints.Remove(point);
            
            
            RefreshConnections();
        }
    }
    
    public void RefreshConnections()
    {
        // 清除现有连接
        foreach (var obj in vfxConnections)
        {
            Destroy(obj);
        }
        vfxConnections.Clear();

        if (flashPoints.Count < 2) return;

        // 新的逻辑：从第一个开始一路找最近，直到所有点都走一遍，最后回到起点
        List<FlashPoint> visited = new List<FlashPoint>();
        List<FlashPoint> unvisited = new List<FlashPoint>(flashPoints);

        FlashPoint current = unvisited[0];
        FlashPoint first = current;

        visited.Add(current);
        unvisited.Remove(current);

        while (unvisited.Count > 0)
        {
            FlashPoint nearest = null;
            float minDist = float.MaxValue;

            foreach (var candidate in unvisited)
            {
                float dist = Vector3.Distance(current.transform.position, candidate.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = candidate;
                }
            }

            if (nearest != null)
            {
                CreateVFXConnection(current, nearest);
                current = nearest;

                visited.Add(current);
                unvisited.Remove(current);
            }
            else
            {
                break;
            }
        }

        // 最后一个点与起点相连，形成闭环
        if (visited.Count >= 2)
        {
            CreateVFXConnection(current, first);
        }
    }

    private void CreateVFXConnection(FlashPoint from, FlashPoint to)
    {
        GameObject vfxObj = Instantiate(vfxPrefab);
        var vfx = vfxObj.GetComponentInChildren<VisualEffect>();
        if (vfx == null)
        {
            Debug.LogError("VFX prefab 上缺少 VisualEffect 组件", vfxObj);
            return;
        }
        SetPosition(vfx, from.gameObject ,to.gameObject);
        vfxConnections.Add(vfxObj);
    }
    
    void SetPosition(VisualEffect vfx,GameObject startPos, GameObject endPos)
    {
        if (vfx == null)
            vfx = GetComponent<VisualEffect>();

        var positionBinder = vfx.gameObject.GetComponentInChildren<PositionBinder>();
        positionBinder.startPosition = startPos;
        positionBinder.endPosition = endPos;
        // vfx.SetVector3("Pos1", startPos);
        // vfx.SetVector3("Pos2", Vector3.Lerp(startPos,endPos,0.3f));
        // vfx.SetVector3("Pos3", Vector3.Lerp(startPos,endPos,0.7f));
        // vfx.SetVector3("Pos4", endPos);
    }

    // public void SetPosition(Transform startPos, Transform endPos)
    // {
    //     if (lineRenderers.Count > 0)
    //     {
    //         for (int i = 0; i < lineRenderers.Count; i++)
    //         {
    //             if (lineRenderers[i].positionCount >= 2)
    //             {
    //                 lineRenderers[i].SetPosition(0,startPos.position);
    //                 lineRenderers[i].SetPosition(1,endPos.position);
    //             }
    //             else
    //             {
    //                 Debug.Log("The Line renderer should have at least 2 positions");
    //             }
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("Line renderer is empty");
    //     }
    // }
    
    
}
