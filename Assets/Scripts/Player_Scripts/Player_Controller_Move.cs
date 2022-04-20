using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller_Move : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed = 3f;
    [SerializeField] private Transform weapon;
    [SerializeField] private float jumpSpeed = 1f;


    private Animator anim;
    private int isWalkingHash;
    private int isRunningHash;
    private int equipHash;
    private int isJumpingHash;
    private int isLandingHash;
    private Player_Controller input;
    private CharacterController characterController;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;

    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    private bool switchWeapon = false;
    private float rotationFactor = 15f;
    private float runSpeed = 1.2f;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;
    private float speedY = 0;
    private bool isJumped;
    private void Awake()
    {
        input = new Player_Controller();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        equipHash = Animator.StringToHash("Equip");
        isJumpingHash = Animator.StringToHash("Jump");
        isLandingHash = Animator.StringToHash("Land");

        sockets = GetComponent<Handle_Mesh_Sockets>();

        input.Player.Move.started += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;

        input.Player.Run.started += OnRun;
        input.Player.Run.canceled += OnRun;

        input.Player.Switch.started += SwitchWeapon;
        input.Player.Switch.canceled += SwitchWeapon;

        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;
        
        isWeaponAttached = false;
        isJumped = false;
        //characterController.detectCollisions = true;
        // input.CharacterControls.Run.started += OnRun;
       
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void SwitchWeapon(InputAction.CallbackContext context)
    {
        switchWeapon = context.ReadValueAsButton();
        
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    public void ActivateWeapon()
    {
        if (switchWeapon && !isWeaponAttached && !isMovementPressed)
        {
            isWeaponAttached = true;
            switchWeapon = false;
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.Spine);
            anim.SetBool(equipHash, true);
        }
        else if (switchWeapon && isWeaponAttached && !isMovementPressed)
        {
            isWeaponAttached = false;
            switchWeapon = false;

            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.RightHand);
            anim.SetBool(equipHash, false);
        }
    }

    public void OnAnimationEvent(string eventName)
    {
        if (eventName == "EquipWeapon")
        {
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.RightHand);
        }

        if (eventName == "Detach")
        {
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.Spine);
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currRotation, targetRotation, rotationFactor * Time.deltaTime);
        }
    }

    void HandleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);


        if (isMovementPressed && !isWalking)
        {
            anim.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            anim.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            anim.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            anim.SetBool(isRunningHash, false);
        }
        
    }

    void HandleGravity()
    {

        if (isJumpPressed && !isJumped)
        {
            isJumped = true;
            anim.SetTrigger(isJumpingHash);
            speedY += jumpSpeed;
            currentMovement.y += jumpSpeed;
        }
        if (!characterController.isGrounded)
        {
            float gravity = -9.81f;
            currentMovement.y += gravity * Time.deltaTime;
            currentRunMovement.y += gravity;
        }
        else if( currentMovement.y < 0)
        {
            float groundGravity = -0.05f;
            currentMovement.y = -0.05f;
            currentRunMovement.y = groundGravity;
        }
        anim.SetFloat("VelocityY", currentMovement.y/jumpSpeed);

       
        Debug.Log(speedY);
        if (isJumped &&  currentMovement.y < 0)
        {
            Debug.Log("ciaooo");

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f,LayerMask.GetMask("Default")))
            {
                isJumped = false;
                anim.SetTrigger(isLandingHash);
            }
        }
    }

    void HandleMovement(Vector3 verticalMovement)
    {
        Vector3 move = new Vector3(currentAnimationBlend.x,0, currentAnimationBlend.y);
        currentAnimationBlend =
            Vector2.SmoothDamp(currentAnimationBlend, currentMovementInput, ref animationVelocity, 0.15f);
        move.y = verticalMovement.y;
        if (isRunPressed)
        {
            characterController.Move(move * Time.deltaTime * PlayerSpeed * runSpeed);

            // //RotatePlayerToCamera
            // Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move((   (move * Time.deltaTime * PlayerSpeed)));
        }

        anim.SetFloat("VelocityX", currentAnimationBlend.x);
        anim.SetFloat("VelocityZ", currentAnimationBlend.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 verticalMovement = Vector3.up *  currentMovement.y;

        HandleGravity();
        ActivateWeapon();
        //HandleRotation();
        HandleAnimation();
        HandleMovement(verticalMovement);

        //Debug.Log(speedY);

    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}