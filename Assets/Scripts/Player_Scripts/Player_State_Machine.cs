using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_State_Machine : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;

    [SerializeField] private float gravityValue = -9.81f;

    //[SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float animSmoothTime = 0.1f;
    [SerializeField] private Transform weapon;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private bool groundedPlayer;
    private Vector3 playerVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction switchWeapon;

    private Animator anim;
    private Handle_Mesh_Sockets sockets;
    private int velocityHash_X;
    private int velocityHash_Z;
    private int VelocityHash_Y;
    private int equipHash;
    private int isWalkingHash;
    private int jumpHash;
    private float animPlayTransition = 0.15f;
    private Vector2 currentAnimationBlend;
    private Vector2 ciao;

    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private bool isMovementPressed;
    private float maxJumpHeight = 1.0f;
    private float maxJumpTime = 0.5f;
    private float initialJumpVelocity;

    public float animSmoothTimexx;
    private float groundDistance = 0.2f;
    private bool isGrounded;

    private Player_BaseState _currentState;
    private Player_StateFactory _states;

    public Player_BaseState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    public bool IsJumpPressed
    {
        get { return IsJumpPressed; }
    }
    void Awake()
    {
        _states = new Player_StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        sockets = GetComponent<Handle_Mesh_Sockets>();
        cameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();

        SetHash();
        isWeaponAttached = false;
    }

    void Update()
    {
        OnMovementInput();

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnMovementInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, input, ref animationVelocity, animSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);

        //CameraFollow
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);


        //BlendAnim
        anim.SetFloat(velocityHash_X, currentAnimationBlend.x);
        anim.SetFloat(velocityHash_Z, currentAnimationBlend.y);
        anim.SetFloat(VelocityHash_Y, playerVelocity.y);

        isMovementPressed = input.x != 0 || input.y != 0;
    }
    // Update is called once per frame

    void SetHash()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        switchWeapon = playerInput.actions["Switch"];

        velocityHash_X = Animator.StringToHash("VelocityX");
        velocityHash_Z = Animator.StringToHash("VelocityZ");
        VelocityHash_Y = Animator.StringToHash("VelocityY");
        equipHash = Animator.StringToHash("Equip");
        jumpHash = Animator.StringToHash("Jump");
        isWalkingHash = Animator.StringToHash("isWalking");
    }
}