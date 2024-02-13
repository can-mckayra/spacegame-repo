using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float accelerationForce = 200f;
    public float decelerationForce = 100f;
    public float maxForwardVelocity = 100f;
    public float maxBackwardVelocity = 50f;
    public float rollTorque = 250f;
    public float maxRollVelocity = 500f;
    public float rollDamping = 10f;
    public float yawTorque = 500f;
    public float pitchTorque = 500f;
    public float maxYawVelocity = 500f;
    public float maxPitchVelocity = 500f;
    public float elevationForce = 100f;
    public float maxElevationVelocity = 100f;
    public float elevationDamping = 0.1f;

    public float accelerationInput;
    public float rollInput;
    public float mouseX;
    public float mouseY;
    public float elevationInput;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.maxAngularVelocity = 7f;
    }

    void Update()
    {
        accelerationInput = Input.GetAxisRaw("Vertical");
        rollInput = Input.GetAxisRaw("Horizontal");
        mouseX = Input.mousePosition.x - Screen.width / 2f;
        mouseY = Input.mousePosition.y - Screen.height / 2f;
        elevationInput = Input.GetAxisRaw("Elevation");
    }

    void FixedUpdate()
    {
        HandleAcceleration();
        HandleRoll();
        //HandleYawAndPitch();
        HandleElevation();

        //Debug.Log(currentForwardSpeed);
        //Debug.Log(rb.angularVelocity);
        Debug.Log(rb.velocity);
    }

    void HandleAcceleration()
    {
        if (accelerationInput > 0)
        {
            rb.AddForce(accelerationForce * accelerationInput * transform.forward);
        }
        else if (accelerationInput < 0)
        {
            rb.AddForce(accelerationInput * decelerationForce * transform.forward); // Slower deceleration
        }

        // Clamp velocity after applying forces
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, accelerationInput >= 0 ? maxForwardVelocity : maxBackwardVelocity);
    }

    void HandleRoll()
    {
        float rollAmount = -rollInput * rollTorque * Time.deltaTime;

        // Cap roll speed
        float clampedRollAmount = Mathf.Clamp(rollAmount, -maxRollVelocity, maxRollVelocity);

        rb.AddTorque(transform.forward * clampedRollAmount);

        // Apply damping to gradually stop rotation
        if (rollInput == 0 && rb.angularVelocity.magnitude > 0)
        {
            rb.AddTorque(-rb.angularVelocity * rollDamping);
        }
    }

    void HandleYawAndPitch()
    {
        // Calculate normalized values (-1 to 1) based on screen center
        float normalizedYaw = mouseX / (Screen.width / 2f);
        float normalizedPitch = mouseY / (Screen.height / 2f);

        // Calculate yaw and pitch amounts based on normalized values and speeds
        float yawAmount = normalizedYaw * yawTorque * Time.deltaTime;
        float pitchAmount = -normalizedPitch * pitchTorque * Time.deltaTime; // Inverted pitch

        // Cap yaw and pitch speed
        float clampedYawAmount = Mathf.Clamp(yawAmount, -maxYawVelocity, maxYawVelocity);
        float clampedPitchAmount = Mathf.Clamp(pitchAmount, -maxPitchVelocity, maxPitchVelocity);
        
        // Apply yaw and pitch torques to the rigidbody
        rb.AddTorque(transform.up * clampedYawAmount);
        rb.AddTorque(transform.right * clampedPitchAmount); // Use right instead of -transform.right for inverted pitch
    }

    void HandleElevation()
    {
        rb.AddForce(elevationForce * elevationInput * transform.up);
    }
}