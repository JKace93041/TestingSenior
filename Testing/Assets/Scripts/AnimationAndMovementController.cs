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
    Transform cameraTransform;

    //parameter ids
    int isWalkingHash;
    int isRunningHash;


    //store values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 moveDirection;
    //Vector3 currentRunMovement;
    bool isMovementPressed;
  
    private bool isRunPressed;
    //constant
    float rotationFactorPerFrame = 15f;
    private float runMultiper = 3.0f;
    public float movementSpeed = 7f;
    public float walkingspeed = 1.5f;
    private float rotationSpeed = 15f;




    //gravitys
    float gravity = -9.81f;
    float groundedGravity = -.05f;
    Vector3 movementVelocity;
    //jumping
    bool isJumping = false;
    bool isJumpPressed = false;
    Vector3 playerVelocity;
    float jumpingVelocity;
    float initialJumpingVelocity;
    float jumpHeight = 3.0f;
    float maxJumpHeight = 4.0f;
    float maxJumpTime = 1f;
    

    private void Awake()
    {
        //gets
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

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
        //setupJumpVaribales();
    }

    void setupJumpVaribales()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpingVelocity = (2 * maxJumpTime) / timeToApex; //watch building a better jump GDC
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
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
        if (animator.GetBool(isWalkingHash) == false)
        {
            isRunPressed = false; //button cant be true unless walking
            return;
        }
        else
        {
            isRunPressed = context.ReadValueAsButton();
        }
      
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
    void HandleJump()
    {

        
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            
            isJumping = true;
            jumpingVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
       
    }

    void HandleRotation()
    {

        //if (currentMovementInput != Vector2.zero)
        ////{
        //    float tartgetAngle = cameraTransform.eulerAngles.y; // current y rotation of the camera;
        //    Quaternion targetRotation = Quaternion.Euler(0, tartgetAngle, 0); //turns it into a quaternion
        //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //}
        if (currentMovementInput != Vector2.zero)
        {

            float targetAngle = Mathf.Atan2(currentMovementInput.x, currentMovementInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation= Quaternion.Euler(0f, targetAngle, 0f);


            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);



        }




        //Vector3 positionToLookAt; //where the player is moving next
        //                          //we only rotate on the x n z so no y is needed
        //positionToLookAt = currentMovement;
        //positionToLookAt.y = 0f;
        ////current rotation
        //Quaternion currentRotation = transform.rotation;
        //Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

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
          
            moveDirection.y = groundedGravity;
            //currentRunMovement.y = groundedGravity;

               
        }
        //else if (isFalling)
        //{
        //    float previousYVelocity = currentMovement.y;
        //    float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
        //    float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
        //    currentMovement.y = nextYVelocity;
        //    currentRunMovement.y = nextYVelocity;
        //}
        else
        {

            moveDirection.y += gravity * Time.deltaTime;
            //currentRunMovement.y += gravity * Time.deltaTime;



            //float previousYVelocity = currentMovement.y;
            //float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            //float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            //currentMovement.y = nextYVelocity;
            //currentRunMovement.y = nextYVelocity;

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        HandleMovement();
        HandleRotation();
        HandleAnimation();
       
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
