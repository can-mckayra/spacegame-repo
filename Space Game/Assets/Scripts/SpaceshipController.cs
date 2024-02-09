using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float accelerationForce = 200f;
    public float decelerationForce = 100f;
    public float maxForwardSpeed = 500f;
    public float maxBackwardSpeed = 250f;
    public float rollSpeed = 250f;
    public float rollDamping = 10f;
    public float yawSpeed = 500f;
    public float pitchSpeed = 500f;
    public float maxRollTorque = 500f;
    public float maxYawTorque = 500f;
    public float maxPitchTorque = 500f;
    public float elevationForce = 100f;
    public float maxElevationSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleAcceleration();
        HandleRoll();
        HandleYawAndPitch();
        HandleElevation();

        Debug.Log(rb.angularVelocity);
    }

    void HandleAcceleration()
    {
        float accelerationInput = Input.GetAxis("Vertical");
        if (accelerationInput > 0)
        {
            rb.AddForce(transform.forward * accelerationForce * accelerationInput);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxForwardSpeed);
        }
        else if (accelerationInput < 0)
        {
            rb.AddForce(transform.forward * decelerationForce * accelerationInput); // Slower deceleration
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBackwardSpeed);
        }
    }

    void HandleRoll()
    {
        float rollInput = Input.GetAxis("Horizontal");
        float rollAmount = -rollInput * rollSpeed * Time.deltaTime;

        // Cap roll speed
        float clampedRollAmount = Mathf.Clamp(rollAmount, -maxRollTorque, maxRollTorque);

        rb.AddTorque(transform.forward * clampedRollAmount);

        // Apply damping to gradually stop rotation
        if (rollInput == 0 && rb.angularVelocity.magnitude > 0)
        {
            rb.AddTorque(-rb.angularVelocity * rollDamping);
        }
    }

    void HandleYawAndPitch()
    {
        // Calculate cursor position relative to the center of the screen
        float mouseX = Input.mousePosition.x - Screen.width / 2f;
        float mouseY = Input.mousePosition.y - Screen.height / 2f;

        // Calculate normalized values (-1 to 1) based on screen center
        float normalizedYaw = mouseX / (Screen.width / 2f);
        float normalizedPitch = mouseY / (Screen.height / 2f);

        // Calculate yaw and pitch amounts based on normalized values and speeds
        float yawAmount = normalizedYaw * yawSpeed * Time.deltaTime;
        float pitchAmount = -normalizedPitch * pitchSpeed * Time.deltaTime; // Inverted pitch

        // Cap yaw and pitch speed
        float clampedYawAmount = Mathf.Clamp(yawAmount, -maxYawTorque, maxYawTorque);
        float clampedPitchAmount = Mathf.Clamp(pitchAmount, -maxPitchTorque, maxPitchTorque);

        // Apply yaw and pitch torques to the rigidbody
        //rb.AddTorque(transform.up * clampedYawAmount);
        rb.AddTorque(transform.right * clampedPitchAmount); // Use right instead of -transform.right for inverted pitch
    }

    void HandleElevation()
    {
        float elevationInput = Input.GetAxis("Elevation");
        rb.AddForce(transform.up * elevationForce * elevationInput);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxElevationSpeed);
    }
}