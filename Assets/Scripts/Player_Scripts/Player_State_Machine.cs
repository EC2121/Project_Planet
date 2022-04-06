using Cinemachine.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class Player_State_Machine : MonoBehaviour
{
    public static UnityEvent takeTheBox = new UnityEvent();
    public static UnityEvent<GameObject> OnHologramDisable = new UnityEvent<GameObject>();
    public static UnityEvent<bool> hit = new UnityEvent<bool>();

    [SerializeField] private Transform weapon;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runSpeed = 2.65f;
    [SerializeField] private float rotationFactor = 0.5f;

    private bool mai_BoxIsTakable;
    private Animator anim;
    private int isWalkingHash;
    private int isRunningHash;
    private int equipHash;
    private int isJumpingHash;
    private int isLandingHash;
    private int attackIndexHash;
    private int fallingSpeedHash;
    private int hasBoxHash;
    private int jumpCountHash;
    private int isAttacking;
    private string unEquipHash;
    private Player_Controller input;
    private CharacterController characterController;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed = false;
    private bool isRunPressed = false;
    private bool isMousePressed = false;
    private bool isJumpPressed = false;
    private bool switchWeapon = false;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;
    private Vector3 playerPos;
    private Player_BaseState _currentState;
    private Player_StateFactory _states;
    private bool requireNewWeaponSwitch = false;
    private bool requireNewAttack = false;
    private readonly float groundGravity = -1.10f;
    private readonly float fallingSpeed;
    private AnimatorStateInfo stateInfo;
    private int attackId = 0;
    private bool isInteract = false;
    private bool hasBox = false;
    private bool requireNewJump = false;
    private float initialJumpVelocity;
    private bool isJumping = false;
    private bool isAttack = false;
    private readonly float maxJumpHeight = 2f;
    private readonly float maxJumpTIme = 0.75f;
    private float gravity = -9.81f;
    private int jumpCount = 0;
    private Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    private Coroutine currentJumpResetRoutine = null;
    private Coroutine currentAttackResetRoutine = null;
    private Transform cameraMainTransform;
    private bool requireNewInteraction = false;
    private float maxHp = 1000f;
    private float hp;
    private bool isHitted = false;
    private bool requireNewHit = false;
    //getters and setters
    public Player_BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return characterController; } set { characterController = value; } }
    public UnityEvent TakeTheBox { get { return takeTheBox; } }
    public UnityEvent<bool> Hit { get { return hit; } }
    public Coroutine CurrentJumpResetRoutine { get { return currentJumpResetRoutine; } set { currentJumpResetRoutine = value; } }
    public Coroutine CurrentAttackResetRoutine { get { return currentAttackResetRoutine; } set { currentAttackResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return initialJumpVelocities; } set { initialJumpVelocities = value; } }
    public Dictionary<int, float> JumpGravities { get { return jumpGravities; } set { jumpGravities = value; } }
    public Animator Animator { get { return anim; } }
    public int IsJumpingHash { get { return isJumpingHash; } }
    public float JumpSpeed { get { return jumpSpeed; } }
    public bool IsJumping { set { isJumping = value; } }
    public bool IsAttack { set { isAttack = value; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public bool RequireNewWeaponSwitch { get { return requireNewWeaponSwitch; } set { requireNewWeaponSwitch = value; } }
    public bool RequireNewAttack { get { return requireNewAttack; } set { requireNewAttack = value; } }
    public bool RequireNewHit { get { return requireNewHit; } set { requireNewHit = value; } }

    public bool RequireNewInteraction { get { return requireNewInteraction; } set { requireNewInteraction = value; } }
    public int AttackIndexHash { get { return attackIndexHash; } set { attackIndexHash = value; } }
    public Vector3 PlayerPos { get { return transform.position; } }
    public int IsLandingHash { get { return isLandingHash; } }
    public int HasBoxHash { get { return hasBoxHash; } }
    public int EquipHash { get { return equipHash; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsInteract { get { return isInteract; } }
    public bool IsMousePressed { get { return isMousePressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public bool RequireNewJump { get { return requireNewJump; } set { requireNewJump = value; } }
    public int IsWalkingHash { get { return isWalkingHash; } }
    public string UnEquipHash { get { return unEquipHash; } }
    public int IsAttacking { get { return isAttacking; } }
    public int IsRunningHash { get { return isRunningHash; } }
    public int JumpCountHash { get { return jumpCountHash; } }
    public float GroundGravity { get { return groundGravity; } }
    public float RunMultiplier { get { return runSpeed; } }
    public bool HasBox { get { return hasBox; } set { hasBox = value; } }
    public bool IsSwitchPressed { get { return switchWeapon; } }
    public bool IsIsHitted { get { return isHitted; } set{isHitted = value;} }
    public bool Mai_BoxIsTakable { get { return mai_BoxIsTakable; } }
    public bool IsWeaponAttached { get { return isWeaponAttached; } set { isWeaponAttached = value; } }
    public Handle_Mesh_Sockets Sockets { get { return sockets; } }
    public Transform Weapon { get { return weapon; } }
    public AnimatorStateInfo AnimStateInfo { get { return stateInfo; } }
    public int AttackCount { get { return attackId; } set { attackId = value; } }
    public int JumpCount { get { return jumpCount; } set { jumpCount = value; } }
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }
    public float CurrentMovementX { get { return currentMovement.x; } set { currentMovement.x = value; } }
    public float CurrentMovementZ { get { return currentMovement.z; } set { currentMovement.z = value; } }
    public float Hp { get { return hp; } set { hp = value; } }
    public Vector2 CurrentMovementInput { get { return currentMovementInput; } set { currentMovementInput = value; } }

    private Vector3 positionToLookAt = Vector3.zero;

    private GameObject hologram;
    private void Awake()
    {

        hologram = GameObject.FindGameObjectWithTag("Hologram");
        hologram.SetActive(false);
        input = new Player_Controller();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        equipHash = Animator.StringToHash("Equip");
        isJumpingHash = Animator.StringToHash("isJumping");
        isLandingHash = Animator.StringToHash("Land");
        fallingSpeedHash = Animator.StringToHash("FallingSpeed");
        attackIndexHash = Animator.StringToHash("AttackIndex");
        hasBoxHash = Animator.StringToHash("HasBox");
        jumpCountHash = Animator.StringToHash("JumpCount");
        isAttacking = Animator.StringToHash("IsAttacking");
        unEquipHash = "Un_Equip";
        
        sockets = GetComponent<Handle_Mesh_Sockets>();
        cameraMainTransform = Camera.main.transform;
        
        _states = new Player_StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        input.Player.Move.started += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;

        input.Player.Run.started += OnRun;
        input.Player.Run.canceled += OnRun;

        input.Player.Switch.started += OnSwitchWeapon;
        input.Player.Switch.canceled += OnSwitchWeapon;

        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;

        input.Player.L_MouseClick.started += OnMousePressed;
        input.Player.L_MouseClick.performed += OnMousePressed;
        input.Player.L_MouseClick.canceled += OnMousePressed;

        input.Player.F_Interact.started += OnInteract;
        input.Player.F_Interact.canceled += OnInteract;

        isWeaponAttached = false;
        hp = maxHp;
        SetUpJumpVariables();
    }
    
    private void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    private void OnMousePressed(InputAction.CallbackContext context)
    {
        isMousePressed = context.ReadValueAsButton();
        requireNewAttack = false;
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        switchWeapon = context.ReadValueAsButton();
        requireNewWeaponSwitch = false;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        isInteract = context.ReadValueAsButton();
        requireNewInteraction = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJump = false;

    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    private void CreateHologram()
    {
        hologram.SetActive(true);
        hologram.transform.rotation = this.transform.rotation;
        hologram.transform.position = this.transform.position + this.transform.forward * 2;
        Animator animator = hologram.GetComponent<Animator>();
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
    }
    private void DestroyHologram()
    {
        OnHologramDisable.Invoke(this.gameObject);
        hologram.SetActive(false);
    }

    private void Update()
    {

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            CreateHologram();
            Invoke("DestroyHologram", 5);
        }

        _currentState.UpdateStates();

        Vector3 forwardCam = cameraMainTransform.forward;
        forwardCam.y = 0;
        forwardCam = forwardCam.normalized;
        if (forwardCam.sqrMagnitude < 0.01f)
            return;
        Quaternion inputFrame = Quaternion.LookRotation(forwardCam, Vector3.up);
        appliedMovement = inputFrame * currentMovement;
        characterController.Move(appliedMovement * Time.deltaTime);
        HandleRotation();
       // Debug.Log(isHitted);
        Debug.Log(hp + " HPPPPPPPPPPP");

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

    private void SetUpJumpVariables()
    {
        float timeToApex = maxJumpTIme / 2f;
        gravity = ( -2 * maxJumpHeight ) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = ( 2 * maxJumpHeight ) / timeToApex;
        float secondJumpGravity = ( -2 * ( maxJumpHeight + 2 ) ) / Mathf.Pow(( timeToApex * 1.25f ), 2);
        float secondJumpInitialVelocity = ( 2 * ( maxJumpHeight + 2 ) ) / ( timeToApex * 1.25f );
        float thirdJumpGravity = ( -2 * ( maxJumpHeight + 4 ) ) / Mathf.Pow(( timeToApex * 1.5f ), 2);
        float thirdJumpInitialVelocity = ( 2 * ( maxJumpHeight + 4 ) ) / ( timeToApex * 1.5f );

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = appliedMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = appliedMovement.z;
        if (appliedMovement.sqrMagnitude > 0.01f && isMovementPressed)
        {
            Quaternion currRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt, Vector3.up);
            transform.rotation = Quaternion.Slerp(currRotation, targetRotation, Damper.Damp(1, rotationFactor, Time.deltaTime));
        }
    }

    public void OnAttackStart()
    {
        Collider[] collidersHitted = Physics.OverlapSphere(weapon.position, 0.5f, 1 << 6);
        foreach (var item in collidersHitted)
        {
            item.GetComponentInParent<Enemy>().AddDamage(40, gameObject, false);
        }
    }

    private void OnEnable()
    {
        input.Player.Enable();
        hit.AddListener(arg0 => isHitted = true);
    }

    private void OnDisable()
    {
        input.Player.Disable();
        hit.RemoveListener(arg0 => isHitted = false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box")) mai_BoxIsTakable = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box")) mai_BoxIsTakable = false;
    }
}