using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine.VFX;

public class FlashManager : MonoBehaviour {
    public static FlashManager Instance;
    public static bool HasInstance => Instance != null;
    
    [Header("VFX Settings")]
    [SerializeField] private PositionBinder vfxPrefab;
    
    private List<PositionBinder> vfxConnections = new List<PositionBinder>();
    private List<FlashPoint> flashPoints = new List<FlashPoint>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterPoint(FlashPoint point){
        if (!flashPoints.Contains(point)){
            flashPoints.Add(point);
            RefreshConnections();
        }
    }

    public void UnregisterPoint(FlashPoint point){
        if (flashPoints.Contains(point)){
            flashPoints.Remove(point);
            RefreshConnections();
        }
    }
    
    private void RefreshConnections(){
        // 清除现有连接
        foreach (var obj in vfxConnections){
            Destroy(obj.gameObject);
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

        while (unvisited.Count > 0){
            FlashPoint nearest = null;
            float minDist = float.MaxValue;

            foreach (var candidate in unvisited){
                float dist = Vector3.Distance(current.transform.position, candidate.transform.position);
                if (dist < minDist){
                    minDist = dist;
                    nearest = candidate;
                }
            }

            if (nearest != null){
                CreateVFXConnection(current, nearest);
                current = nearest;

                visited.Add(current);
                unvisited.Remove(current);
            }else{
                break;
            }
        }

        // 最后一个点与起点相连，形成闭环
        if (visited.Count > 2){
            CreateVFXConnection(current, first);
        }
    }

    private void CreateVFXConnection(FlashPoint from, FlashPoint to){
        PositionBinder vfxObj = Instantiate(vfxPrefab);
        var vfx = vfxObj.GetComponent<VisualEffect>();
        if (vfx == null){
            Debug.LogError("VFX prefab 上缺少 VisualEffect 组件", vfxObj);
            return;
        }
        SetPosition(vfx, from, to);
        vfxConnections.Add(vfxObj);
    }
    
    void SetPosition(VisualEffect vfx, FlashPoint from, FlashPoint to){
        var positionBinder = vfx.gameObject.GetComponent<PositionBinder>();
        positionBinder.SetEndPoints(new[]{
            from, to
        });
    }
}
