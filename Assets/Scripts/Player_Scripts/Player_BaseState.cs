using UnityEngine.Rendering;

public abstract class Player_BaseState
{
    protected Player_State_Machine _ctx;
    protected Player_StateFactory _factory;

    public Player_BaseState(Player_State_Machine currentContext, Player_StateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    protected void SwitchState(Player_BaseState newState)
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentState = newState;
    }

    protected void SetSuperState()
    {
    }

    protected void SetSubState()
    {
    }
}