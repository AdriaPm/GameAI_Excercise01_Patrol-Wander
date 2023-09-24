using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class POV_Camera : MonoBehaviour
{

    public float mouseSensitivity = 2.0f;  // Mouse sensitivity
    public float minVerticalAngleDOWN = -50.0f;  // Minimum vertical angle (looking down)
    public float maxVerticalAngleUP = 50.0f;   // Maximum vertical angle (looking up)
    public float minHorizontalAngleLEFT = -70.0f; // Minimum horizontal angle (looking left)
    public float maxHorizontalAngleRIGHT = 70.0f;  // Maximum horizontal angle (looking right)

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate horizontal and vertical rotations
        rotationY += mouseX * mouseSensitivity;
        rotationX -= mouseY * mouseSensitivity;

        // Clamp vertical rotation within angle limits
        rotationX = Mathf.Clamp(rotationX, minVerticalAngleDOWN, maxVerticalAngleUP);

        // Clamp horizontal rotation within angle limits
        rotationY = Mathf.Clamp(rotationY, minHorizontalAngleLEFT, maxHorizontalAngleRIGHT);

        // Apply rotations to the camera
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}
