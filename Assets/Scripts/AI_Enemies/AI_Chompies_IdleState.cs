using UnityEngine;

public class AI_Chompies_IdleState : AI_Enemies_IBaseState
{
    //public override void OnEnter(AI_Chompies_MGR AI)
    //{
    //    AI.Owner.Idle();
    //}

    public void OnEnter(Enemy owner)
    {
        owner.Anim.SetBool(owner.NearBaseHash, true);
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
    }


    public bool IdleTimerExpired(Enemy owner)
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





}
