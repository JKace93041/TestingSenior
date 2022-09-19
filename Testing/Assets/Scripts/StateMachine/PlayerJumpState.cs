using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();

    }
    public override void EnterState() 
    {
        HandleJump();
    }
    public override void UpdateState() 
    {
        CheckSwitchState();
        HandleGravity();
    }

    public override void ExitState() 
    {
        _ctx._animator.SetBool(_ctx._isJumpingHash, false);
        if (_ctx._isJumpPressed)
        {
            _ctx._requireNewJumpPress = true;

        }

    }

    public override void CheckSwitchState() 
    {
        if (_ctx._characterController.isGrounded)
        {
            SwitchStates(_factory.Grounded());
        }
    
    }


    public override void InitializeSubState() { }
    
    void HandleJump()
    {


        _ctx._animator.SetBool(_ctx._isJumpingHash, true);

     
        _ctx._isJumping = true;
        _ctx._jumpingVelocity = Mathf.Sqrt(_ctx._jumpHeight * -3.0f * _ctx._gravity);
        _ctx._playerVelocityY = _ctx._moveDirectionY;
        _ctx._playerVelocityY = _ctx._jumpingVelocity;
    }
    void HandleGravity()
    {
        _ctx._moveDirectionY += _ctx._gravity * Time.deltaTime;

    }
}
