using UnityEngine;
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
        owner.Agent.CalculatePath(new Vector3(owner.PatrolCenter.x + Random.insideUnitCircle.x * 10
         , 0.5f, owner.PatrolCenter.z + Random.insideUnitCircle.y * 10), owner.AgentPath);
        owner.Agent.path = owner.AgentPath;
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

        if (owner.Hologram.gameObject.activeInHierarchy)
        {
            float distanceFromHologram = Vector3.Distance(owner.transform.position, owner.Hologram.position);
            if (distanceFromHologram <= 10)
            {
                owner.Target = owner.Hologram;
                owner.IsAlerted = true;
                return true;
            }
        }


        return false;
    }




}
