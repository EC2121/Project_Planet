using UnityEngine;

public class AI_Chompies_IdleState : AI_Enemies_IBaseState
{
    //public override void OnEnter(AI_Chompies_MGR AI)
    //{
    //    AI.Owner.Idle();
    //}

    public void OnEnter(Enemy owner)
    {
        owner.PatrolCenter = owner.transform.position;
        owner.Anim.SetBool(owner.InPursuitHash, false);
        owner.Anim.SetBool(owner.HasTargetHash, false);
        owner.Anim.SetBool(owner.NearBaseHash, true);
        owner.IsAlerted = false;
        owner.Target = null;
        owner.Agent.ResetPath();
    }

    public void OnExit(Enemy owner)
    {
    }

    public void UpdateState(Enemy owner)
    {
        if (IdleTimerExpired(owner))
        {
            owner.SwitchState(EnemyStates.Patrol);
            return;
        }
        if (CheckForTarget(owner))
        {
            owner.SwitchState(EnemyStates.Alert);
            return;
        }
    }


    private bool IdleTimerExpired(Enemy owner)
    {
        owner.IdleTimer += Time.deltaTime;
        if (owner.IdleTimer >= owner.PatrolCD)
        {
            owner.IdleTimer = 0;
            return true;
        }

        return false;
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

    public void OnCollEnter(Enemy owner, Collision other)
    {
    }
}