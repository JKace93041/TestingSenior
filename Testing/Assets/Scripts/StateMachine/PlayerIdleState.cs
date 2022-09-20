using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState() 
    
    {

        _ctx._animator.SetBool(_ctx._isWalkingHash, false);
        _ctx._animator.SetBool(_ctx._isRunningHash, false);
        _ctx._moveDirectionZ = 0f;
        _ctx._moveDirectionX = 0f;
        if (_ctx._moveDirectionZ == 0f && _ctx._moveDirectionX == 0f)
        {
            _ctx._animator.SetBool(_ctx._isWalkingHash, false);
            _ctx._animator.SetBool(_ctx._isRunningHash, false);
        }

    }
    public override void UpdateState() 
    
    {

        CheckSwitchState();
        //Debug.Log("IdleState");


    }

    public override void ExitState() { }

    public override void CheckSwitchState() 
    
    {

        if (_ctx._isMovementPressed && _ctx._isRunPressed)
        {
            SwitchStates(_factory.Run());

        }
        else if (_ctx._isMovementPressed)
        {
            SwitchStates(_factory.Walk());
        }
        else if (_ctx._isDodgePressed)
        {
            SwitchStates(_factory.Dodge());
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
