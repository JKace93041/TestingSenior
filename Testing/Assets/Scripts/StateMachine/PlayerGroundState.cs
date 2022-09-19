using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        _isRootState = true;

        InitializeSubState();
    }
    public override void EnterState() 
    {
        _ctx._moveDirectionY = _ctx._groundedGravity;
        _ctx._playerVelocityY = _ctx._groundedGravity;
        Debug.Log("hello world");
    }
    public override void UpdateState() 
    {
        CheckSwitchState();

    }

    public override void ExitState() { }

    public override void CheckSwitchState() 
    {
        //if player is groudned and jump is pressed switch to jump state
        if (_ctx._isJumpPressed && !_ctx._requireNewJumpPress)
        {
            SwitchStates(_factory.Jump());
        }
    
    }


    public override void InitializeSubState() 
    
    {

        if (!_ctx._isMovementPressed && !_ctx._isRunPressed)
        {
            SetSubStates(_factory.Idle());
        }
        else if (_ctx._isMovementPressed && !_ctx._isRunPressed)
        {
            SetSubStates(_factory.Walk());
        }
        else if (_ctx._isMovementPressed && _ctx._isRunPressed)
        {
            SetSubStates(_factory.Run());

        }

    }
    // Start is called before the first frame update
  
}
