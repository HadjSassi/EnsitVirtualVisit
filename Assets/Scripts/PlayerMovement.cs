using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    
    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
    private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    
    private CharacterController CaractherController;

    private Vector3 moveDirection;
    
    private Transform playerCamera;

    public float speed = 3f;

    private float gravity = 20f;

    public float jumpForce = 5f;

    private float verticalVelocity = 0;

    private bool jumpTrigger = false;
    
    private GameObject avatar;
    
    private float walkSpeed = 3f;
    
    private float runSpeed = 8f;
    
    private float turnSmoothVelocity;
    
    private const float TURN_SMOOTH_TIME = 0.05f;
    
    private bool isRunning;
    
    private Animator animator;

    private void Awake()
    {
        avatar = transform.GetChild(0).gameObject;
        CaractherController = GetComponent<CharacterController>();
        animator = avatar.GetComponent<Animator>();
        animator.applyRootMotion = false;
        animator.SetBool(IsGroundedHash, this.CaractherController.isGrounded);
        if (playerCamera == null)
        {
            playerCamera = transform.GetChild(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }
    
    //this code let me jump but it's not considering the camera orientation
    void MoveThePlayer()
    {
        // Get input directly from Unity's Input class
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Get the direction relative to the camera's forward direction
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0f;
        right.y = 0f;
        
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * verticalInput + right * horizontalInput;
        
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        float currentSpeed = moveDirection.magnitude ;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;
        ApplyGravity();
        CaractherController.Move(moveDirection);
        animator.SetFloat(MoveSpeedHash, currentSpeed);
        if (currentSpeed > 0)
        {
            RotateAvatarTowardsMoveDirection(moveDirection);
        }
    }
    
    

    
    void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        PlayerJumb();
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    private void PlayerJumb()
    {
        
        if (this.CaractherController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
            jumpTrigger = true;
            animator.SetTrigger(JumpHash);
        }

        if (jumpTrigger && this.CaractherController.isGrounded)
        {
            jumpTrigger = false;
            animator.SetBool(IsGroundedHash, this.CaractherController.isGrounded);
        }
    }
    
    private void RotateAvatarTowardsMoveDirection(Vector3 moveDirection)
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + transform.rotation.y;
        float angle = Mathf.SmoothDampAngle(avatar.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TURN_SMOOTH_TIME);
        avatar.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

}
