using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chompy : Enemy
{
    // Start is called before the first frame update
    private void Init()
    {
        visionAngle = 30;
        visionRange = 5;
        visionAngleRange = 10;
        attackRange = 3;
        attackCD = 2;
        patrolCD = 2;
        attackTimer = 2;
    }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agentPath = new NavMeshPath();
        patrolCenter = transform.position;
        Init();
        FSM = GetComponent<AI_Chompies_MGR>();
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
