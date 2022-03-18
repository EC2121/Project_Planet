using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player_Controller_Move : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed = 3f;
    [SerializeField] private Transform weapon;

    private Animator anim;
    private int isWalkingHash;
    private int isRunningHash;
    private int equipHash;
    private Player_Controller input;
    private CharacterController characterController;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;

    private bool isMovementPressed;
    private bool isRunPressed;
    private bool switchWeapon = false;
    private float rotationFactor = 15f;
    private float runSpeed = 1.5f;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;

    private void Awake()
    {
        input = new Player_Controller();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        equipHash = Animator.StringToHash("Equip");

        sockets = GetComponent<Handle_Mesh_Sockets>();

        input.Player.Move.started += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;

        input.Player.Run.started += OnRun;
        input.Player.Run.canceled += OnRun;
        
        input.Player.Switch.started += SwitchWeapon;
        input.Player.Switch.canceled += SwitchWeapon;
      

        // input.CharacterControls.Run.started += OnRun;
        isWeaponAttached = false;

    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void SwitchWeapon(InputAction.CallbackContext context)
    {
        switchWeapon = context.ReadValueAsButton();
        
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        // currentMovement.x = currentMovementInput.x;
        // currentMovement.z = currentMovementInput.y;

        //  currentMovement.x = Mathf.SmoothDamp(old, currentMovementInput.x, ref , prova *Time.deltaTime*20 );
        // currentMovement.z = Mathf.SmoothDamp(meow, currentMovementInput.y, ref ciao, prova *Time.deltaTime*20);


        // anim.SetFloat("VelocityX", currentMovement.x);
        // anim.SetFloat("VelocityZ", currentMovement.z);
        // velocityX = Mathf.Lerp(velocityX, input.x * currentMaxVelocity, Time.deltaTime * acceleration);
        //  velocityZ = Mathf.Lerp(velocityZ, input.y * currentMaxVelocity, Time.deltaTime * acceleration);
        // // currentMovement.x = Mathf.Lerp(currentMovement.x, currentMovementInput.x, prova*Time.deltaTime);
        // currentMovement.z = Mathf.Lerp(currentMovement.z, currentMovementInput.y, prova*Time.deltaTime);

        //currentMovement = Vector2.SmoothDamp(currentMovement, currentMovementInput, ref ciao, prova);

        // currentRunMovement.x = currentMovementInput.x * runSpeed;
        // currentRunMovement.z = currentMovementInput.y * runSpeed;

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
            Debug.Log("Ciao");

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
        if (characterController.isGrounded)
        {
            float groundGravity = -0.05f;
            currentMovement.y = groundGravity;
            currentRunMovement.y = groundGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    void HandleMovement()
    {
        if (isRunPressed)
        {
            Vector3 move = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);

            characterController.Move(move * Time.deltaTime);
        }
        else
        {
            currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, currentMovementInput, ref animationVelocity, 0.15f);

            Vector3 move2 = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);
            characterController.Move(move2 * Time.deltaTime * PlayerSpeed);

            anim.SetFloat("VelocityX", currentAnimationBlend.x);
            anim.SetFloat("VelocityZ", currentAnimationBlend.y);
        }
    }
    // Update is called once per frame
    void Update()
    {
        ActivateWeapon();
        HandleGravity();
        //HandleRotation();
        HandleAnimation();
        HandleMovement(); 
        Debug.Log(switchWeapon);

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