using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;


public enum RobyStates { Idle, Follow, Patroll, Battle, RangeAttack, MeleeAttack, ZoneAttack, Die, Hit }
public class Script_Roby : MonoBehaviour
{
    public static UnityEvent Roby_Dead = new UnityEvent();
    public static UnityEvent<float> Roby_Hit = new UnityEvent<float>();

    [HideInInspector] public NavMeshAgent Roby_NavAgent;
    [HideInInspector] public GameObject Roby_EnemyTarget;
    [HideInInspector] public List<GameObject> roby_EnemysInMyArea;

    //Public solo per prova!
    [HideInInspector] public float Roby_Live;
    public float roby_Life;
    public bool GetDamage = false;

    [HideInInspector] public int Roby_EnemyIndex;
    [HideInInspector] public bool Roby_IgnoreEnemy;
    [HideInInspector] public bool IsAttacking;

    public Animator Roby_Animator { get; private set; }
    public GameObject Mai_Player { get; private set; }
    public Dictionary<RobyStates, Script_AI_Roby_BaseState> Roby_StateDictionary { get; private set; }
    public float Mai_MinDistance { get; private set; }
    public float Mai_PlayerNearZone { get; private set; }
    public float Mai_PlayerNormalZone { get; private set; }
    public float Mai_PlayerBattleZone { get; private set; }
    public float Roby_RobyNearZone { get; private set; }
    public int Roby_AshAnimator_walk { get; private set; }
    public int Roby_AshAnimator_walkSpeed { get; private set; }
    public int Roby_AshAnimator_TurnValue { get; private set; }
    public int Roby_AshAnimator_turnTrigger { get; private set; }
    public int Roby_AshAnimator_Melee { get; private set; }
    public int Roby_AshAnimator_Zone { get; private set; }
    public int Roby_AshAnimator_Range { get; private set; }
    public int Roby_AshAnimator_RangeDone { get; private set; }
    public int Roby_AshAnimator_Dead { get; private set; }
    public int Roby_AshAnimator_GetDamage { get; private set; }


    public BoxCollider AttackCollider;
    private Script_AI_Roby_BaseState Roby_CurrentState;
    private ParticleSystem roby_Particle_Shoot;
    private NavMeshPath roby_NavMeshPath;

    private void OnEnable()
    {
        Roby_Hit.AddListener(OnRobyAddDamage);
        Roby_Dead.AddListener(OnRobyDie);
    }

    private void OnDisable()
    {
        Roby_Hit.RemoveListener(OnRobyAddDamage);
        Roby_Dead.RemoveListener(OnRobyDie);
    }

    private void OnAnimatorMove()
    {
        Vector3 position = Roby_Animator.rootPosition;
        //Quaternion rotation = Roby_Animator.rootRotation;
        transform.rotation = Roby_Animator.rootRotation;

        position.y = Roby_NavAgent.nextPosition.y;
        transform.position = position;
        Roby_NavAgent.nextPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Roby_CurrentState.CustomOnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        Roby_CurrentState.CustomOnTriggerExit(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        Roby_CurrentState.CustomOnTriggerStay(this, other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Roby_CurrentState.CustomCollisionEnter(this, collision);
    }
    protected virtual void Update()
    {

        print(roby_EnemysInMyArea.Count);
        if (GetDamage)
        {
            Roby_Hit.Invoke(10);
            GetDamage = false;
        }
        Roby_CurrentState.UpdateState(this);
    }

    private void Awake()
    {
        Mai_Player = GameObject.Find("Mai");

        Roby_NavAgent = GetComponent<NavMeshAgent>();
        Roby_Animator = GetComponent<Animator>();
        //roby_RigidBody = GetComponent<Rigidbody>();
        roby_Particle_Shoot = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        Mai_PlayerNearZone = 7;
        Mai_PlayerNormalZone = 10;
        Mai_PlayerBattleZone = 15;
        Mai_MinDistance = 4;
        Roby_RobyNearZone = 5;

        roby_NavMeshPath = new NavMeshPath();

        roby_EnemysInMyArea = new List<GameObject>();

        Roby_NavAgent.updatePosition = false;
        //Roby_NavAgent.updateRotation = false;
        Roby_Animator.applyRootMotion = true;

        roby_Life = 100000;
        Roby_AshAnimator_Dead = Animator.StringToHash("Death");
        Roby_AshAnimator_RangeDone = Animator.StringToHash("NoMoreAttack");
        Roby_AshAnimator_Range = Animator.StringToHash("RangeAttack");
        Roby_AshAnimator_Zone = Animator.StringToHash("Rotate");
        Roby_AshAnimator_Melee = Animator.StringToHash("MeleeAttack");
        Roby_AshAnimator_walk = Animator.StringToHash("InPursuit");
        Roby_AshAnimator_walkSpeed = Animator.StringToHash("Speed");
        Roby_AshAnimator_TurnValue = Animator.StringToHash("Angle");
        Roby_AshAnimator_turnTrigger = Animator.StringToHash("TurnTrigger");
        Roby_AshAnimator_GetDamage = Animator.StringToHash("Hit");

        Roby_StateDictionary = new Dictionary<RobyStates, Script_AI_Roby_BaseState>();
        Roby_StateDictionary[RobyStates.Idle] = new Script_AI_Roby_Idle();
        Roby_StateDictionary[RobyStates.Patroll] = new Script_AI_Roby_Patroll();
        Roby_StateDictionary[RobyStates.Follow] = new Script_AI_Roby_FollowState();
        Roby_StateDictionary[RobyStates.Battle] = new Script_AI_Roby_BattleState();
        Roby_StateDictionary[RobyStates.MeleeAttack] = new Script_AI_Roby_BattleState_MeleeAttack();
        Roby_StateDictionary[RobyStates.RangeAttack] = new Script_AI_Roby_BattleState_RangedAttack();
        Roby_StateDictionary[RobyStates.ZoneAttack] = new Script_Ai_Roby_BattleState_ZoneAttack();
        Roby_StateDictionary[RobyStates.Die] = new Script_AI_Roby_Dead();

        Roby_CurrentState = Roby_StateDictionary[RobyStates.Idle];
    }

    public virtual void SwitchState(RobyStates state)
    {
        Roby_CurrentState.OnExit(this);
        Roby_CurrentState = Roby_StateDictionary[state];
        Roby_CurrentState.OnEnter(this);
    }

    public void OnRobyAddDamage(float Damage)
    {
        if (roby_Life > 1)
        {
            roby_Life -= Damage;
            Roby_Animator.SetTrigger(Roby_AshAnimator_GetDamage);

            if (roby_Life <= 0) Roby_Dead.Invoke();
        }
        
       
    }

    public void OnAttackStart()
    {
        IsAttacking = true;
        AttackCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
        AttackCollider.enabled = false;
    }

    public void OnRobyDie()
    {
        Enemy.OnActorDeath.Invoke(this.gameObject);
        roby_Life = 1;
        SwitchState(RobyStates.Die);
    }

    public void PrintMe(string msg)
    {
        print(msg);
    }

    public void SetPath(Vector3 roby_targetPath)
    {
        Roby_NavAgent.CalculatePath(roby_targetPath, roby_NavMeshPath);
        Roby_NavAgent.SetPath(roby_NavMeshPath);
    }

    public void RobyShoot()
    {
        if (ReferenceEquals(Roby_EnemyTarget, null)) return;
      
        roby_Particle_Shoot.transform.LookAt(Roby_EnemyTarget.transform.position);
        roby_Particle_Shoot.Play(true);
        Roby_Animator.SetTrigger(Roby_AshAnimator_RangeDone);
        SwitchState(RobyStates.Battle);
    }

    public void RobyExplosion()
    {
        foreach (GameObject enemys in roby_EnemysInMyArea)
        {
            enemys.GetComponent<Rigidbody>().AddExplosionForce(30, transform.position, 5, 1, ForceMode.Impulse);
        }
        Roby_IgnoreEnemy = true;
        SwitchState(RobyStates.Follow);
    }

    public bool EnemysInArea(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            if (!roby_EnemysInMyArea.Contains(enemy))
                roby_EnemysInMyArea.Add(enemy);
            return true;
        }
        return false;
    }

    public void EnemyOutArea(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            if (roby_EnemysInMyArea.Contains(enemy))
                roby_EnemysInMyArea.Remove(enemy);
            if (enemy == Roby_EnemyTarget)
            {
                Roby_EnemyTarget = null;
            }

            print(roby_EnemysInMyArea.Count);
        }
    }

    public bool IsMaITooFar(float zone)
    {
        float distanceFromMai = Vector3.Distance(transform.position, Mai_Player.transform.position);
        if (distanceFromMai > zone) return true;
        return false;
    }

    public float InverseClamp(float min, float max, float value)
    {
        if (value > min && value < max)
        {
            float value_min = Mathf.Abs(min - value);
            float value_max = Mathf.Abs(max - value);
            float result = Mathf.Min(value_max, value_min);
            if (result == value_max) return max;
            else return min;
        }
        return value;
    }

    public void AngleCalculator()
    {

        Vector3 MyForw = transform.forward;
        float dot = Vector3.Dot(MyForw, (Roby_NavAgent.destination - transform.position).normalized);
        Vector3 Cross = Vector3.Cross(MyForw, (Roby_NavAgent.destination - transform.position).normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (Mathf.Sign(Cross.y) == -1)
            angle = 360 - angle;

        print(angle);
        print(Roby_NavAgent.nextPosition);
        if (angle < 1) return;

        if (angle <= 180)
        {
            Roby_Animator.SetTrigger(Roby_AshAnimator_turnTrigger);
            Roby_Animator.SetFloat("Angle", angle / 180);
        }
        else if (angle > 180)
        {
            Roby_Animator.SetTrigger(Roby_AshAnimator_turnTrigger);
            Roby_Animator.SetFloat("Angle", -(angle / 360));
        }
        else return;

    }

}
