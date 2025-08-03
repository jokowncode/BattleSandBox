using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter {
    void Update()
    {
        Debug.Log("EnemyHealth" + Health);
        Debug.Log("EnemySpeed" + Speed);
        Debug.Log("EnemyAttack" + PhysicsAttack);
    }
}
