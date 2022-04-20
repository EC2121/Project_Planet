using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Chompy : Enemy
{
    // Start is called before the first frame update
    //private void Awake()
    //{
    //    //Player = GameObject.FindGameObjectWithTag("Player").transform;
    //    Anim = GetComponent<Animator>();
    //    Agent = GetComponent<NavMeshAgent>();
    //    AgentPath = new NavMeshPath();
    //    patrolCenter = transform.position;
    //    Init();
    //}

    //private void Init()
    //{
    //    NearBaseHash = Animator.StringToHash("NearBase");
    //    StatesDictionary = new Dictionary<EnemyStates, AI_Enemies_IBaseState>();
    //    StatesDictionary[EnemyStates.Idle] = new AI_Chompies_IdleState();
    //    StatesDictionary[EnemyStates.Patrol] = new AI_Chompies_PatrolState();
    //    currentState = StatesDictionary[EnemyStates.Idle];
    //    Agent.updatePosition = false;
    //    patrolCenter = transform.position;
    //}


    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
    }


}
