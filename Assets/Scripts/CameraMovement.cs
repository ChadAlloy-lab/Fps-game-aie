using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;

    private float rotateCameraPitch;

    public Transform playerBody;

    

    private CameraMovement firstPersonCam;
    internal static object current;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    // Update is called once per frame
    void Update()
    {
    }
}
