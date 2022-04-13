using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI_Chompies_PatrolState : AI_Enemies_IBaseState
{

    public static UnityEvent unityEvent;
    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool(owner.NearBaseHash, false);
        SetRandomPath(owner);
    }

 

    public void OnExit(Enemy owner)
    {
    }

    public void UpdateState(Enemy owner)
    {
        if (owner.Agent.remainingDistance < owner.Agent.stoppingDistance)
        {
            owner.SwitchState(EnemyStates.Idle);
            return;
        }
        if(CheckForTarget(owner))
        {
            owner.SwitchState(EnemyStates.Alert);
            return;
        }

    }

    private void SetRandomPath(Enemy owner)
    {
        Vector3 randomPosition = Random.insideUnitSphere * owner.PatrolMaxDist + owner.transform.position;
        NavMeshHit navMeshHit;
        int patrolableAreaMask = 1 << 3;
        if (NavMesh.SamplePosition(randomPosition, out navMeshHit, owner.PatrolMaxDist, patrolableAreaMask))
        {
            owner.Agent.CalculatePath(navMeshHit.position, owner.AgentPath);
            owner.Agent.path = owner.AgentPath;
        }
      
        //owner.Agent.CalculatePath(new Vector3(owner.PatrolCenter.x + Random.insideUnitCircle.x * 10
        // , owner.transform.position.y, owner.PatrolCenter.z + Random.insideUnitCircle.y * 10), owner.AgentPath);
        //owner.Agent.path = owner.AgentPath;
    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {
        //if (owner.Target != null) return;

        //if (ReferenceEquals(other.gameObject, owner.Player.gameObject))
        //{
        //    owner.Target = owner.Player;
        //    owner.IsAlerted = true;
        //    owner.SwitchState(EnemyStates.Alert);
        //    owner.sphereCollider.enabled = false;

        //    return;
        //}
        //if (ReferenceEquals(other.gameObject, owner.Roby.gameObject))
        //{
        //    owner.Target = owner.Roby;
        //    owner.IsAlerted = true;
        //    owner.SwitchState(EnemyStates.Alert);
        //    owner.sphereCollider.enabled = false;

        //    return;
        //}

    }

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }

    public bool CheckForTarget(Enemy owner)
    {

        float distanceFromPlayer = Vector3.Distance(owner.transform.position, owner.Player.position);
        float distanceFromRoby = Vector3.Distance(owner.transform.position, owner.Roby.position);


        if ((distanceFromPlayer <= 10) || (distanceFromPlayer <= 10 &&
            Vector3.Angle(owner.transform.forward, owner.Player.position - owner.transform.position) <= 30))
        {
            owner.Target = owner.Player;
            owner.IsAlerted = true;
            return true;
        }

        if ((distanceFromRoby <= 10) || (distanceFromRoby <= 10 &&
            Vector3.Angle(owner.transform.forward, owner.Roby.position - owner.transform.position) <= 30))
        {
            owner.Target = owner.Roby;
            owner.IsAlerted = true;
            return true;
        }



        return false;
    }




}
