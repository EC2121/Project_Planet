using UnityEngine;
using UnityEngine.Rendering;

public abstract class Player_BaseState
{
    private Player_State_Machine _ctx;
    private Player_StateFactory _factory;
    private Player_BaseState _currentSubState;
    private Player_BaseState _currentSuperState;
    private bool _isRootState = false;
    
    protected bool IsRootState { set {_isRootState = value;}}
    protected Player_State_Machine Context { get { return  _ctx;}}
    protected Player_StateFactory Factory { get { return  _factory;}}

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
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }
    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitState();
        }
    }
    protected void SwitchState(Player_BaseState newState)
    {
        ExitState();

        newState.EnterState();
        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(Player_BaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(Player_BaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}