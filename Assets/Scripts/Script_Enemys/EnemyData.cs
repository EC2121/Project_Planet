using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    // Start is called before the first frame update
    public EnemyType enemyType;
    public float VisionRange;
    public float VisionAngle;
    public float AttackRange;
    public float AttackCD;
    public float PatrolCD;
    public float AlertRange;
    public float MaxHp;
    public Avatar Avatar;
    public float AgentSpeed;
    public float AgentStoppingDistance;
    public float PatrolMaxDistance;

    
}
