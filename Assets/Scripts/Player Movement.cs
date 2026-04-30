using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpHeight = 2f;
    public float sprintSpeed = 10f;
    private float currentSpeed;

    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;

    public float normalFOV = 60f;
    public float zoomFOV = 30f;
    public float zoomSpeed = 5f;

    public Image crosshair;
    public Vector2 normalSize = new Vector2(60, 60);
    public Vector2 aimSize = new Vector2(38, 38);
    private Vector2 targetSize;

    public float shootExpandAmount = 15f;
    public float shrinkSpeed = 3f;
    private float currentSpread = 0f;


    private float forwardInputValue;
    private float StrafeInputValue;
    private bool jumpInput;

    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private float rotateCameraPitch;

    private Camera firstPersonCam;
    private CharacterController characterController;

    public Transform playerSpawnPoint;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        firstPersonCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;

    }


    void OnEnable()
    {
        characterController.enabled = false;
        transform.position = playerSpawnPoint.position;
        transform.rotation = playerSpawnPoint.rotation;

        rotateCameraPitch = 0f;
        characterController.enabled = true;
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        StrafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        Movement();
        JumpAndGravity();
        CameraMovement();
        
        currentSpread = Mathf.Lerp(currentSpread, 0f, Time.deltaTime * shrinkSpeed);
        if (Input.GetMouseButton(1))
        {
            firstPersonCam.fieldOfView = Mathf.Lerp(firstPersonCam.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
            targetSize = aimSize;
        }
        else
        {
            firstPersonCam.fieldOfView = Mathf.Lerp(firstPersonCam.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
            targetSize = normalSize;
        }

        
       Vector2 spreadOffset = new Vector2(currentSpread, currentSpread);
       crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, targetSize + spreadOffset, Time.deltaTime * zoomSpeed);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = movementSpeed;
        }
        if (Input.GetMouseButtonDown(0))
        {
            currentSpread += shootExpandAmount;
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
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }


            if (jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            }
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
