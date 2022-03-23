using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;


public enum RobyStates { Idle, Follow, Patroll, Battle, RangeAttack, MeleeAttack, ZoneAttack, Die, Hit }
public class Script_Roby : MonoBehaviour
{
    public Animator Roby_Animator { get; private set; }
    public GameObject Mai_Player { get; private set; }
    public float Mai_MinDistance { get; private set; }
    public float Mai_PlayerNearZone { get; private set; }
    public float Mai_PlayerNormalZone { get; private set; }
    public float Mai_PlayerBattleZone { get; private set; }


    [SerializeField] private Transform roby_LeftHandRoot;
    [SerializeField] private Transform roby_SphereRoot;


    public NavMeshAgent Roby_NavAgent;
    [HideInInspector] public GameObject Roby_EnemyTarget;

    public Dictionary<RobyStates, Script_AI_Roby_BaseState> Roby_StateDictionary;
    public Script_AI_Roby_BaseState Roby_CurrentState;
    public Rigidbody roby_RigidBody;
    public ParticleSystem roby_Particle_Shoot;

    private Script_Roby roby_FSM;
    private MultiAimConstraint roby_MultiAimConstraint;
    private RigBuilder roby_rig;

    public float roby_RobyNearZone;

    public int animator_walkAsh;
    public int animator_walkSpeedAsh;
    public int roby_Animator_TurnValueAsh;
    public int animator_turnTriggerAsh;
    public int roby_Animator_MeleeAsh;
    public int roby_Animator_ZoneAsh;
    public int roby_Animator_RangeAsh;
    public int roby_Animator_RangeDoneAsh;
    public int roby_Animator_DeadAsh;
    public int enemyIndex;

    public bool Roby_IgnoreEnemy;

    public List<GameObject> roby_EnemysInMyArea;
    public float[] nearEnemys;

    private NavMeshPath roby_NavMeshPath;

    private void OnEnable()
    {
        //Roby_Event_Die.AddListener(OnRobyDie);
    }

    private void OnDisable()
    {
        //Roby_Event_Die.RemoveListener(OnRobyDie);
    }
    private void Awake()
    {
        Mai_Player = GameObject.Find("Mai_Player");

        Roby_NavAgent = GetComponent<NavMeshAgent>();
        Roby_Animator = GetComponent<Animator>();
        roby_FSM = GetComponent<Script_Roby>();
        roby_RigidBody = GetComponent<Rigidbody>();
        roby_rig = GetComponent<RigBuilder>();

        roby_Particle_Shoot = GetComponentInChildren<ParticleSystem>();
        roby_MultiAimConstraint = GetComponentInChildren<MultiAimConstraint>();

    }

    private void Init()
    {
        Mai_PlayerNearZone = 7;
        Mai_PlayerNormalZone = 10;
        Mai_PlayerBattleZone = 15;
        Mai_MinDistance = 4;
        roby_RobyNearZone = 5;

        roby_NavMeshPath = new NavMeshPath();

        roby_EnemysInMyArea = new List<GameObject>();
        nearEnemys = new float[10];

        Roby_NavAgent.updatePosition = false;
        //Roby_NavAgent.updateRotation = false;
        Roby_Animator.applyRootMotion = true;


        roby_Animator_DeadAsh = Animator.StringToHash("Death");
        roby_Animator_RangeDoneAsh = Animator.StringToHash("NoMoreAttack");
        roby_Animator_RangeAsh = Animator.StringToHash("RangeAttack");
        roby_Animator_ZoneAsh = Animator.StringToHash("Rotate");
        roby_Animator_MeleeAsh = Animator.StringToHash("MeleeAttack");
        animator_walkAsh = Animator.StringToHash("InPursuit");
        animator_walkSpeedAsh = Animator.StringToHash("Speed");
        roby_Animator_TurnValueAsh = Animator.StringToHash("Angle");
        animator_turnTriggerAsh = Animator.StringToHash("TurnTrigger");

        Roby_StateDictionary = new Dictionary<RobyStates, Script_AI_Roby_BaseState>();
        Roby_StateDictionary[RobyStates.Idle] = new Script_AI_Roby_Idle();
        Roby_StateDictionary[RobyStates.Patroll] = new Script_AI_Roby_Patroll();
        Roby_StateDictionary[RobyStates.Follow] = new Script_AI_Roby_FollowState();
        Roby_StateDictionary[RobyStates.Battle] = new Script_AI_Roby_BattleState();
        Roby_StateDictionary[RobyStates.MeleeAttack] = new Script_AI_Roby_BattleState_MeleeAttack();
        Roby_StateDictionary[RobyStates.RangeAttack] = new Script_AI_Roby_BattleState_RangedAttack();
        Roby_StateDictionary[RobyStates.ZoneAttack] = new Script_Ai_Roby_BattleState_ZoneAttack();

        //Roby_StateDictionary[RobyStates.Die] = new AI_Chompies_DieState();
        Roby_CurrentState = Roby_StateDictionary[RobyStates.Idle];
    }

    void Start()
    {
        Init();
    }

    public virtual void SwitchState(RobyStates state)
    {
        Roby_CurrentState.OnExit(this);
        Roby_CurrentState = Roby_StateDictionary[state];
        Roby_CurrentState.OnEnter(this);
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
        print(roby_EnemysInMyArea.Count);
    }
    protected virtual void Update()
    {
        //AngleCalculator(Roby_NavAgent.desiredVelocity);
        Roby_CurrentState.UpdateState(this);
        print(Roby_CurrentState);
    }


    #region MovingMethod
   public void PrintMe(string msg)
    {
        print(msg);
    }

    public void SetPath(Vector3 roby_targetPath)
    {
        Roby_NavAgent.CalculatePath(roby_targetPath, roby_NavMeshPath);
        Roby_NavAgent.SetPath(roby_NavMeshPath);
        //AngleCalculator();
    }

   
    #endregion

    #region AttackMethod
    

    public void RobyShoot()
    {
        roby_Particle_Shoot.Play(true);
        Roby_Animator.SetTrigger(roby_Animator_RangeDoneAsh);
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

    #endregion

    public void RobyDie()
    {
        Roby_Animator.SetTrigger(roby_Animator_DeadAsh);
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
            Roby_Animator.SetTrigger(animator_turnTriggerAsh);
            Roby_Animator.SetFloat("Angle", angle / 180);
        }
        else if (angle > 180)
        {
            Roby_Animator.SetTrigger(animator_turnTriggerAsh);
            Roby_Animator.SetFloat("Angle", -(angle / 360));
        }
        else return;
        //Vector3 MyForw = transform.forward;
        //float dot = Vector3.Dot(MyForw, (Roby_NavAgent.nextPosition - transform.position).normalized);
        //Vector3 Cross = Vector3.Cross(MyForw, (Roby_NavAgent.nextPosition - transform.position).normalized);
        //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //if (Mathf.Sign(Cross.y) == -1)
        //    angle = 360 - angle;
        //float angle = Vector3.Angle(transform.forward, direction) * Mathf.Sign(Vector3.Dot(transform.right, direction));
        //float lastAngle = angle;
        //if (Mathf.Abs(lastAngle) < 1 && angle != 0)
        //{
        //    Roby_Animator.SetTrigger(animator_turnTriggerAsh);
        //    Roby_Animator.SetFloat(roby_Animator_TurnValueAsh, angle / 90);
        //}
    }
    //public void FollowPlayer()
    //{
    //    Roby_Animator.SetFloat(animator_walkSpeedAsh, 1);

    //    Vector3 nearestPointOnEdge = Mai_Player.transform.position + (Mai_PlayerNearZone) * (Vector3.Normalize(transform.position - Mai_Player.transform.position));
    //    Roby_NavAgent.SetDestination(nearestPointOnEdge);
    //}
    //public void RobyZoneAttackTrigger()
    //{
    //    Roby_Animator.SetTrigger(roby_Animator_ZoneAsh);
    //}
    //public void StopRoby()
    //{
    //    Roby_Animator.SetBool(animator_walkAsh, false);
    //    Roby_NavAgent.velocity = Vector3.zero;
    //    Roby_NavAgent.ResetPath();
    ////}
    ///
    //public bool CheckRemainingDistance()
    //{
    //    if (Roby_NavAgent.remainingDistance < Roby_NavAgent.stoppingDistance) return true;
    //    else return false;
    //}
    //public void ChaseTarget()
    //{
    //    Roby_Animator.SetBool(animator_walkAsh, true);
    //    Roby_NavAgent.SetDestination(Roby_EnemyTarget.transform.position);
    //}
    //public void RobyMeleeAttack()
    //{
    //    Roby_Animator.SetTrigger(roby_Animator_MeleeAsh);
    //}
    //public void Patrolling()
    //{
    //    Roby_Animator.SetFloat(animator_walkSpeedAsh, 0);
    //    Roby_Animator.SetBool(animator_walkAsh, true);

    //    Roby_NavAgent.SetDestination(Mai_Player.transform.position +
    //        new Vector3(InverseClamp(Mai_Player.transform.position.x - Mai_MinDistance, Mai_Player.transform.position.x + Mai_MinDistance, Random.insideUnitCircle.x * Mai_PlayerNearZone), 0,
    //        InverseClamp(Mai_Player.transform.position.z - Mai_MinDistance, Mai_Player.transform.position.z + Mai_MinDistance, Random.insideUnitCircle.y * Mai_PlayerNearZone)));
    //}
    //public void RobyRangeAttack()
    //{
    //    //Roby_Animator.SetTrigger(roby_Animator_RangeAsh);
    //    ////float angle = AngleCalculator();
    //    ////float dot = Vector3.Dot(transform.forward, (myEnemy.transform.position - transform.position).normalized);
    //    ////print(angle);
    //    ////print(dot);

    //    //if (angle <= 180) Roby_Animator.SetFloat("Angle", angle / 180);
    //    //else Roby_Animator.SetFloat("Angle", -(angle / 360));
    //    //Roby_Animator.SetTrigger(animator_turnTriggerAsh);

    //    //Roby_NavAgent.SetDestination(Roby_EnemyTarget.transform.position);
    //    //Roby_NavAgent.isStopped = true;
    //    //var data = roby_MultiAimConstraint.data.sourceObjects;
    //    //data.SetTransform(0, myEnemy.transform);
    //    //roby_MultiAimConstraint.data.sourceObjects = data;
    //    //roby_rig.Build();
    //    //roby_Animator.SetTrigger(roby_Animator_RangeDoneAsh);
    //}
    //public void ChooseTarget()
    //{
    //    if (ReferenceEquals(Roby_EnemyTarget, null) || !Roby_EnemyTarget.activeInHierarchy)
    //    {
    //        Roby_Animator.SetFloat(animator_walkSpeedAsh, 0);
    //        float lowest = float.MaxValue;
    //        for (int i = 0; i < roby_EnemysInMyArea.Count; i++)
    //        {
    //            float distanceFromEnemys = (Vector3.Distance(transform.position, roby_EnemysInMyArea[i].transform.position));
    //            if (distanceFromEnemys != 0 && distanceFromEnemys < lowest)
    //            {
    //                lowest = distanceFromEnemys;
    //                enemyIndex = i;
    //            }
    //        }
    //        Roby_EnemyTarget = roby_EnemysInMyArea[enemyIndex];
    //    }
    //}
    //public void OnRobyDie()
    //{
    //    //Roby_Event_Die.Invoke();
    //}
    //public bool EnemyWithinRange()
    //{
    //    if (Vector3.Distance(transform.position, roby_EnemysInMyArea[enemyIndex].transform.position) < roby_RobyNearZone) return true;
    //    else return false;
    //}

    //public bool AreEnemyNear()
    //{
    //    if (roby_EnemysInMyArea.Count != 0) return true;
    //    else return false;
    //}
}
