
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationManager : MonoBehaviour{
    
    public static FormationManager Instance;

    [SerializeField] private int MaxCount = 5;

    private int Angle => 180 / this.MaxCount;
    
    private void Awake(){
        if (Instance != null){
            return;
        }
        Instance = this;
    }
    
    public bool ValidTarget(Fighter target) {
        return target.AttackerCount < this.MaxCount;
    }

    public Vector3 GetFormationPosition(Fighter target, float radius) {
        float angle = (target.AttackerCount * Angle) % 360f;
        return MathTools.CalculateCirclePosition(target.transform.position, angle, radius);
    }
}

