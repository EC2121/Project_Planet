public class Player_StateFactory
{
    private Player_State_Machine _context;

    public Player_StateFactory(Player_State_Machine currentContext)
    {
        _context = currentContext;
    }

    public Player_BaseState Idle()
    {
        return new Player_IdleState(_context,this);
    }

    public Player_BaseState Walk()
    {
        return new Player_WalkState(_context,this);
    }

    public Player_BaseState Jump()
    {
        return new Player_JumpState(_context,this);
    }

    public Player_BaseState Run()
    {
        return new Player_RunState(_context,this);
    }

    public Player_BaseState Grounded()
    {
        return new Player_GroundState(_context,this);
    }

    public Player_BaseState SwitchWeapon()
    {
        return new Player_SwitchWeaponState(_context,this);
    }
    public Player_BaseState StaffAttack()
    {
        return new Player_StaffAttack(_context,this);
    }

    public Player_BaseState Hitted()
    {
        return new Player_HittedState(_context, this);
    }
    
    public Player_BaseState Interactable()
    {
        return new Player_Interactable(_context, this);
    }

    public Player_BaseState Dead()
    {
        return new Player_Dead(_context, this);
    }
    
    public Player_BaseState RunAttack()
    {
        return new Player_RunAttack(_context, this);
    }
    public Player_BaseState JumpAttack()
    {
        return new Player_JumpAttack(_context, this);
    }
}
