using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerStateMachine : MonoBehaviour
{

    private TextMeshProUGUI textMesh;

    //state variables
    public PlayerBaseState _currentState;
    PlayerStateFactory  _states;
    


    //component variables
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;
    Transform cameraTransform;

    //parameter ids
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isDodgeingHash;
    int horizontalHash;
    public int dodgeAnimation;
    int verticalHash;
    AnimatorStateInfo animStateInfo;
    public float NTime;

    bool animationFinished;

    //store values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 moveDirection;
    //Vector3 currentRunMovement;


    //IsPressedBools
    bool isMovementPressed;
    bool isRunPressed;
    bool isDodgePressed;
    bool isJumpPressed = false;

    //isDoing
    bool isJumpingAnimating;
    bool requireNewJumpPress = false;
    bool requireNewDodgePress = false;

    bool isJumping = false;
    bool isWalking;
    bool isRunning;
    bool isDodgeing = false;

    //constant
    float rotationFactorPerFrame = 15f;
    private float runMultiper = 3.0f;
    public float movementSpeed = 7f;
    public float walkingspeed = 1.5f;
    private float rotationSpeed = 15f;


    //animation blends
    public Vector2 currentAnimationBlendVector;
    public Vector2 animationVelocity;
    private float animationSmoothTime = .1f;
    public float animationPlayTransition = .15f;
    InputAction dodgeAction;
    //gravitys
    float gravity = -9.81f;
    float groundedGravity = -.05f;
    Vector3 movementVelocity;
    //jumping
    Vector3 playerVelocity;
    float jumpingVelocity;
    float initialJumpingVelocity;
    float jumpHeight = 1.0f;
    float maxJumpHeight = 4.0f;
    float maxJumpTime = 1f;
    private bool DodgeInput;
    //getters and setters
    public PlayerBaseState CurrenttState { get { return _currentState; } set { _currentState = value; } }
    public bool _isJumpPressed { get { return isJumpPressed; } }
    public bool _isMovementPressed { get { return isMovementPressed; } }
    public bool _isRunPressed { get { return isRunPressed; } }

    public bool _isDodgePressed { get { return isDodgePressed; } }
    public Animator _animator { get { return animator; } }
    public CharacterController _characterController { get { return characterController; } }

    public int _isJumpingHash { get { return isJumpingHash; } }
    public int _isWalkingHash { get { return isWalkingHash; } }
    public int _isRunningHash { get { return isRunningHash; } }
    public int _IsDodgingHash { get { return isDodgeingHash; } }
    public bool _isDodging { get { return isDodgeing; } set {  isDodgeing = value; } }

    public bool _isJumping { set { isJumping = value; } }
    public bool _requireNewJumpPress { get  { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public float _jumpingVelocity { get { return jumpingVelocity; } set { jumpingVelocity = value; } }
    public float _jumpHeight { get { return jumpHeight;  } }
    public float _gravity { get { return gravity; } }
    public float _groundedGravity { get { return groundedGravity; } }
    public float _playerVelocityY { get { return playerVelocity.y; } set { playerVelocity.y = value; } }
    public float _moveDirectionY { get { return moveDirection.y; } set { moveDirection.y = value; } }
    public float _moveDirectionX { get { return moveDirection.x; } set { moveDirection.x = value; } }
    public float _moveDirectionZ { get { return moveDirection.z; } set { moveDirection.z = value; } }
    public float _currentMovementY { get { return currentMovementInput.y; } set { currentMovementInput.y = value; } }
    public Transform _cameraTransform { get { return cameraTransform; } }
    public Vector2 _currentMovementInput { get { return currentMovementInput; } }
    public float _movementSpeed { get { return movementSpeed; } }
    public float _walkingSpeed { get { return walkingspeed; } }
    public bool _requireNewDodgePress { get { return requireNewDodgePress; } set { requireNewDodgePress = value; } }

    public InputAction _dodgeAction { get { return dodgeAction; } }
    public int _dodgeAnimation { get { return dodgeAnimation; } }

    public Vector3 _movementVelocity { get { return movementVelocity; }set { movementVelocity = value; } }

    //public Vector2 _CurrentMovementInput { get { return currentMovementInput; } }


private void Awake()
    {
        textMesh = FindObjectOfType<TextMeshProUGUI>();

        //gets
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        //playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
       

        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        NTime = animStateInfo.normalizedTime;
        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();


        //stringtohaash
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");
        isJumpingHash = Animator.StringToHash("isJumping");
        isDodgeingHash = Animator.StringToHash("isDodgeing");
        dodgeAnimation = Animator.StringToHash("Dodge");

        dodgeAction = playerInput.CharacterControls.Dodge;

        //movement
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        //run
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        //jump
        playerInput.CharacterControls.Jump.started += onJump;      
        playerInput.CharacterControls.Jump.canceled += onJump;
        //dodge
        playerInput.CharacterControls.Dodge.started += onDodge;
        playerInput.CharacterControls.Dodge.canceled += onDodge;

        //setupJumpVaribales();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        //handles movement inputs
        currentMovementInput = context.ReadValue<Vector2>(); //read values of inputs
        currentMovement.x = currentMovementInput.x; //stores the vector 2 inputs in a vector 3
        currentMovement.z = currentMovementInput.y;
        HandleMovement();
        //currentMovement.y = 0f;
        //currentRunMovement.y = 0f;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0; //how to tell if the player is moving

    }
    void onRun(InputAction.CallbackContext context)
    {
        //if (animator.GetBool(isWalkingHash) == false)
        //{
        //    isRunPressed = false; //button cant be true unless walking
        //    return;
        //}
        //else
        //{
            isRunPressed = context.ReadValueAsButton();
        //}

    }

    void onDodge(InputAction.CallbackContext context)
    {
        isDodgePressed = context.ReadValueAsButton();
        requireNewDodgePress = false;

    }

    void HandleMovement()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 moveDirection = new Vector3(currentMovementInput.x, 0, currentMovementInput.y); // putector3 moveDirection = new Vector3(currentMovement.x, 0, currentMovement.z);
        moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
        moveDirection.y = 0f; //weird interactions if the y isnt set the 0 the character doesnt move on the Y axis so its set to zero
        Vector3 movemenVelocity = moveDirection;
        if (isRunPressed)
        {

            movementVelocity = moveDirection * movementSpeed;

        }
        else
        {
            isRunPressed = false;
            movementVelocity = moveDirection * walkingspeed;

            //characterController.Move(currentMovement * Time.deltaTime);

        }

        characterController.Move(movementVelocity * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

    }
    public void ControlAnimatorValues()
    {

        //Animation snap will force either the walk or running

        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, _currentMovementInput, ref animationVelocity, animationSmoothTime);
        animator.SetFloat(horizontalHash, currentAnimationBlendVector.x);
        animator.SetFloat(verticalHash, currentAnimationBlendVector.y);

    }
    void HandleRotation()
    {

        if (currentMovementInput != Vector2.zero)
        {

            float targetAngle = Mathf.Atan2(currentMovementInput.x, currentMovementInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            //
            //
            //
            //   float tartgetAngle = cameraTransform.eulerAngles.y; // current y rotation of the camera;
            //    Quaternion targetRotation = Quaternion.Euler(0, tartgetAngle, 0); //turns it into a quaternion
            //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        //if (currentMovementInput != Vector2.zero)
        //{
        //    Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);


        //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //    //float targetAngle = Mathf.Atan2(currentMovementInput.x, currentMovementInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        //    //Quaternion rotation= Quaternion.Euler(0f, targetAngle, 0f);


        //    //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);



        //}




        //Vector3 positionToLookAt; //where the player is moving next
        //                          //we only rotate on the x n z so no y is needed
        //positionToLookAt = currentMovement;
        //positionToLookAt.y = 0f;
        ////current rotation
        //Quaternion currentRotation = transform.rotation;
        //Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        HandleMovement();
        HandleRotation();
        ControlAnimatorValues();
        textMesh.SetText(_currentState.ToString());
        print(_currentState);
        //print(_isMovementPressed);
        Debug.Log("character controller is: "+ characterController.isGrounded);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
        //Debug.Log(_moveDirectionZ + "  " + "  " + _moveDirectionX);

    }
    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();

    }
}
