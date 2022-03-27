using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
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
    public AnimatorController AnimatorController;
    public Avatar Avatar;
  

 
}
