using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public float baseSpeed = 10f;    // Normal speed
    public float boostMultiplier = 2f;  // How much faster the boost is
    public float slowMultiplier = 0.5f; // How much slower when holding Ctrl
    public float acceleration = 5f; // How quickly speed changes

    public float pitchSpeed = 50f;  // Tilt nose up/down
    public float yawSpeed = 50f;    // Turn left/right
    public float rollSpeed = 50f;   // Roll (tilt sideways)

    private float currentSpeed;
    private float verticalInput;
    private float horizontalInput;
    private float rollInput;

    void Start()
    {
        currentSpeed = baseSpeed; // Start at base speed
    }

    void Update()
    {
        // Get user input
        verticalInput = Input.GetAxis("Vertical");   // Up/Down for pitch
        horizontalInput = Input.GetAxis("Horizontal"); // Left/Right for yaw
        rollInput = 0; // Default to no roll

        // Check Q/E keys for rolling
        if (Input.GetKey(KeyCode.Q)) rollInput = 1;   // Roll left
        if (Input.GetKey(KeyCode.E)) rollInput = -1;  // Roll right

        // Handle Boost & Slow Down
        float targetSpeed = baseSpeed; // Default speed

        if (Input.GetKey(KeyCode.LeftShift)) targetSpeed *= boostMultiplier; // Boost
        if (Input.GetKey(KeyCode.LeftControl)) targetSpeed *= slowMultiplier; // Slow down

        // Smooth speed transition (gradual acceleration/deceleration)
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Move the plane forward at the current speed
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Pitch (tilt nose up/down)
        transform.Rotate(Vector3.right * pitchSpeed * verticalInput * Time.deltaTime);

        // Yaw (turn left/right)
        transform.Rotate(Vector3.up * yawSpeed * horizontalInput * Time.deltaTime);

        // Roll (tilt sideways)
        transform.Rotate(Vector3.forward * rollSpeed * rollInput * Time.deltaTime);
    }
}

