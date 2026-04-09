using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpHeight = 2f;
    public float sprintSpeed = 10f;
    private float currentSpeed;

    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;



    private float forwardInputValue;
    private float StrafeInputValue;
    private bool jumpInput;

    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private float rotateCameraPitch;

    private Camera firstPersonCam;
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        firstPersonCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        StrafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        Movement();
        JumpAndGravity();
        CameraMovement();


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = movementSpeed;
        }

    }

    void Movement()
    {
        Vector3 direction = ((transform.forward * forwardInputValue
                            + transform.right * StrafeInputValue).normalized * currentSpeed * Time.deltaTime);

        direction += Vector3.up * verticalVelocity * Time.deltaTime;

        characterController.Move(direction);
                   
    }

    void JumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;
        }
        if (jumpInput)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
        else
        {

            if (verticalVelocity < terminalVelocity)
            {
                float gravityMultipler = 1;
                if (characterController.velocity.y < -1)
                {
                    gravityMultipler = fallGravityMultiplier;
                }
                verticalVelocity += Physics.gravity.y * gravityMultipler * Time.deltaTime;
            }
        }
    }

    void CameraMovement()
    {
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        rotateCameraPitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotateCameraPitch = Mathf.Clamp(rotateCameraPitch, -pitchRange, pitchRange);
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotateCameraPitch,0,0);


    }
        

}
