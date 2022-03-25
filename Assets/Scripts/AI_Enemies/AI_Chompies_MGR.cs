using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class AI_Chompies_MGR : MonoBehaviour
{
    [HideInInspector]
    public Enemy Owner;
    public AI_Chompies_BaseState currentState;
    public AI_Chompies_IdleState idleState;
    public AI_Chompies_PatrolState patrolState;
    public AI_Chompies_FollowState followState;
    public AI_Chompies_AttackState attackState;



    public AI_Chompies_MGR(Enemy owner)
    {
        
        
    }
    private void Start()
    {
        idleState = new AI_Chompies_IdleState();
        patrolState = new AI_Chompies_PatrolState();
        followState = new AI_Chompies_FollowState();
        attackState = new AI_Chompies_AttackState();
        Owner = GetComponent<Enemy>();
        currentState = idleState;
        currentState.OnEnter(this);
    }

  

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this,other);
    }

    public void SwitchState(AI_Chompies_BaseState state)
    {
        currentState.OnExit(this);
        currentState = state;
        currentState.OnEnter(this);
    }

  

   
}
