public class Player_Interactable : Player_BaseState
{
    private bool canExit = false;
    public Player_Interactable(Player_State_Machine currentContext, Player_StateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Interact();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        canExit = false;

        if (Context.IsInteract)
        {
            Context.RequireNewInteraction = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if (canExit )
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    private void Interact()
    {
        if (Context.Mai_BoxIsTakable)
        {
            Context.GamePlayerFinalePhase.Invoke();
            if (!Context.HasBox)
            {
                Context.HasBox = true;
                canExit = true;
                Context.Animator.SetBool(Context.HasBoxHash, true);
                Context.TakeTheBox.Invoke();
            }
            else if (Context.HasBox)
            {
                Context.HasBox = false;
                canExit = true;
                Context.Animator.SetBool(Context.HasBoxHash, false);
                Context.TakeTheBox.Invoke();
            }
        }
        if (Context.IsCrystalActivable)
        {
            Context.CanCrystal.Invoke();
            canExit = true;

        }
    }

}