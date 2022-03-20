using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Script_Roby : MonoBehaviour
{
    public Animator roby_Animator { get; private set; }
    public float mai_MinDistance { get; private set; }
    public float mai_PlayerNearZone { get; private set; }
    public float mai_PlayerNormalZone { get; private set; }
    public float mai_PlayerBattleZone { get; private set; }

    private GameObject mai_Player;
    private NavMeshAgent agent;
    private GameObject myEnemy;
    private Script_AI_Roby_MGR roby_FSM;
    private Rigidbody roby_RigidBody;

    private float roby_RobyNearZone;

    private int animator_walkAsh;
    private int animator_walkSpeedAsh;
    private int animator_turnAsh;
    private int animator_turnTriggerAsh;
    private int roby_Animator_MeleeAsh;
    private int roby_Animator_ZoneAsh;
    private int enemyIndex;

    private List<GameObject> roby_EnemysInMyArea;
    private float[] nearEnemys;

    private void Awake()
    {
        mai_Player = GameObject.Find("Mai_Player");
        agent = GetComponent<NavMeshAgent>();
        roby_Animator = GetComponent<Animator>();
        roby_FSM = GetComponent<Script_AI_Roby_MGR>();
        roby_RigidBody = GetComponent<Rigidbody>();
    }

    private void Init()
    {
        mai_PlayerNearZone = 7;
        mai_PlayerNormalZone = 10;
        mai_PlayerBattleZone = 15;
        mai_MinDistance = 4;
        roby_RobyNearZone = 5;

        roby_EnemysInMyArea = new List<GameObject>();
        nearEnemys = new float[10];

        roby_Animator_ZoneAsh = Animator.StringToHash("Rotate");
        roby_Animator_MeleeAsh = Animator.StringToHash("MeleeAttack");
        animator_walkAsh = Animator.StringToHash("InPursuit");
        animator_walkSpeedAsh = Animator.StringToHash("Speed");
        animator_turnAsh = Animator.StringToHash("Angle");
        animator_turnTriggerAsh = Animator.StringToHash("TurnTrigger");
    }

    void Start()
    {
        Init();
    }

    public void FollowPlayer()
    {
        roby_Animator.SetFloat(animator_walkSpeedAsh, 1);

        Vector3 nearestPointOnEdge = mai_Player.transform.position + (mai_PlayerNearZone) * (Vector3.Normalize(transform.position - mai_Player.transform.position));
        agent.SetDestination(nearestPointOnEdge);
    }

    public void Patrolling()
    {
        roby_Animator.SetFloat(animator_walkSpeedAsh, 0);
        roby_Animator.SetBool(animator_walkAsh, true);

        agent.SetDestination(mai_Player.transform.position +
            new Vector3(InverseClamp(mai_Player.transform.position.x - mai_MinDistance, mai_Player.transform.position.x + mai_MinDistance, Random.insideUnitCircle.x * mai_PlayerNearZone), 0,
            InverseClamp(mai_Player.transform.position.z - mai_MinDistance, mai_Player.transform.position.z + mai_MinDistance, Random.insideUnitCircle.y * mai_PlayerNearZone)));
    }

    public bool CheckRemainingDistance()
    {
        if (agent.remainingDistance < agent.stoppingDistance) return true;
        else return false;
    }

    public void RobyExplosion()
    {
        foreach (GameObject enemys in roby_EnemysInMyArea)
        {
            enemys.GetComponent<Rigidbody>().AddExplosionForce(15, transform.position, 5, 1, ForceMode.Impulse);
        }
    }

    public void RobyZoneAttackTrigger()
    {
        roby_Animator.SetTrigger(roby_Animator_ZoneAsh);
    }

    public bool IsMaITooFar(float zone)
    {
        if (Vector3.Distance(transform.position, mai_Player.transform.position) > zone) return true;
        else return false;
    }

    public void StopRoby()
    {
        roby_Animator.SetBool(animator_walkAsh, false);
        agent.velocity = Vector3.zero;
        agent.ResetPath();
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

    public void RobyMeleeAttack()
    {
        roby_Animator.SetTrigger(roby_Animator_MeleeAsh);
        //roby_RigidBody.AddExplosionForce(10, transform.position, 10,1,ForceMode.Impulse);
    }

    public void ChaseTarget()
    {
        roby_Animator.SetBool(animator_walkAsh, true);
        if (ReferenceEquals(myEnemy, null) || !myEnemy.activeInHierarchy)
        {
            roby_Animator.SetFloat(animator_walkSpeedAsh, 0);
            myEnemy = roby_EnemysInMyArea[enemyIndex];
        }
        agent.SetDestination(myEnemy.transform.position);
    }

    public bool EnemyWithinRange()
    {
        float lowest = float.MaxValue;
        for (int i = 0; i < roby_EnemysInMyArea.Count; i++)
        {
            nearEnemys[i] = (Vector3.Distance(transform.position, roby_EnemysInMyArea[i].transform.position));
        }
        for (int i = 0; i < nearEnemys.Length; i++)
        {
            if (nearEnemys[i] != 0 && nearEnemys[i] < lowest)
            {
                lowest = nearEnemys[i];
                enemyIndex = i;
            }
        }

        if (nearEnemys[enemyIndex] < roby_RobyNearZone) return true;
        else return false;
    }

    public bool EnemysInArea(GameObject enemy)
    {
        if (enemy.CompareTag("Chomp"))
        {
            if (!roby_EnemysInMyArea.Contains(enemy))
                roby_EnemysInMyArea.Add(enemy);
            return true;
        }
        return false;
    }

    public void EnemyOutArea(GameObject enemy)
    {
        if (enemy.CompareTag("Chomp"))
        {
            if (roby_EnemysInMyArea.Contains(enemy))
                roby_EnemysInMyArea.Remove(enemy);
        }
    }
}
