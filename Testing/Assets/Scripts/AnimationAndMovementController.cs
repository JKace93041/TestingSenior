using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    //component variables
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;

    //parameter ids
    int isWalkingHash;
    int isRunningHash;


    //store values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
  
    private bool isRunPressed;
    //constant
    float rotationFactorPerFrame = 15f;
    private float runMultiper = 3.0f;

    //gravitys
    float gravity = -9.81f;
    float groundedGravity = -.05f;

    //jumping
    bool isJumping = false;
    bool isJumpPressed = false;
    Vector3 playerVelocity;
    float jumpingVelocity;
    float jumpHeight = 1.0f;
    float maxJumpTime =3f;
   


    private void Awake()
    {
        //gets
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        //stringtohaash
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

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
        setupJumpVaribales();

    }

    void setupJumpVaribales()
    {
        //float timeToApex = maxJumpTime / 2;
        //gravity = (-2 * maxJumpheight) / Mathf.Pow(timeToApex, 2);
        //initialjumpingVelocity = (2 * maxJumpTime) / timeToApex; //watch building a better jump GDC
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        //handles movement inputs
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        //currentMovement.y = 0f;
        //currentRunMovement.y = 0f;
        currentRunMovement.x = currentMovementInput.x * runMultiper;
        currentRunMovement.z = currentMovementInput.y * runMultiper;
        currentMovement.y = 0f;
        currentRunMovement.y = 0f;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }
    void onRun(InputAction.CallbackContext context)
    {
        if (animator.GetBool(isWalkingHash) == false)
        {
            return;
        }
        isRunPressed = context.ReadValueAsButton();
    }
    void HandleJump()
    {

        
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            print("hi");
            isJumping = true;
            //currentMovement.y = initialjumpingVelocity * .5f;
            //currentRunMovement.y = initialjumpingVelocity * .5f;
            jumpingVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            playerVelocity = currentMovementInput;
            playerVelocity.y = jumpingVelocity;
            print("jump");
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
        //else if (!isJumpPressed && isJumping && characterController.isGrounded)
        //{
        //    isJumping = false;
        //}
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt; //where the player is moving next
        //we only rotate on the x n z so no y is needed
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        //current rotation
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

    }
    void HandleAnimation()
    {
        //paramter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            print(false);
            animator.SetBool(isWalkingHash, false);
        }
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);

        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
     void HandleGravity()
    {



        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        //gravity is always applied the charactercontroller is touching the ground

        if (characterController.isGrounded)
        {
          
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;

               
        }
        //else if (isFalling)
        //{
        //    float previousYVelocity = currentMovement.y;
        //    float newYVelocity = currentMovement.y + (gravity * fallMultiplier *  Time.deltaTime);
        //    float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
        //    currentMovement.y = nextYVelocity;
        //    currentRunMovement.y = nextYVelocity;
        //}
        //else
        //{
        //    float previousYVelocity = currentMovement.y;
        //    float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
        //    float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
        //    currentMovement.y = nextYVelocity;
        //    currentRunMovement.y = nextYVelocity;
        //}

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(characterController.isGrounded);
        HandleRotation();
        HandleAnimation();
        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
            

        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);

        }
        playerVelocity.y += groundedGravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        HandleGravity();
        HandleJump();
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
