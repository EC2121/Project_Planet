using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Script_AI_Roby_StateEnum : MonoBehaviour
{
    private Animator roby_Animator;
    private GameObject mai_Player;
    private NavMeshAgent agent;

    private float distanceFromMai;
    private float maiPlayerNearZone;
    private float maiPlayerNormalZone;
    private float time = 0;


    private int walkAsh;
    private int walkSpeedAsh;
    private int turnAsh;
    private int turnTriggerAsh;

    private void Awake()
    {
        mai_Player = GameObject.Find("Mai_Player");
        agent = GetComponent<NavMeshAgent>();
        roby_Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        maiPlayerNearZone = 7;
        maiPlayerNormalZone = 10;
        distanceFromMai = 4;

        walkAsh = Animator.StringToHash("InPursuit");
        walkSpeedAsh = Animator.StringToHash("Speed");
        turnAsh = Animator.StringToHash("Angle");
        turnTriggerAsh = Animator.StringToHash("TurnTrigger");

    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, mai_Player.transform.position) > maiPlayerNormalZone)
        {
            FollowPlayer();
        }
        else
        {
            Patrolling();
        }
    }
    private void FollowPlayer()
    {
        if (!agent.hasPath || agent.remainingDistance < 1)
        {
            Vector3 DPC = transform.position - mai_Player.transform.position;
            float ND = Vector3.Dot(Vector3.Normalize(DPC), DPC);
            Vector3 QmC = DPC - ND * Vector3.Normalize(DPC);
            Vector3 closest = mai_Player.transform.position + maiPlayerNormalZone * (QmC / Vector3.Magnitude(QmC));
            Debug.DrawLine(mai_Player.transform.position, closest);
            agent.SetDestination(closest);
        }
        roby_Animator.SetBool(walkAsh, true);
        roby_Animator.SetFloat(walkSpeedAsh, 1);
    }
    private void Patrolling()
    {
        roby_Animator.SetFloat(walkSpeedAsh, 0);
        if (agent.remainingDistance > agent.stoppingDistance)
            return;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            float activity = Random.Range(0, 2);
            print(activity);
            switch (activity)
            {
                case 0:
                    roby_Animator.SetBool(walkAsh, true);
                    agent.SetDestination(mai_Player.transform.position +
                        new Vector3(InverseClamp(mai_Player.transform.position.x - distanceFromMai, mai_Player.transform.position.x + distanceFromMai, Random.insideUnitCircle.x * maiPlayerNearZone), 0,
                        InverseClamp(mai_Player.transform.position.z - distanceFromMai, mai_Player.transform.position.z + distanceFromMai, Random.insideUnitCircle.y * maiPlayerNearZone)));
                    time = 2;
                    break;
                case 1:
                    time = 3;
                    break;
            }
        }
        else
        {
            agent.velocity = Vector3.zero;
            roby_Animator.SetBool(walkAsh, false);
            //roby_Animator.SetTrigger(turnTriggerAsh);
            //roby_Animator.SetFloat(turnAsh, Random.Range(-1f,1f));
        }
    }
    private
     float InverseClamp(float min, float max, float value)
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
}
