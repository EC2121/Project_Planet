using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyStates { Idle, Patrol, Attack, Follow, Alert,Die }
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Transform Player;
    [HideInInspector] public Transform Roby;
    [HideInInspector] public Transform Target;
    [HideInInspector] public NavMeshAgent Agent;
    [HideInInspector] public Animator Anim;
    [HideInInspector] public NavMeshPath AgentPath;
    [HideInInspector] public Dictionary<EnemyStates, AI_Enemies_IBaseState> StatesDictionary;
    [HideInInspector] public AI_Enemies_IBaseState currentState;
    [HideInInspector] public AnimatorController AnimatorController;
    [HideInInspector] public Avatar Avatar;
    [HideInInspector] public Vector3 PatrolCenter;
    [HideInInspector] public float AttackRange;
    [HideInInspector] public float AttackCD;
    [HideInInspector] public float PatrolCD;
    [HideInInspector] public float AlertRange;
    [HideInInspector] public float AttackTimer;
    [HideInInspector] public float IdleTimer;
    [HideInInspector] public float Hp;
    [HideInInspector] public bool IsAlerted;


    //Animation Hashes
    [HideInInspector] public int NearBaseHash = Animator.StringToHash("NearBase");
    [HideInInspector] public int InPursuitHash = Animator.StringToHash("InPursuit");
    [HideInInspector] public int HasTargetHash = Animator.StringToHash("HasTarget");
    [HideInInspector] public int AttackHash = Animator.StringToHash("Attack");
    [HideInInspector] public int SpottedHash = Animator.StringToHash("Spotted");
    [HideInInspector] public int DieHash = Animator.StringToHash("Die");

    public bool DebugMode;


    private void Start()
    {
    }

    protected virtual void Update()
    {
        
        currentState.UpdateState(this);
    }
    private void OnAnimatorMove()
    {
        Vector3 position = Anim.rootPosition;
        position.y = Agent.nextPosition.y;
        transform.position = position;
        Agent.nextPosition = transform.position;
    }
    public void LoadData(EnemyData Data, Transform playerRef, Transform robyRef)
    {
        this.Player = playerRef;
        this.Roby = robyRef;
        Hp = Data.MaxHp;
        StatesDictionary = new Dictionary<EnemyStates, AI_Enemies_IBaseState>();
        StatesDictionary[EnemyStates.Idle] = new AI_Chompies_IdleState();
        StatesDictionary[EnemyStates.Patrol] = new AI_Chompies_PatrolState();
        StatesDictionary[EnemyStates.Follow] = new AI_Chompies_FollowState();
        StatesDictionary[EnemyStates.Attack] = new AI_Chompies_AttackState();
        StatesDictionary[EnemyStates.Alert] = new AI_Chompies_AlertState();
        StatesDictionary[EnemyStates.Die] = new AI_Chompies_DieState();
        currentState = StatesDictionary[EnemyStates.Idle];
        Anim = GetComponent<Animator>();
        Anim.runtimeAnimatorController = Data.AnimatorController;
        Anim.avatar = Data.Avatar;
        Anim.applyRootMotion = true;
        Agent = GetComponent<NavMeshAgent>();
        Agent.updatePosition = false;
        AgentPath = new NavMeshPath();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = Data.VisionRange;
        //SetSpawnPoint
        PatrolCenter = transform.position;
       
        AttackRange = Data.AttackRange;
        AttackCD = Data.AttackCD;
        PatrolCD = Data.PatrolCD;
        AlertRange = Data.AlertRange;
        AttackTimer = UnityEngine.Random.Range(0, 3);
        IdleTimer = UnityEngine.Random.Range(0,3);
        IsAlerted = false;
    }

    public void AddDamage(float amount)
    {
        Hp -= amount;
        if (Hp <= 0) SwitchState(EnemyStates.Die);
    }
    public virtual void SwitchState(EnemyStates state)
    {
        currentState.OnExit(this);
        currentState = StatesDictionary[state];
        currentState.OnEnter(this);
    }

    public void OnAlertEnd()
    {
        SwitchState(EnemyStates.Follow);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTrigEnter(this,other);
    }


    
}
