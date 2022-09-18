using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    private void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;

    }
    void HandleAnimation()
    {


    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovement.y != 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
        characterController.Move(currentMovement * Time.deltaTime);
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
