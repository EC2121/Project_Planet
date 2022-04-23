using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public enum RobyStates { Idle, Follow, Patroll, Battle, RangeAttack, MeleeAttack, ZoneAttack, Die, Hit, BreakWall }
public class Script_Roby : MonoBehaviour
{
    public static UnityEvent Roby_Dead = new UnityEvent();
    public static UnityEvent<float> Roby_Hit = new UnityEvent<float>();

    public float roby_Life;
    public bool GetDamage = false;
    public Transform Roby_Hand;
    public GameObject Mai_Player;
    public CharacterController Mai_CharacterController;
    public SphereCollider Roby_SphereCollider_Alive;
    public SphereCollider Roby_SphereCollider_Dead;
    public Slider RobyHpSlider;
    public GameObject Roby_Interact_TXT;

    public ParticleSystem LeftFoot;
    public ParticleSystem RightFoot;
     public ParticleSystem Roby_Particle_Shoot;


    [HideInInspector] public GameObject wallToBreak;
    [HideInInspector] public NavMeshAgent Roby_NavAgent;
    [HideInInspector] public GameObject Roby_EnemyTarget;
    [HideInInspector] public List<GameObject> roby_EnemysInMyArea;
    [HideInInspector] public int Roby_EnemyIndex;
    [HideInInspector] public float Roby_Live;
    [HideInInspector] public bool Roby_IgnoreEnemy;
    [HideInInspector] public bool IsAttacking;

    public Animator Roby_Animator { get; private set; }
    public Dictionary<RobyStates, Script_AI_Roby_BaseState> Roby_StateDictionary { get; private set; }
    public NavMeshObstacle Roby_NavMeshObstacle { get; set; }
    public BoxCollider Roby_BoxCollider_Alive { get; set; }
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
    public int Roby_AshAnimator_StartRepair { get; private set; }
    public string Roby_String_Animator_SkyWalkToStop { get; private set; }
    public bool roby_FullSlider { get; set; }


    private NavMeshPath roby_NavMeshPath;
    private Script_AI_Roby_BaseState Roby_CurrentState;

    private void OnEnable()
    {
        Enemy.OnEnemyDeath.AddListener(SwitchTarget);
        Roby_Hit.AddListener(OnRobyAddDamage);
        Roby_Dead.AddListener(OnRobyDie);
        Player_State_Machine.reviveRoby.AddListener(() => roby_FullSlider = true);
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath.RemoveListener(SwitchTarget);
        Roby_Hit.RemoveListener(OnRobyAddDamage);
        Roby_Dead.RemoveListener(OnRobyDie);
        Player_State_Machine.reviveRoby.RemoveListener(() => roby_FullSlider = false);
    }

    private void OnAnimatorMove()
    {
        Vector3 position = Roby_Animator.rootPosition;
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
        Roby_CurrentState.UpdateState(this);
    }

    private void Awake()
    {
        Roby_NavAgent = GetComponent<NavMeshAgent>();
        Roby_Animator = GetComponent<Animator>();
        Roby_BoxCollider_Alive = GetComponentInChildren<BoxCollider>();
        Roby_NavMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        Mai_CharacterController = Mai_Player.GetComponent<CharacterController>();
    }

    public void OnParticleCollision(GameObject other)
    {
        other.GetComponentInParent<Enemy>().AddDamage(20, gameObject, false);

        Roby_Particle_Shoot.Clear();
    }

    private void Start()
    {
        Init();
    }

    public void OnBreakableWallFound(GameObject wall)
    {
        if (wallToBreak != null) return;
        if (roby_EnemysInMyArea.Count < 1)
        {
            wallToBreak = wall;
            SwitchState(RobyStates.BreakWall);
        }
    }

    private void Init()
    {
        Player_State_Machine.onBreakableWallFound.AddListener(OnBreakableWallFound);
        Mai_PlayerNearZone = 3;
        Mai_PlayerNormalZone = 5;
        Mai_PlayerBattleZone = 10;
        Mai_MinDistance = 4;
        Roby_RobyNearZone = 5;
        RobyHpSlider.maxValue = roby_Life;
        RobyHpSlider.value = roby_Life;
        roby_NavMeshPath = new NavMeshPath();

        roby_EnemysInMyArea = new List<GameObject>();

        Roby_NavAgent.updatePosition = false;
        Roby_Animator.applyRootMotion = true;

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
        Roby_AshAnimator_StartRepair = Animator.StringToHash("StartRepair");
        Roby_String_Animator_SkyWalkToStop = "SkyWalkToStop";

        Roby_StateDictionary = new Dictionary<RobyStates, Script_AI_Roby_BaseState>
        {
            [RobyStates.Idle] = new Script_AI_Roby_Idle(),
            [RobyStates.Patroll] = new Script_AI_Roby_Patroll(),
            [RobyStates.Follow] = new Script_AI_Roby_FollowState(),
            [RobyStates.Battle] = new Script_AI_Roby_BattleState(),
            [RobyStates.MeleeAttack] = new Script_AI_Roby_BattleState_MeleeAttack(),
            [RobyStates.RangeAttack] = new Script_AI_Roby_BattleState_RangedAttack(),
            [RobyStates.ZoneAttack] = new Script_Ai_Roby_BattleState_ZoneAttack(),
            [RobyStates.Die] = new Script_AI_Roby_Dead(),
            [RobyStates.BreakWall] = new Script_AI_Roby_BreakWall()
        };
        Roby_CurrentState = Roby_StateDictionary[RobyStates.Idle];
    }

    public virtual void SwitchState(RobyStates state)
    {
        try
        {
            Roby_CurrentState.OnExit(this);
            Roby_CurrentState = Roby_StateDictionary[state];
            Roby_CurrentState.OnEnter(this);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: "+ e.Message);
        }
       
    }

    public void SwitchTarget(GameObject enemy)
    {
        if (roby_EnemysInMyArea.Contains(enemy))
        {
            roby_EnemysInMyArea.Remove(enemy);
            if (roby_EnemysInMyArea.Count > 0)
            {
                Roby_EnemyTarget = roby_EnemysInMyArea[0];
            }
        }
    }

    public void OnRobyAddDamage(float Damage)
    {
        if (roby_Life > 1)
        {
            roby_Life -= Damage;
            if (!Roby_Animator.GetCurrentAnimatorStateInfo(0).IsName("GrenadierMeleeAttack"))
                Roby_Animator.SetTrigger(Roby_AshAnimator_GetDamage);
            RobyHpSlider.value = roby_Life;
            if (roby_Life <= 0) Roby_Dead.Invoke();
        }
    }

    public void OnAttackStart()
    {

        IsAttacking = true;
        Collider[] breakableWall = Physics.OverlapSphere(Roby_Hand.position, 2, 1 << 15);
        if (breakableWall.Length > 0)
        {
            wallToBreak.GetComponentInParent<BreakableObject>().OnFragment();
        }
        Collider[] chompys_Collider = Physics.OverlapSphere(Roby_Hand.position, 0.5f, 1 << 6);

        if (chompys_Collider.Length == 0) return;

        foreach (Collider item in chompys_Collider)
            item.gameObject.GetComponentInParent<Enemy>().AddDamage(20, gameObject, false);
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
    }

    public void OnRobyDie()
    {
        Enemy.OnActorDeath.Invoke(gameObject);
        roby_Life = 1;
        SwitchState(RobyStates.Die);
    }

    public void SetPath(Vector3 roby_targetPath)
    {
        Roby_NavAgent.CalculatePath(roby_targetPath, roby_NavMeshPath);
        Roby_NavAgent.SetPath(roby_NavMeshPath);
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

        }
    }

    public void RobyShoot()
    {
        if (ReferenceEquals(Roby_EnemyTarget, null) || roby_EnemysInMyArea.Count == 0) return;

        Roby_Particle_Shoot.transform.LookAt(Roby_EnemyTarget.transform.position + new Vector3(0, Roby_EnemyTarget.transform.localScale.y * 0.5f, 0));
        Roby_Particle_Shoot.Play(true);
        Roby_Animator.SetTrigger(Roby_AshAnimator_RangeDone);
        SwitchState(RobyStates.Battle);
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
        float dot = Vector3.Dot(MyForw, ( Roby_NavAgent.destination - transform.position ).normalized);
        Vector3 Cross = Vector3.Cross(MyForw, ( Roby_NavAgent.destination - transform.position ).normalized);
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
            Roby_Animator.SetFloat("Angle", -( angle / 360 ));
        }
        else
        {
            return;
        }
    }

}
