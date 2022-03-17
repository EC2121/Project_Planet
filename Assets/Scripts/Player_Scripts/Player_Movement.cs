using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float animSmoothTime = 0.1f;

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
    private int equipHash;
    private int isWalkingHash;
    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;
    private bool isWeaponAttached;
    private bool isMovementPressed;
    public Transform weapon;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        sockets = GetComponent<Handle_Mesh_Sockets>();
        cameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
        
        SetHash();
        isWeaponAttached = false;

    }

    void SetHash()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        switchWeapon = playerInput.actions["Switch"];

        velocityHash_X = Animator.StringToHash("VelocityX");
        velocityHash_Z = Animator.StringToHash("VelocityZ");
        equipHash = Animator.StringToHash("Equip");
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    void HandleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        if (isMovementPressed && !isWalking)
        {
            anim.SetBool(isWalkingHash, true);

        }
        else if (!isMovementPressed && isWalking)
        {
            anim.SetBool(isWalkingHash, false);
        }

    }
    public void ActivateWeapon()
    {
        if (switchWeapon.triggered && !isWeaponAttached)
        {
            isWeaponAttached = true;

            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.Spine);
            anim.SetBool(equipHash,true);


        }
        else if (switchWeapon.triggered && isWeaponAttached)
        {
            isWeaponAttached = false;
            
            sockets.Attach(weapon.transform, Handle_Mesh_Sockets.SocketId.RightHand);
            anim.SetBool(equipHash,false);
        
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
            sockets.Attach(weapon.transform,Handle_Mesh_Sockets.SocketId.Spine);
        }
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

        isMovementPressed = move.x != 0 || move.y != 0;

    }
    // Update is called once per frame
    void Update()
    {
        ActivateWeapon();
        OnMovementInput();
        HandleAnimation();
        //GravityHandle
        groundedPlayer = controller.isGrounded;
        if (jumpAction.triggered && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

      
        //JumpDistance
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // //RotatePlayerToCamera
        // Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}