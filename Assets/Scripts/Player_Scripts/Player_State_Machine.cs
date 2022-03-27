using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_State_Machine : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed = 3f;
    [SerializeField] private Transform weapon;
    [SerializeField] private float jumpSpeed = 10f;
    
    private Animator anim;
    private int isWalkingHash;
    private int isRunningHash;
    private int equipHash;
    private int isJumpingHash;
    private int isLandingHash;
    private int velocityHash_X;
    private int velocityHash_Y;
    private int velocityHash_Z;
    private int fallingSpeedHash;

    private Player_Controller input;
    private CharacterController characterController;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    private bool switchWeapon = false;
    private float runSpeed = 1.2f;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;
    private bool isJumped;
    private Vector3 playerPos;
    private Player_BaseState _currentState;
    private Player_StateFactory _states;
    private bool requireNewjumpPress = false;
    private float groundGravity = -0.05f;
    private float fallingSpeed;
    //getters and setters
    public Player_BaseState CurrentState { get { return _currentState;} set { _currentState = value;}}
    public CharacterController CharacterController { get { return characterController;} set { characterController = value;}}
    public Animator Animator { get { return anim;}}
    public int IsJumpingHash  { get { return isJumpingHash;}}
    public float JumpSpeed  { get { return jumpSpeed;}}
    public bool IsJumping  { set { isJumped = value;}}
    public bool IsJumpPressed  { get { return isJumpPressed;} set {isJumped = value;}}
    public bool RequireJumpPress { get { return requireNewjumpPress;} set { requireNewjumpPress = value;}}
    public float CurrentMovementY { get { return currentMovement.y;} set { currentMovement.y = value;}}
    public float CurrentRunMovementY { get { return currentRunMovement.y;} set { currentRunMovement.y = value;}}
    public int VelocityHash_X  { get { return velocityHash_X;}}
    public int VelocityHash_Y  { get { return velocityHash_Y;}}
    public int VelocityHash_Z { get { return velocityHash_Z;}}
    public Vector3 PlayerPos { get { return transform.position;}}
    public int IsLandingHash { get { return isLandingHash;}}
    public bool IsMovementPressed  { get { return isMovementPressed;}}
    public bool IsRunPressed  { get { return isRunPressed;}}
    public int IsWalkingHash { get { return isWalkingHash;}}
    public int IsRunningHash { get { return isRunningHash;}}
    public float GroundGravity  { get { return groundGravity;}}


    void Awake()
    {
        input = new Player_Controller();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        equipHash = Animator.StringToHash("Equip");
        isJumpingHash = Animator.StringToHash("Jump");
        isLandingHash = Animator.StringToHash("Land");
        velocityHash_X = Animator.StringToHash("VelocityX");
        velocityHash_Y = Animator.StringToHash("VelocityY");
        velocityHash_Z = Animator.StringToHash("VelocityZ");
        fallingSpeedHash = Animator.StringToHash("FallingSpeed");

        sockets = GetComponent<Handle_Mesh_Sockets>();

        _states = new Player_StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        
        input.Player.Move.started += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;

        input.Player.Run.started += OnRun;
        input.Player.Run.canceled += OnRun;

        input.Player.Switch.started += SwitchWeapon;
        input.Player.Switch.canceled += SwitchWeapon;

        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;
        input.Player.Jump.performed += OnJump;

        
        isWeaponAttached = false;
        isJumped = false;
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
        requireNewjumpPress = false;
       // Debug.Log(requireNewjumpPress);
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void Update()
    {
        ActivateWeapon();
        HandleMovement();
        _currentState.UpdateStates();
    }
    void HandleMovement( )
    {
        Vector3 verticalMovement = Vector3.up *  currentMovement.y;

        Vector3 move = new Vector3(currentAnimationBlend.x,0, currentAnimationBlend.y);
        currentAnimationBlend =
            Vector2.SmoothDamp(currentAnimationBlend, currentMovementInput, ref animationVelocity, 0.15f);
        move.y = verticalMovement.y;
        if (isRunPressed)
        {
            characterController.Move(move * Time.deltaTime * PlayerSpeed * runSpeed);
        }
        else
        {
            characterController.Move( (move * Time.deltaTime * PlayerSpeed));
        }
        
        fallingSpeed =Mathf.Clamp( Mathf.Abs(currentAnimationBlend.x + currentAnimationBlend.y), 0,1) * (isRunPressed ? runSpeed : 1);
        
        anim.SetFloat(velocityHash_X, currentAnimationBlend.x);
        anim.SetFloat(velocityHash_Z, currentAnimationBlend.y);
        anim.SetFloat(fallingSpeedHash,fallingSpeed);
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
   
    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}