using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityTemplateProjects.Saves_Scripts;

public enum EnemyStates
{
    Idle,
    Patrol,
    Attack,
    Follow,
    Alert,
    Die,
    Hit,
    Thrown
}

public enum EnemyType
{
    Chomper,
    AlphaChomper
}

public class Enemy : MonoBehaviour
{
    public static UnityEvent<float, GameObject, bool> OnDamageTaken = new UnityEvent<float, GameObject, bool>();
    public static UnityEvent<GameObject> OnActorDeath = new UnityEvent<GameObject>();
    public static UnityEvent<GameObject> OnEnemyDeath = new UnityEvent<GameObject>();
    [HideInInspector] public Transform Player { get; private set; }
    [HideInInspector] public Transform Roby { get; private set; }
    [HideInInspector] public Transform Hologram { get; private set; }
    [HideInInspector] public NavMeshAgent Agent { get; private set; }
    [HideInInspector] public Animator Anim { get; private set; }

    [HideInInspector] public EnemyType enemyType;
    [HideInInspector] public Dictionary<EnemyStates, AI_Enemies_IBaseState> StatesDictionary;
    [HideInInspector] public /*AnimatorController*/ AnimatorOverrideController AnimatorController;
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
    [HideInInspector] public float VisionRange;
    [HideInInspector] public float VisionAngle;
    [HideInInspector] public float PatrolMaxDist;
    [HideInInspector] public float Hp;
    [HideInInspector] public float HorizontalDot;
    [HideInInspector] public bool IsAlerted;
    [HideInInspector] public bool IsAttacking;
    [HideInInspector] public bool IsDisabled; //Da tenere? Non penso

    //Animation Hashes
    [HideInInspector] public int NearBaseHash = Animator.StringToHash("NearBase");
    [HideInInspector] public int InPursuitHash = Animator.StringToHash("InPursuit");
    [HideInInspector] public int HasTargetHash = Animator.StringToHash("HasTarget");
    [HideInInspector] public int AttackHash = Animator.StringToHash("Attack");
    [HideInInspector] public int SpottedHash = Animator.StringToHash("Spotted");
    [HideInInspector] public int DieHash = Animator.StringToHash("Die");


    [HideInInspector] public SphereCollider sphereCollider;
    public List<Enemy> nearEnemies;
    public Transform Tounge;
    public bool DebugMode;

    private void OnEnable()
    {
        Player_State_Machine.OnHologramEnable.AddListener(OnHologramCreated);
        Player_State_Machine.OnHologramDisable.AddListener(OnHologramDestroy);
        OnActorDeath.AddListener(SwitchTarget);
        OnDamageTaken.AddListener(AddDamage);
        SaveSystem.OnSave += SaveSystemOnOnSave;
        IsDisabled = false;
        sphereCollider = GetComponent<SphereCollider>();
    }

   

    private void SaveSystemOnOnSave(object sender, EventArgs e)
    {
        SaveSystem.SaveData(gameObject, true);
    }

    private void OnDisable()
    {
        OnDamageTaken.RemoveListener(AddDamage);
        OnActorDeath.RemoveListener(SwitchTarget);
        SaveSystem.OnSave -= SaveSystemOnOnSave;
        IsDisabled = true;
    }

    private void Start()
    {
    }

    public void SwitchTarget(GameObject actor)
    {
        Target = Player;
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
            separation += transform.position - enemy.transform.position;
        }

        alignment /= nearEnemies.Count;
        alignment.Normalize();

        cohesion /= nearEnemies.Count;
        cohesion.Normalize();

        separation /= nearEnemies.Count;
        separation.Normalize();


        return ( alignment + cohesion + ( separation * 1.5f ) ) / 3;
    }


    protected virtual void Update()
    {
     
        currentState.UpdateState(this);

    }

    private void OnAnimatorMove()
    {
        
        Vector3 position;
        position = Anim.rootPosition;
        position.y = Agent.nextPosition.y;
        transform.position = position;
    }

    public void LoadData(EnemyData Data, Transform playerRef, Transform robyRef, Transform HologramRef)
    {
        Target = null;
        Hologram = HologramRef;
        Player = playerRef;
        Roby = robyRef;
        Hp = Data.MaxHp;
        enemyType = Data.enemyType;
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
        Anim.avatar = Data.Avatar;
        Anim.applyRootMotion = true;
        Agent = gameObject.AddComponent<NavMeshAgent>();
        Agent.speed = Data.AgentSpeed;
        Agent.stoppingDistance = Data.AgentStoppingDistance;
        AgentPath = new NavMeshPath();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        //SetSpawnPoint
        PatrolCenter = transform.position;
        nearEnemies = new List<Enemy>();
        AttackRange = Data.AttackRange;
        AttackCD = Data.AttackCD;
        PatrolCD = Data.PatrolCD;
        AlertRange = Data.AlertRange;
        VisionAngle = Data.VisionAngle;
        VisionRange = Data.VisionRange;
        PatrolMaxDist = Data.PatrolMaxDistance;
        AttackTimer = UnityEngine.Random.Range(0, 3);
        IdleTimer = UnityEngine.Random.Range(0, 3);
        IsAlerted = false;

        Agent.Warp(PatrolCenter);
    }

    public void OnHologramCreated()
    {
        if (Vector3.Distance(Hologram.position, transform.position) < 20)
        {
            Target = Hologram;
        }
    }

    public void OnAttackStart()
    {
        IsAttacking = true;

        Collider[] colliders = Physics.OverlapSphere(Tounge.position, 0.5f);


 
    }

    public void OnHologramDestroy(GameObject go)
    {
        SwitchState(EnemyStates.Idle);
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
    }

    public void Vanish()
    {
        gameObject.SetActive(false);
    }

    public void AddDamage(float amount, GameObject source, bool wasThrown)
    {
        if (Hp > 0)
        {
            Hp -= amount;
            if (Hp <= 0)
            {
                Invoke("Vanish", 3f);
                OnEnemyDeath.Invoke(gameObject);
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
    }

    public virtual void SwitchState(EnemyStates state)
    {
        currentState.OnExit(this);
        currentState = StatesDictionary[state];
        currentState.OnEnter(this);
    }



    public void OnHitEnd()
    {
        SwitchState(EnemyStates.Follow);
    }

    private void OnTriggerEnter(Collider other)
    {

       

        currentState.OnTrigEnter(this, other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollEnter(this, collision);
    }


    private void OnTriggerExit(Collider other)
    {
    }
}