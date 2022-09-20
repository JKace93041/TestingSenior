using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{


    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
         :base(currentContext, playerStateFactory)
    {
        _isRootState = true;

        InitializeSubState();
    }


    public override void EnterState()
    {
        HandleDodge();
    }



    public override void UpdateState()
    {
        CheckSwitchState();
        //Debug.Log(_ctx._isDodging);

    }

    public override void ExitState()
    {
        _ctx._animator.SetBool(_ctx._IsDodgingHash, false);
        //Debug.Log("hi");

        if (_ctx._dodgeAction.triggered)
        {
            _ctx._requireNewDodgePress = true;

        }
    }



   

    public override void CheckSwitchState()
    {
        if (_ctx._characterController.isGrounded)
            if (_ctx._animator.GetCurrentAnimatorStateInfo(_ctx._IsDodgingHash).normalizedTime < 1.0f)
            {


                SwitchStates(_factory.Grounded());

            }
        //{
        //    SwitchStates(_factory.Grounded());
        //}
        else if (_ctx._characterController.isGrounded && _ctx._isJumpPressed && !_ctx._requireNewJumpPress)
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
        if (!_ctx._isDodging)
        {
            if (_ctx._isMovementPressed && !_ctx._isRunPressed)
            {
                SetSubStates(_factory.Walk());
            }
            else if (_ctx._isMovementPressed && _ctx._isRunPressed)
            {
                SetSubStates(_factory.Run());

            }
        }
    }


    void HandleDodge()
    {
        _ctx._isDodging = true;
        Debug.Log(_ctx._isDodging);

        //_ctx._animator.Play(_ctx.dodgeAnimation);
        _ctx._animator.CrossFade(_ctx.dodgeAnimation,.15f);
        if (_ctx._animator.GetCurrentAnimatorStateInfo(_ctx._IsDodgingHash).normalizedTime < 1.0f)
        {
        _ctx._isDodging = false;

            Debug.Log("is doding is:"+ _ctx._isDodging);
        }
        //_ctx._animator.SetBool(_ctx._IsDodgingHash, true);
        //Debug.Log("I am in dodge");






    }
}
