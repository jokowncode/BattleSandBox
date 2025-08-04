using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter {
    void Update()
    {
        DebugFighterData();
    }

    void DebugFighterData()
    {
        Debug.Log("FighterData：");
        Debug.Log("Shield: "+Shield);
        Debug.Log("Health: "+Health);
        Debug.Log("PhysicsAtk: "+PhysicsAttack);
        Debug.Log("MagicAtk: "+MagicAttack);
        Debug.Log("MoveSpeed: "+Speed);
        Debug.Log("AtkSpeed: "+FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier));     
    }
}
