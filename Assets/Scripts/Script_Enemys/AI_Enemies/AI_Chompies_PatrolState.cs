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

    }

    private void SetRandomPath(Enemy owner)
    {
        owner.Agent.CalculatePath(new Vector3(owner.PatrolCenter.x + Random.insideUnitCircle.x * 10
         , 0.5f, owner.PatrolCenter.z + Random.insideUnitCircle.y * 10), owner.AgentPath);
        owner.Agent.path = owner.AgentPath;
    }

    public void OnTrigEnter(Enemy owner, Collider other)
    {
        if (owner.Target != null) return;

        if (ReferenceEquals(other.gameObject, owner.Player.gameObject))
        {
            owner.Target = owner.Player;
            owner.IsAlerted = true;
            owner.SwitchState(EnemyStates.Alert);
            return;
        }
        if (ReferenceEquals(other.gameObject, owner.Roby.gameObject))
        {
            owner.Target = owner.Roby;
            owner.IsAlerted = true;
            owner.SwitchState(EnemyStates.Alert);
            return;
        }

    }

    //public bool CheckForTarget(Enemy owner)
    //{

    //    float distanceFromPlayer = Vector3.Distance(owner.transform.position, owner.Player.position);
    //    float distanceFromRoby = Vector3.Distance(owner.transform.position, owner.Player.position);


    //    if ((distanceFromPlayer <= owner.VisionRange) || (distanceFromPlayer <= owner.VisionAngleRange &&
    //        Vector3.Angle(owner.transform.forward, owner.Player.position - owner.transform.position) <= owner.VisionAngle))
    //    {
    //        owner.Target = owner.Player;
    //        owner.IsAlerted = true;
    //        return true;
    //    }

    //    if ((distanceFromRoby <= owner.VisionRange) || (distanceFromRoby <= owner.VisionAngleRange &&
    //        Vector3.Angle(owner.transform.forward, owner.Roby.position - owner.transform.position) <= owner.VisionAngle))
    //    {
    //        owner.Target = owner.Roby;
    //        owner.IsAlerted = true;
    //        return true;
    //    }

    //    return false;
    //}

   

    
}
