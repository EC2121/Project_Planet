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

    private Animator anim;
    private int velocityHash_X;
    private int velocityHash_Z;

    private Vector2 currentAnimationBlend;
    private Vector2 animationVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        anim = GetComponent<Animator>();
        velocityHash_X = Animator.StringToHash("VelocityX");
        velocityHash_Z = Animator.StringToHash("VelocityZ");
    }

    // Update is called once per frame
    void Update()
    {
        //GravityHandle
        groundedPlayer = controller.isGrounded;
        if (jumpAction.triggered && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

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

        //JumpDistance
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //RotateToCamera
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}