using System;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class Player_State_Machine : MonoBehaviour
{
    public static UnityEvent TakeTheBox = new UnityEvent();

    public Transform StaffJoint;
    [SerializeField] private float PlayerSpeed = 3f;
    [SerializeField] private Transform weapon;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runSpeed = 2.65f;
    [SerializeField] private float rotationFactor = 10f;

    private Animator anim;
    private int isWalkingHash;
    private int isRunningHash;
    private int equipHash;
    private int isJumpingHash;
    private int isLandingHash;
    private int velocityHash_X;
    private int velocityHash_Y;
    private int velocityHash_Z;
    private int attackIndexHash;
    private int fallingSpeedHash;

    private Player_Controller input;
    private CharacterController characterController;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isMousePressed;
    private bool isJumpPressed;
    private bool switchWeapon = false;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;
    private bool isJumped;
    private Vector3 playerPos;
    private Player_BaseState _currentState;
    private Player_StateFactory _states;
    private bool requireNewWeaponSwitch = false;
    private bool requireNewAttack = false;
    private float groundGravity = -0.05f;
    private float fallingSpeed;
    private AnimatorStateInfo stateInfo;
    private int attackId = 0;
    private Vector3 move;
    private bool isInteract = false;
    private bool hasBox = false;
    //public Transform prova;

    //getters and setters
    public Player_BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return characterController; } set { characterController = value; } }
    public Animator Animator { get { return anim; } }
    public int IsJumpingHash { get { return isJumpingHash; } }
    public float JumpSpeed { get { return jumpSpeed; } }
    public bool IsJumping { set { isJumped = value; } }
    public bool IsJumpPressed { get { return isJumpPressed; } set { isJumpPressed = value; } }
    public bool RequireNewWeaponSwitch { get { return requireNewWeaponSwitch; } set { requireNewWeaponSwitch = value; } }
    public bool RequireNewAttack { get { return requireNewAttack; } set { requireNewAttack = value; } }
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; } }
    public float CurrentRunMovementY { get { return currentRunMovement.y; } set { currentRunMovement.y = value; } }
    public int VelocityHash_X { get { return velocityHash_X; } }
    public int VelocityHash_Y { get { return velocityHash_Y; } }
    public int VelocityHash_Z { get { return velocityHash_Z; } }
    public int AttackIndexHash { get { return attackIndexHash; } set { attackIndexHash = value; } }
    public Vector3 PlayerPos { get { return transform.position; } }
    public int IsLandingHash { get { return isLandingHash; } }
    public int EquipHash { get { return equipHash; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsMousePressed { get { return isMousePressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public int IsWalkingHash { get { return isWalkingHash; } }
    public int IsRunningHash { get { return isRunningHash; } }
    public float GroundGravity { get { return groundGravity; } }
    public bool IsSwitchPressed { get { return switchWeapon; } set { switchWeapon = value; } }
    public bool IsWeaponAttached { get { return isWeaponAttached; } set { isWeaponAttached = value; } }
    public Handle_Mesh_Sockets Sockets { get { return sockets; } }
    public Transform Weapon { get { return weapon; } }
    public AnimatorStateInfo AnimStateInfo { get { return stateInfo; } }
    public int AttackId { get { return attackId; } set { attackId = value; } }
    public Vector3 Move { get { return move; } set { move = value; } }
    
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
        attackIndexHash = Animator.StringToHash("AttackIndex");
        sockets = GetComponent<Handle_Mesh_Sockets>();

        _states = new Player_StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        input.Player.Move.started += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;

        input.Player.Run.started += OnRun;
        input.Player.Run.canceled += OnRun;
        input.Player.Run.performed += OnRun;

        input.Player.Switch.started += OnSwitchWeapon;
        input.Player.Switch.canceled += OnSwitchWeapon;

        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;
        input.Player.Jump.performed += OnJump;

        input.Player.L_MouseClick.started += OnMousePressed;
        input.Player.L_MouseClick.performed += OnMousePressed;
        input.Player.L_MouseClick.canceled += OnMousePressed;

        input.Player.F_Interact.started += OnInteract;
        input.Player.F_Interact.canceled += OnInteract;

        isWeaponAttached = false;
        isJumped = false;
    }
    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnMousePressed(InputAction.CallbackContext context)
    {
        isMousePressed = context.ReadValueAsButton();
        requireNewAttack = false;
    }

    void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        switchWeapon = context.ReadValueAsButton();
        requireNewWeaponSwitch = false;
    }
    void OnInteract(InputAction.CallbackContext context)
    {
        isInteract = context.ReadValueAsButton();
        //requireNewWeaponSwitch = false;
    }
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;


        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void Update()
    {
        // HandleMovement();
        HandleRotation();
        HandleGravity();
        _currentState.UpdateStates();
        // ActivateWeapon();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        fallingSpeed = Mathf.Clamp(Mathf.Abs(currentMovementInput.x + currentMovementInput.y), 0, 1) * (isRunPressed ? runSpeed : 1);

        anim.SetFloat(velocityHash_X, currentMovement.x);
        anim.SetFloat(velocityHash_Z, currentMovement.z);
        anim.SetFloat(fallingSpeedHash, fallingSpeed);

        if (isInteract && !hasBox)
        {
           // isInteract = false;
         
            hasBox = true;
            anim.SetBool("HasBox", true);
            TakeTheBox.Invoke();
        }
        else if (isInteract && hasBox)
        {
           
            hasBox = false;
            anim.SetBool("HasBox", false);
            TakeTheBox.Invoke();
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
    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.gameObject.tag =="Player")
    //     {
    //         Debug.Log("CIAoasdasdasd");
    //
    //     }
    // }

    void HandleMovement()
    {
        Vector3 verticalMovement = Vector3.up * currentMovement.y;
        currentAnimationBlend =
            Vector2.SmoothDamp(currentAnimationBlend, currentMovementInput, ref animationVelocity, 0.15f);
        move = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);

        move.y = verticalMovement.y;
        fallingSpeed = Mathf.Clamp(Mathf.Abs(currentAnimationBlend.x + currentAnimationBlend.y), 0, 1) * (isRunPressed ? runSpeed : 1);
        characterController.Move((move * PlayerSpeed * (isRunPressed ? runSpeed : 1) * Time.deltaTime));

        anim.SetFloat(velocityHash_X, move.x);
        anim.SetFloat(velocityHash_Z, move.y);
        anim.SetFloat(fallingSpeedHash, fallingSpeed);
    }
    void HandleGravity()
    {
        float gravity = -9.81f;
        currentMovement.y += gravity * Time.deltaTime;
        currentRunMovement.y += gravity * Time.deltaTime;
    }
    private void OnAnimatorMove()
    {

        //transform.Rotate(0,currentMovementInput.x,0);
        //transform.rotation = anim.rootRotation;
        //transform.Translate(0, 0, move.z *0.05f);
        //transform.position = anim.rootPosition;
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

    public void OnAttackStart()
    {
        Collider[] collidersHitted = Physics.OverlapSphere(StaffJoint.position, 0.5f, 1 << 6);
        foreach (var item in collidersHitted)
        {
            item.GetComponentInParent<Enemy>().AddDamage(40,this.gameObject,false);
        }
    }
    // public void ActivateWeapon()
    // {
    //     if (!isInteract && !isMovementPressed)
    //     {
    //         isInteract = true;
    //        // Context.IsSwitchPressed = false;
    //         sockets.Attach(prova, Handle_Mesh_Sockets.SocketId.Spine);
    //         //anim.SetBool(Context.EquipHash, true);
    //     }
    //     else if (isInteract && !isMovementPressed)
    //     {
    //         isInteract = false;
    //        // Context.IsSwitchPressed = false;
    //
    //         sockets.Attach(prova, Handle_Mesh_Sockets.SocketId.RightHand);
    //       // Context.Animator.SetBool(Context.EquipHash, false);
    //     }
    // }
    //public void ActivateWeapon()
    //{
    //    if (isInteract && !isWeaponAttached && !isMovementPressed)
    //    {
    //        isWeaponAttached = true;
    //        isInteract = false;
    //       // sockets.Attach(prova, Handle_Mesh_Sockets.SocketId.Spine);
    //       // anim.SetBool(equipHash, true);
    //    }
    //    else if (isInteract && isWeaponAttached && !isMovementPressed)
    //    {
    //        isWeaponAttached = false;
    //        isInteract = false;

    //        //prova.parent = null;
    //        //anim.SetBool(equipHash, false);
    //    }
    //}
    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}