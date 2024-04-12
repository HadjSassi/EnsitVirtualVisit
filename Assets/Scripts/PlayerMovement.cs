using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");


    private CharacterController characterController;

    private Transform playerCamera;

    private GameObject avatar;

    private Animator animator;

    private Vector3 moveDirection;


    public float speed = 3f;
    private float gravity = 20f;
    public float jumpForce = 5f;
    private float verticalVelocity = 0;
    private bool jumpTrigger = false;
    private float turnSmoothVelocity;
    private const float TURN_SMOOTH_TIME = 0.05f;
    private float currentSpeed = 0f;

    private void Awake()
    {
        avatar = transform.GetChild(0).gameObject;
        characterController = GetComponent<CharacterController>();
        animator = avatar.GetComponent<Animator>();
        animator.applyRootMotion = false;
        animator.SetBool(IsGroundedHash, this.characterController.isGrounded);
        playerCamera = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (avatar != null && animator != null)
            MoveThePlayer();
        else
        {
            avatar = transform.GetChild(0).gameObject;
            animator = avatar.GetComponent<Animator>();
        }
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

        var velocity = characterController.velocity;
        float velocityFacing = Mathf.Abs(velocity.magnitude) > 1 ? 1 : 0;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = 5;
            currentSpeed = moveDirection.magnitude * 2 * velocityFacing;
            animator.SetFloat(MoveSpeedHash, currentSpeed, 0.20f, Time.deltaTime);
        }
        else
        {
            if (speed == 5)
            {
                speed = 3;
                currentSpeed = moveDirection.magnitude * velocityFacing;
                animator.SetFloat(MoveSpeedHash, currentSpeed, 0.20f, Time.deltaTime);
            }
            else
            {
                speed = 3;
                currentSpeed = moveDirection.magnitude * velocityFacing;
                animator.SetFloat(MoveSpeedHash, currentSpeed, 0.10f, Time.deltaTime);
            }
        }


        moveDirection = transform.TransformDirection(moveDirection);

        moveDirection *= speed * Time.deltaTime;
        ApplyGravity();
        characterController.Move(moveDirection);
        if (currentSpeed > 0)
        {
            RotateAvatarTowardsMoveDirection(moveDirection);
        }
    }


    void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        // PlayerJumb();
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    private void PlayerJumb()
    {
        if (this.characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
            jumpTrigger = true;
            animator.SetTrigger(JumpHash);
        }

        if (jumpTrigger && this.characterController.isGrounded)
        {
            jumpTrigger = false;
            animator.SetBool(IsGroundedHash, this.characterController.isGrounded);
        }
    }

    private void RotateAvatarTowardsMoveDirection(Vector3 moveDirection)
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + transform.rotation.y;
        float angle = Mathf.SmoothDampAngle(avatar.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
            TURN_SMOOTH_TIME);
        avatar.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}