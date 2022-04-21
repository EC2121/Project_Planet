using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_State_Machine : MonoBehaviour
{
    public static UnityEvent reviveRoby = new UnityEvent();
    public static UnityEvent takeTheBox = new UnityEvent();
    public static UnityEvent canCrystal = new UnityEvent();
    public static UnityEvent<GameObject> onBreakableWallFound = new UnityEvent<GameObject>();
    public static UnityEvent<GameObject> OnHologramDisable = new UnityEvent<GameObject>();
    public static UnityEvent OnHologramEnable = new UnityEvent();
    public static UnityEvent hit = new UnityEvent();
    public static UnityEvent gamePlayerFinalePhase = new UnityEvent();

    [SerializeField] private Image PingImage;
    [SerializeField] private LayerMask BreakableRayCastMask;
    [SerializeField] private Transform weapon;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runSpeed = 2.65f;
    [SerializeField] private float rotationFactor = 0.5f;
    [SerializeField] private float maxHp;
    [SerializeField] private Slider mayHpSlider;

    public static bool hasBox;
    public Slider reviveSlider;

    private bool isCrystalActivable;
    private bool canReviveRoby;
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
    private int isRunAttackingHash;
    private int isJumpHittedHash;
    private int isHittedHash;
    private int isJumpAttackHash;
    private string unEquipString;
    private string attachWeaponString;
    private string detachWeaponString;
    private string boxString;
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
    private bool onHologram = false;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private Handle_Mesh_Sockets sockets;
    private Vector3 playerPos;
    private Player_BaseState _currentState;
    private Player_StateFactory _states;
    private bool requireNewWeaponSwitch = false;
    private bool requireNewAttack = false;
    private float groundGravity = -0.05f;
    private float fallingSpeed;
    private AnimatorStateInfo stateInfo;
    private int attackId = 0;
    private bool isInteract = false;
    private bool requireNewJump = false;
    private float initialJumpVelocity;
    private bool isJumping = false;
    private bool isAttack = false;
    private float maxJumpHeight = 2f;
    private float maxJumpTIme = 0.75f;
    private float gravity = -9.81f;
    private float timeToApex;
    private int jumpCount = 0;
    private Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    private Coroutine currentJumpResetRoutine = null;
    private Coroutine currentAttackResetRoutine = null;
    private Transform cameraMainTransform;
    private bool requireNewInteraction = false;
    private float hp;
    private bool isHitted = false;
    private bool requireNewHit = false;
    public GameObject hologram;

    public Player_BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return characterController; } set { characterController = value; } }
    public UnityEvent TakeTheBox { get { return takeTheBox; } }
    public UnityEvent CanCrystal { get { return canCrystal; } }
    public UnityEvent ReviveRoby { get { return reviveRoby; } }
    public UnityEvent GamePlayerFinalePhase { get { return gamePlayerFinalePhase; } }
    public UnityEvent Hit { get { return hit; } }
    public Coroutine CurrentJumpResetRoutine { get { return currentJumpResetRoutine; } set { currentJumpResetRoutine = value; } }
    public Coroutine CurrentAttackResetRoutine { get { return currentAttackResetRoutine; } set { currentAttackResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return initialJumpVelocities; } set { initialJumpVelocities = value; } }
    public Dictionary<int, float> JumpGravities { get { return jumpGravities; } set { jumpGravities = value; } }
    public Animator Animator { get { return anim; } }
    public int IsJumpingHash { get { return isJumpingHash; } }
    public float JumpSpeed { get { return jumpSpeed; } }
    public float TimeToApex { get { return timeToApex; } }
    public bool IsJumping { set { isJumping = value; } }
    public float MaxJumpHeight { get { return maxJumpHeight; } set { maxJumpHeight = value; } }
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
    public int IsHittedHash { get { return isHittedHash; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsInteract { get { return isInteract; } }
    public bool IsMousePressed { get { return isMousePressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public bool RequireNewJump { get { return requireNewJump; } set { requireNewJump = value; } }
    public int IsWalkingHash { get { return isWalkingHash; } }
    public int IsJumpAttackHash { get { return isJumpAttackHash; } }
    public int IsJumpHittedHash { get { return isJumpHittedHash; } }
    public string UnEquipString { get { return unEquipString; } }
    public int IsAttacking { get { return isAttacking; } }
    public int IsRunningHash { get { return isRunningHash; } }
    public int IsRunAttackingHash { get { return isRunAttackingHash; } }
    public int JumpCountHash { get { return jumpCountHash; } }
    public float GroundGravity { get { return groundGravity; } }
    public float Gravity { get { return gravity; } set { gravity = value; } }
    public float RunMultiplier { get { return runSpeed; } }
    public bool HasBox { get { return hasBox; } set { hasBox = value; } }
    public bool IsSwitchPressed { get { return switchWeapon; } }
    public bool IsIsHitted { get { return isHitted; } set { isHitted = value; } }
    public bool Mai_BoxIsTakable { get { return mai_BoxIsTakable; } }
    public bool IsCrystalActivable { get { return isCrystalActivable; } }
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
    public float MaySliderValue { get { return mayHpSlider.value; } set { mayHpSlider.value = value; } }


    [SerializeField] private bool hasKey;
    private GameObject keyReference;

    public GameObject Inventory;
    public bool HasKey
    {
        get => hasKey;
        set
        {
            hasKey = value;
            keyReference.SetActive(!hasKey);
            Inventory.transform.GetChild(0).gameObject.SetActive(hasKey);
            //Inventory.GetComponent<Image>().enabled = hasKey;
        }
    }

    
    private float hologramTimer;
    private bool startHologramTimer;
    private RaycastHit Hitinfo;
    private bool canPing = true;
    private float robyslidervalue = 0;
    public float recovery = 0;
    private void Awake()
    {
        Inventory = GameObject.Find("Inventory");
        keyReference = GameObject.FindGameObjectWithTag("Key");
        Interactable.OnKeyTakenDel += () => HasKey = true;
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
        isRunAttackingHash = Animator.StringToHash("isRunAttacking");
        isJumpHittedHash = Animator.StringToHash("isJumpHitted");
        isHittedHash = Animator.StringToHash("isHitted");
        isJumpAttackHash = Animator.StringToHash("isJumpAttack");
        unEquipString = "Un_Equip";
        attachWeaponString = "EquipWeapon";
        detachWeaponString = "Detach";
        boxString = "Box";

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

        input.Player.Q.started += Hologram;
        input.Player.Q.canceled += Hologram;

        isWeaponAttached = false;
        hp = maxHp;
        mayHpSlider.maxValue = maxHp;
        mayHpSlider.value = hp;
        SetUpJumpVariables();
    }

    private void Start()
    {
        hologram.SetActive(false);
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    private void Hologram(InputAction.CallbackContext context)
    {
        onHologram = context.ReadValueAsButton();
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
        hologram.transform.rotation = transform.rotation;
        hologram.transform.position = transform.position + transform.forward * 2;
        Animator animator = hologram.GetComponent<Animator>();
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, true);
        OnHologramEnable.Invoke();
    }
    private void DestroyHologram()
    {
        OnHologramDisable.Invoke(gameObject);
        hologram.SetActive(false);
    }

    private IEnumerator PingCoroutine()
    {
        yield return new WaitForSeconds(3f);
        PingImage.gameObject.SetActive(false);
        canPing = true;
    }

    private void Update()
    {
        _currentState.UpdateStates();

        if (canPing && isInteract)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out Hitinfo, 20, BreakableRayCastMask))
            {
                canPing = false;
                PingImage.gameObject.SetActive(true);
                PingImage.transform.position = Hitinfo.point + ( Hitinfo.normal * 0.1f );
                PingImage.transform.forward = Hitinfo.normal;
                StartCoroutine(PingCoroutine());
                onBreakableWallFound?.Invoke(Hitinfo.transform.gameObject);
            }
        }
        if (recovery <= 0)
        {
            hp = Mathf.Clamp(hp + ( Time.deltaTime * 3 ), 0, maxHp);
            mayHpSlider.value = hp;
        }
        else
        {
            recovery -= Time.deltaTime;
        }

        if (onHologram)
        {
            if (hologramTimer <= 0)
            {
                startHologramTimer = true;
                CreateHologram();
                hologramTimer = 10;
                Invoke(nameof(DestroyHologram), 5);
            }
        }

        if (startHologramTimer)
        {
            hologramTimer -= Time.deltaTime;
            if (hologramTimer <= 0) startHologramTimer = false;
        }
        HandleCameraRotation();
        characterController.Move(appliedMovement * Time.deltaTime);
        HandleRotation();
        HandleGravity();

        if (canReviveRoby && isInteract)
        {
            reviveSlider.gameObject.SetActive(true);
            robyslidervalue += Time.deltaTime;
            reviveSlider.value = robyslidervalue;
            if (reviveSlider.value == reviveSlider.maxValue)
            {
                reviveRoby.Invoke();
                canReviveRoby = false;
            }
        }
        else
        {
            reviveSlider.gameObject.SetActive(false);
            reviveSlider.value = 0;
            robyslidervalue = 0;
        }

        if (characterController.isGrounded)
        {
            characterController.stepOffset = 0.7f;
            characterController.slopeLimit = 45;
        }
    }

    private void HandleCameraRotation()
    {
        Vector3 forwardCam = cameraMainTransform.forward;
        forwardCam.y = 0;
        forwardCam = forwardCam.normalized;
        if (forwardCam.sqrMagnitude < 0.01f)
            return;
        Quaternion inputFrame = Quaternion.LookRotation(forwardCam, Vector3.up);
        appliedMovement = inputFrame * currentMovement;
    }

    private void HandleGravity()
    {
        if (!characterController.isGrounded && !isJumpPressed)
        {
            currentMovement.y += gravity * Time.deltaTime;
            currentRunMovement.y += gravity * Time.deltaTime;
        }
    }
    public void OnAnimationEvent(string eventName)
    {

        if (eventName == attachWeaponString )
        {
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.RightHand);
        }

        if (eventName == detachWeaponString)
        {
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.Spine);
        }
    }
    private void SetUpJumpVariables()
    {
        timeToApex = maxJumpTIme / 2f;
        gravity = ( -2 * maxJumpHeight ) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = ( 2 * maxJumpHeight ) / timeToApex;
        float secondJumpGravity = ( -2 * ( maxJumpHeight + 2 ) ) / Mathf.Pow(( timeToApex * 1.25f ), 2);
        float secondJumpInitialVelocity = ( 2 * ( maxJumpHeight + 1 ) ) / ( timeToApex * 1.25f );
        float thirdJumpGravity = ( -2 * ( maxJumpHeight + 4 ) ) / Mathf.Pow(( timeToApex * 1.5f ), 2);
        float thirdJumpInitialVelocity = ( 2 * ( maxJumpHeight + 2 ) ) / ( timeToApex * 1.5f );

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    public void ForceIdle()
    {
        _currentState = _states.Grounded();
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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isHitted = true;
            characterController.stepOffset = 0f;
            characterController.slopeLimit = 0f;
        }
    }
    private void OnCollisionStay(Collision collision)
    {

    }
    private void OnCollisionExit(Collision other)
    {

    }

    public void OnAttackStart()
    {
        Collider[] collidersHitted = Physics.OverlapSphere(weapon.position, 1f, 1 << 6);
        foreach (var item in collidersHitted)
        {
            item.GetComponentInParent<Enemy>().AddDamage(40, gameObject, false);
        }
    }

    private void OnEnable()
    {
        input.Player.Enable();
        hit.AddListener(() => isHitted = true);
    }

    private void OnDisable()
    {
        input.Player.Disable();
        hit.RemoveListener(() => isHitted = true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boxString))
        {
            mai_BoxIsTakable = true;
        }
        if (other.CompareTag("Finish"))
        {
            canReviveRoby = true;
        }
        if (other.CompareTag("Crystal"))
        {
            isCrystalActivable = true;
        }
        if (other.gameObject.layer == 11)
        {
            _currentState = _states.Dead();
            _currentState.EnterState();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            characterController.stepOffset = 0.7f;
            characterController.slopeLimit = 45;
        }
        if (other.CompareTag("Crystal")) isCrystalActivable = false;

        if (other.CompareTag("Finish"))
            canReviveRoby = false;

        if (other.CompareTag(boxString)) mai_BoxIsTakable = false;
    }

    public void OnJumpAttack()
    {
        Collider[] collidersHitted = Physics.OverlapSphere(transform.position + transform.forward * 0.1f + transform.up * 0.1f, 2, 1 << 6);
        foreach (var item in collidersHitted)
        {
            item.GetComponentInParent<Enemy>().AddDamage(0, gameObject, true);
        }
    }
}