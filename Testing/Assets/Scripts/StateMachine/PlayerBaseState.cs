
public abstract class PlayerBaseState 
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSubState;
    protected PlayerBaseState _currentSuperState;
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext; //variable of playerstatemachine so i can access things from that script
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();


    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }

    
    }
    protected  void SwitchStates(PlayerBaseState newState) 
    {
        //Exit Current State
        ExitState();
        // new state enters state
        newState.EnterState();


        if (_isRootState)
        {
            // switch current state of context
            _ctx.CurrenttState = newState; //which is what i did here using a getter and setter
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubStates(newState);
        }


       
        
    
    }
   protected void SetSuperStates(PlayerBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    
    }
   protected void SetSubStates(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperStates(this);
    }










}
