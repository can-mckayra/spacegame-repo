using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float accelerationForce = 200f;
    public float decelerationForce = 100f;
    public float maxForwardVelocity = 500f;
    public float maxBackwardVelocity = 250f;
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

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.maxAngularVelocity = 7f;
    }

    void Update()
    {
        HandleAcceleration();
        HandleRoll();
        HandleYawAndPitch();
        HandleElevation();

        //Debug.Log(rb.velocity);
        Debug.Log(rb.angularVelocity);
    }

    void HandleAcceleration()
    {
        float accelerationInput = Input.GetAxisRaw("Vertical");
        if (accelerationInput > 0)
        {
            rb.AddForce(accelerationForce * accelerationInput * transform.forward);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxForwardVelocity);
        }
        else if (accelerationInput < 0)
        {
            rb.AddForce(transform.forward * decelerationForce * accelerationInput); // Slower deceleration
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBackwardVelocity);
        }
    }

    void HandleRoll()
    {
        float rollInput = Input.GetAxisRaw("Horizontal");
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
        // Calculate cursor position relative to the center of the screen
        float mouseX = Input.mousePosition.x - Screen.width / 2f;
        float mouseY = Input.mousePosition.y - Screen.height / 2f;

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
        float elevationInput = Input.GetAxisRaw("Elevation");

        // Check if elevation input is active
        if (elevationInput != 0)
        {
            // Apply upward force when elevation key is pressed
            rb.AddForce(elevationForce * elevationInput * transform.up);
        }
        else
        {
            // Calculate the magnitude of the upward velocity
            float upwardVelocityMagnitude = Vector3.Dot(rb.velocity, transform.up);

            // Calculate the damping factor based on current upward velocity
            float dampingFactor = Mathf.Clamp01(upwardVelocityMagnitude / maxElevationVelocity);

            // Calculate damping force to gradually reduce upward velocity when elevation key is not pressed
            Vector3 dampingForce = dampingFactor * elevationDamping * -upwardVelocityMagnitude * transform.up;

            // Apply damping force
            rb.AddForce(dampingForce);
        }

        // Clamp the magnitude of the velocity to limit speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxElevationVelocity);
    }
}