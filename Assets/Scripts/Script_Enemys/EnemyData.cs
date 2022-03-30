using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;
using UnityEngine.AI;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    // Start is called before the first frame update

    public float VisionRange;
    public float AttackRange;
    public float AttackCD;
    public float PatrolCD;
    public float AlertRange;
    public float MaxHp;
    public /*AnimatorController*/AnimatorOverrideController AnimatorController;
    public Avatar Avatar;
    public float AgentSpeed;
    public float AgentStoppingDistance;
  

 
}
