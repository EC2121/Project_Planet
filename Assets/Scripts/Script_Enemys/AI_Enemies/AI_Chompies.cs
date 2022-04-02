using UnityEngine;
using UnityEngine.AI;

public class AI_Chompies : MonoBehaviour
{
    private Transform target;
    private Transform player;
    private NavMeshAgent agent;
    private NavMeshPath agentPath;
    private Vector3 patrolCenter;
    private Animator anim;
    private readonly float visionAngle;
    private readonly float visionRange;
    private readonly float visionAngleRange;
    private readonly float attackRange = 1;
    private readonly float attackCD = 2f;
    private float attackTimer = 2f;
    private float patrolTimer = 0f;
    private readonly float patrolCD = 2f;
    public bool DebugMode;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agentPath = new NavMeshPath();
        anim = GetComponent<Animator>();
        patrolCenter = transform.position;
        anim.SetBool("Grounded", true);
    }

    private void CheckForPlayer()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        if (( distance <= visionRange ) || ( distance <= visionAngleRange &&
            Vector3.Angle(transform.forward, player.position - transform.position) <= visionAngle ))
        {
            anim.SetBool("HasTarget", true);
            anim.SetBool("InPursuit", true);
            anim.SetTrigger("Spotted");
            target = player;
            SetPathToPlayer();
        }

    }

    private void Patrol()
    {

        if (agent.remainingDistance > agent.stoppingDistance) return;
        if (( patrolTimer += Time.deltaTime ) < patrolCD)
        {
            anim.SetBool("NearBase", true);
            return;
        }

        anim.SetBool("NearBase", false);
        agent.CalculatePath(new Vector3(patrolCenter.x + UnityEngine.Random.insideUnitCircle.x * 10
            , 0.5f, patrolCenter.z + UnityEngine.Random.insideUnitCircle.y * 10), agentPath);
        agent.path = agentPath;

        patrolTimer = 0;


    }

    private void SetPathToPlayer()
    {
        anim.SetBool("InPursuit", true);
        agent.CalculatePath(target.position, agentPath);
        agent.path = agentPath;
    }

    private bool CanAttackTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }
    private void Attack()
    {
        anim.SetBool("InPursuit", false);
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCD)
        {
            anim.SetTrigger("Attack");
            attackTimer = 0;
        }

    }

    private void Reset()
    {
        target = null;
        patrolCenter = transform.position;
        patrolTimer = 0f;
        attackTimer = 0f;
        anim.SetBool("InPursuit", false);
        anim.SetBool("HasTarget", false);

    }

    private void Update()
    {
        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("ChomperAttack");
        if (ReferenceEquals(target, null)) CheckForPlayer();
        bool targetNull = ReferenceEquals(target, null);

        if (!targetNull && !CanAttackTarget() && !isAttacking) SetPathToPlayer();
        if (!targetNull && CanAttackTarget()) Attack();
        if (targetNull) Patrol();
        if (!targetNull && Vector3.Distance(transform.position, target.transform.position) >= 20f) Reset();
        if (!targetNull && !isAttacking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(( target.position - transform.position ).normalized, Vector3.up), Time.deltaTime * 5f);
        }

        Debug.Log(ReferenceEquals(target, null));

    }





    //private void OnDrawGizmos()
    //{

    //    if (DebugMode)
    //    {
    //        Handles.DrawWireDisc(transform.position, Vector3.up, visionRange);
    //        if (patrolCenter != null) Handles.DrawWireDisc(patrolCenter, Vector3.up, 10);
    //        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, visionAngle, visionAngleRange);
    //        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -visionAngle, visionAngleRange);
    //    }

    //}

}
