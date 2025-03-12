using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float turnSpeed = 50.0f;
    public float forwardInput;
    public float horizontalInput;

    // Drift Variables
    public float driftFactor = 0.95f; // Controls how much the drift slows movement
    public float driftTurnMultiplier = 1.5f; // Increases turn speed when drifting
    private bool isDrifting;

    // NOS (Boost) Variables
    public float nitroBoostMultiplier = 2f; // How much faster NOS makes the car
    public float nitroDuration = 2f; // How long NOS lasts
    private bool isUsingNitro;
    private float normalSpeed;

    // Suspension (Bouncing effect)
    public float suspensionStrength = 5f;
    public float suspensionDamping = 2f;
    private Rigidbody rb;

    // Brakes
    public float brakeForce = 10f;
    public float brakeDeceleration = 0.5f; // Lower value for slower braking
    private bool isBraking;

    private float currentBrakeForce;
    private float originalDrag;

    // Camera Shake
    public float cameraShakeIntensity = 0.1f; // Intensity of the shake

    // Weight transfer variables
    public float weightTransferSpeed = 0.5f; // How fast the weight transfers when turning

    void Start()
    {
        // Make sure the Rigidbody component is attached
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found. Please attach a Rigidbody component to the car.");
            return;
        }

        normalSpeed = speed; // Store default speed
        currentBrakeForce = brakeDeceleration; // Start with normal deceleration

        // Store the original drag value of the Rigidbody
        originalDrag = rb.drag;
    }

    void Update()
    {
        // Get player input for movement
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // **DRIFT MECHANIC**
        if (Input.GetKey(KeyCode.Space)) // Hold space to drift
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        // **NOS (BOOST) MECHANIC**
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isUsingNitro) // Press Shift to boost
        {
            StartCoroutine(UseNitro());
        }

        // **BRAKING MECHANIC**
        if (Input.GetKey(KeyCode.LeftControl)) // Hold LeftControl to brake
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }

        // **MOVEMENT & ROTATION**
        if (isBraking)
        {
            ApplyBrakes();
        }
        else
        {
            ApplyThrottle();
        }
    }

    void FixedUpdate()
    {
        // **SUSPENSION SYSTEM**
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            Vector3 suspensionForce = Vector3.up * suspensionStrength * (1.5f - hit.distance);
            rb.AddForce(suspensionForce - rb.velocity * suspensionDamping, ForceMode.Acceleration);
        }

        // **WEIGHT TRANSFER SYSTEM**
        Vector3 weightTransfer = Vector3.left * horizontalInput * weightTransferSpeed; // Shift weight based on turning input
        rb.AddForce(weightTransfer, ForceMode.Acceleration);
    }

    void ApplyThrottle()
    {
        // Accelerate the vehicle
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);

        // If drifting, apply a drift multiplier to turning
        if (isDrifting)
        {
            // Apply normal turning with drift multiplier
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime * driftTurnMultiplier);
        }
        else
        {
            // Apply normal turning speed without drift
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
        }

        // **Camera Shake** based on speed
        if (rb.velocity.magnitude > 20f)
        {
            Camera.main.transform.position += Random.insideUnitSphere * cameraShakeIntensity;
        }
    }

    void ApplyBrakes()
    {
        // Gradual deceleration for more realistic braking

        // Increase drag when braking for gradual slowdown
        rb.drag = Mathf.Lerp(rb.drag, 5f, Time.deltaTime * brakeDeceleration); // Increase drag slowly to decelerate

        // Gradually slow down the car's velocity
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * brakeDeceleration);
        }
    }

    IEnumerator UseNitro()
    {
        isUsingNitro = true;
        speed *= nitroBoostMultiplier; // Increase speed

        yield return new WaitForSeconds(nitroDuration); // Wait for duration

        speed = normalSpeed; // Reset speed
        isUsingNitro = false;
    }

    // Reset the drag back to the original value after braking is done
    void OnDisable()
    {
        rb.drag = originalDrag;
    }
}


