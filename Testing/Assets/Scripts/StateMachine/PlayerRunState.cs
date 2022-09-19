using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{


    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState() 
    
    {

        _ctx._animator.SetBool(_ctx._isRunningHash, true);
        _ctx._animator.SetBool(_ctx._isWalkingHash, true);
    }
    public override void UpdateState() 
    
    {
        CheckSwitchState();
        _ctx._moveDirectionX = _ctx._currentMovementInput.x;
        _ctx._moveDirectionZ = _ctx._currentMovementInput.y;
        //_ctx._moveDirectionY = 0f;
        _ctx._movementVelocity = _ctx._moveDirectionX * _ctx._cameraTransform.right.normalized + _ctx._moveDirectionZ * _ctx._cameraTransform.forward.normalized;

        _ctx._movementVelocity = _ctx._movementSpeed * _ctx._movementVelocity;
        Debug.Log("RunState");

    }

    public override void ExitState() 
    {
        //_ctx._animator.SetBool(_ctx._isWalkingHash, false);
        //_ctx._animator.SetBool(_ctx._isRunningHash, false);

    }

    public override void CheckSwitchState() 
    
    {
        if (!_ctx._isMovementPressed)
        {
            SwitchStates(_factory.Idle());
        }
        else if (_ctx._isMovementPressed && !_ctx._isRunPressed)
        {
            SwitchStates(_factory.Walk());

        }
        


    }


    public override void InitializeSubState() { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
