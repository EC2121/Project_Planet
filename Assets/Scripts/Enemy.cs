using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected Transform target;
    protected Animator anim;
    protected Transform player;
    protected NavMeshAgent agent;
    protected NavMeshPath agentPath;
    protected Vector3 patrolCenter;
    protected float visionAngle;
    protected float visionRange;
    protected float visionAngleRange;
    protected float attackRange;
    protected float attackCD;
    protected float patrolCD;
    protected float attackTimer;
    protected float patrolTimer;
    protected AI_Chompies_MGR FSM;

    private bool DebugMode;

    public virtual void Patrol()
    {
        anim.SetBool("NearBase", false);
        agent.CalculatePath(new Vector3(patrolCenter.x + UnityEngine.Random.insideUnitCircle.x * 10
            , 0.5f, patrolCenter.z + UnityEngine.Random.insideUnitCircle.y * 10), agentPath);
        agent.path = agentPath;

    }
    public virtual bool CheckForPlayer()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        if ((distance <= visionRange) || (distance <= visionAngleRange &&
            Vector3.Angle(transform.forward, player.position - transform.position) <= visionAngle))
        {
            target = player;
            return true;
        }
        return false;
    }

    public virtual bool CheckDistance()
    {
        return Vector3.Distance(transform.position, target.transform.position) >= 20f;
    }

    public virtual bool FindNewPoint()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolCD)
        {
            patrolTimer = 0;
            return true;
        }
        return false;
    }
    public virtual void Res()
    {
        target = null;
        patrolCenter = transform.position;
        patrolTimer = 0f;
        attackTimer = 0f;
        anim.SetBool("InPursuit", false);
        anim.SetBool("HasTarget", false);
    }
    public virtual bool HasPathFinished()
    {

        return agent.remainingDistance < agent.stoppingDistance;
    }
    public virtual void Idle()
    {
        anim.SetBool("NearBase", true);
    }
    public virtual void FollowPlayer()
    {
        agent.CalculatePath(target.position, agentPath);
        agent.path = agentPath;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up), Time.deltaTime * 5f);
    }

    public virtual void StopChasingPlayer()
    {
        anim.SetBool("InPursuit", false);
        agent.ResetPath();
    }

    public virtual bool IsAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("ChomperAttack") || anim.IsInTransition(0);
    }
    public virtual bool CanAttackTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }
    public virtual void Attack()
    {
        attackTimer += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up), Time.deltaTime * 5f);
        if (attackTimer >= attackCD)
        {
            anim.SetTrigger("Attack");
            attackTimer = 0;
        }
    }
    public virtual void SetPathToPlayer()
    {
        anim.SetBool("HasTarget", true);
        anim.SetBool("InPursuit", true);
        agent.CalculatePath(target.position, agentPath);
        agent.path = agentPath;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
