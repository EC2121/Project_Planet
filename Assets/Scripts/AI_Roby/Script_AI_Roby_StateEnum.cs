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
    private bool patrolling = false;
    private void Awake()
    {
        mai_Player = GameObject.Find("Mai_Player");
        maiPlayerNearZone = 5;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (patrolling)
        {
            Patrolling();
        }
        else FollowPlayer();
    }
    private void FollowPlayer()
    {
        distanceFromMai = Vector3.Distance(transform.position, mai_Player.transform.position);
        if (distanceFromMai > maiPlayerNearZone)
        {
            patrolling = false;
            agent.SetDestination(mai_Player.transform.position);
            //transform.position = Vector3.Lerp(transform.position, mai_Player.transform.position, Time.deltaTime);
        }
        else
        {
            agent.SetDestination(transform.position);
            patrolling = true;
            //agent.SetDestination((new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0)*2) + mai_Player.transform.position);
        }
    }
    private void Patrolling()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            agent.SetDestination(mai_Player.transform.position*Random.Range(-2,2));
    }
}
