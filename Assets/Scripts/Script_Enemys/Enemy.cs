using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyStates { Idle, Patrol, Attack, Follow, Alert, Die, Hit, Thrown}
public class Enemy : MonoBehaviour
{

    public static UnityEvent<float, GameObject,bool> OnDamageTaken = new UnityEvent<float, GameObject,bool>();
    [HideInInspector] public Transform Player { get; private set; }
    [HideInInspector] public Transform Roby { get; private set; }
    [HideInInspector] public NavMeshAgent Agent { get; private set; }
    [HideInInspector] public Animator Anim { get; private set; }

    [HideInInspector] public Dictionary<EnemyStates, AI_Enemies_IBaseState> StatesDictionary;
    [HideInInspector] public AnimatorController AnimatorController;
    [HideInInspector] public NavMeshPath AgentPath;
    [HideInInspector] public AI_Enemies_IBaseState currentState;
    [HideInInspector] public Transform Target;
    [HideInInspector] public Avatar Avatar;
    [HideInInspector] public Vector3 PatrolCenter;
    [HideInInspector] public float AttackRange;
    [HideInInspector] public float AttackCD;
    [HideInInspector] public float PatrolCD;
    [HideInInspector] public float AlertRange;
    [HideInInspector] public float AttackTimer;
    [HideInInspector] public float IdleTimer;
    [HideInInspector] public float Hp;
    [HideInInspector] public float HorizontalDot;
    [HideInInspector] public bool IsAlerted;

    //Animation Hashes
    [HideInInspector] public int NearBaseHash = Animator.StringToHash("NearBase");
    [HideInInspector] public int InPursuitHash = Animator.StringToHash("InPursuit");
    [HideInInspector] public int HasTargetHash = Animator.StringToHash("HasTarget");
    [HideInInspector] public int AttackHash = Animator.StringToHash("Attack");
    [HideInInspector] public int SpottedHash = Animator.StringToHash("Spotted");
    [HideInInspector] public int DieHash = Animator.StringToHash("Die");

    public List<Enemy> nearEnemies;

    public bool DebugMode;
    private void OnEnable()
    {
        OnDamageTaken.AddListener(AddDamage);
    }

    private void OnDisable()
    {
        OnDamageTaken.RemoveListener(AddDamage);
    }
    private void Start()
    {
    }
    public Vector3 Flocking()
    {
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;


        foreach (var enemy in nearEnemies)
        {
            alignment += enemy.Agent.velocity;
            cohesion += enemy.transform.position;
            separation += this.transform.position - enemy.transform.position;
        }

        alignment /= nearEnemies.Count;
        alignment.Normalize();

        cohesion /= nearEnemies.Count;
        cohesion.Normalize();

        separation /= nearEnemies.Count;
        separation.Normalize();


        return (alignment + cohesion + (separation * 1.5f)) / 3;

    }



    protected virtual void Update()
    {
        //Vector3 flockingVec = Flocking();
        //transform.position = Agent.nextPosition;
        //Agent.nextPosition = Vector3.Lerp(transform.position, Agent.nextPosition + flockingVec, Time.deltaTime);
        currentState.UpdateState(this);
    }
    private void OnAnimatorMove()
    {
        //if (ReferenceEquals(currentState, StatesDictionary[EnemyStates.Follow]))
        //{
        //    Vector3 flockingVec = Flocking();
        //    position = Vector3.Lerp(Anim.rootPosition, Agent.nextPosition + flockingVec, Time.deltaTime);    //Anim.rootPosition;
        //}
        //else
        //    position = Anim.rootPosition;

        //Vector3 flockingVec = Flocking();
        Vector3 position;
        position = Anim.rootPosition;
        position.y = Agent.nextPosition.y;
        transform.position = position;

    }
    public void LoadData(EnemyData Data, Transform playerRef, Transform robyRef)
    {
        Target = null;
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
        StatesDictionary[EnemyStates.Hit] = new AI_Chompies_HitState();
        StatesDictionary[EnemyStates.Thrown] = new AI_Chompies_ThrownState();
        currentState = StatesDictionary[EnemyStates.Idle];
        Anim = GetComponent<Animator>();
        Anim.runtimeAnimatorController = Data.AnimatorController;
        Anim.avatar = Data.Avatar;
        Anim.applyRootMotion = true;
        Agent =  gameObject.AddComponent<NavMeshAgent>();
        Agent.speed = Data.AgentSpeed;
        Agent.stoppingDistance = Data.AgentStoppingDistance;
        AgentPath = new NavMeshPath();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = Data.VisionRange;
        sphereCollider.isTrigger = true;
        //SetSpawnPoint
        PatrolCenter = transform.position;
        nearEnemies = new List<Enemy>();
        AttackRange = Data.AttackRange;
        AttackCD = Data.AttackCD;
        PatrolCD = Data.PatrolCD;
        AlertRange = Data.AlertRange;
        AttackTimer = UnityEngine.Random.Range(0, 3);
        IdleTimer = UnityEngine.Random.Range(0, 3);
        IsAlerted = false;
    }

    public void AddDamage(float amount,GameObject source,bool wasThrown)
    {
        if (Target == null) return;
        
        Hp -= amount;

        if (Hp <= 0)
        {
            SwitchState(EnemyStates.Die);
            return;
        }

        else
        {
            if (wasThrown)
            {
                SwitchState(EnemyStates.Thrown);
                return;
            }
            HorizontalDot = Vector3.Dot(transform.forward, source.transform.forward);
            SwitchState(EnemyStates.Hit);
        }
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

    public void OnHitEnd()
    {
        SwitchState(EnemyStates.Follow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!nearEnemies.Contains(enemy))
            {
                nearEnemies.Add(enemy);
            }
        }

        currentState.OnTrigEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (nearEnemies.Contains(enemy))
            {
                nearEnemies.Remove(enemy);
            }
        }


    }



}
