using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory 
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext) //a constructor because this is a new instance of a class 
    {
        _context = currentContext;


    }

    public PlayerBaseState Idle() 
    {

        return new PlayerIdleState(_context,this);
    }
    public PlayerBaseState Walk() 
    {
        return new PlayerWalkState(_context, this);

    }

    public PlayerBaseState Run() 
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerBaseState Jump() 
    {
        return new PlayerJumpState(_context, this);
    }
    public PlayerBaseState Grounded() 
    {
        return new PlayerGroundState(_context, this);
    }
    public PlayerDodgeState Dodge()
    {
        return new PlayerDodgeState(_context, this);
    }








}
