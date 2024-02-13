using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Force and Velocity Controls")]
    public float accelerationForce = 200f;
    public float decelerationForce = 100f;
    public float maxForwardVelocity = 100f;
    public float maxBackwardVelocity = 50f;
    public float rollTorque = 50f;
    //public float maxRollVelocity = 500f; reminder to implement angular velocity clamp.
    public float rollDamping = 10f;
    public float yawTorque = 100f;
    public float pitchTorque = 100f;
    public float maxYawVelocity = 100f;
    public float maxPitchVelocity = 100f;
    public float elevationForce = 100f;
    public float maxElevationVelocity = 100f;
    public float elevationDamping = 0.1f;

    // Inputs
    private float accelerationInput;
    private float rollInput;
    private float elevationInput;
    private float mouseX;
    private float mouseY;

    [Header("Crosshair Handling")]
    public RectTransform crosshair;
    public float crosshairMultiplier = 10.0f;
    public float normalizeMagnitude = 100.0f;
    public float radius = 10000.0f;
    public float speed = 50.0f;

    public Vector3 screenCenter;
    public Vector3 crosshairOrigin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.maxAngularVelocity = 7f;

        Cursor.visible = false;
        screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0.0f);
        crosshairOrigin = screenCenter;
    }

    void Update()
    {
        accelerationInput = Input.GetAxisRaw("Vertical");
        rollInput = Input.GetAxisRaw("Horizontal");
        elevationInput = Input.GetAxisRaw("Elevation");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        HandleCrossHair();
    }

    void FixedUpdate()
    {
        HandleAcceleration();
        HandleRoll();
        HandleYawAndPitch();
        HandleElevation();

        //Debug.Log(rb.velocity);
        //Debug.Log(currentForwardSpeed);
        Debug.Log(rb.angularVelocity);
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
        rb.AddTorque(-rollInput * rollTorque * transform.forward);

        if (rollInput == 0 && rb.angularVelocity.magnitude > 0)
        {
            rb.AddTorque(-rb.angularVelocity * rollDamping);
        }
    }

    void HandleYawAndPitch()
    {
        // Calculate yaw and pitch amounts based on normalized values and speeds
        float yawAmount = crosshairOrigin.x * yawTorque * Time.deltaTime;
        float pitchAmount = -crosshairOrigin.y * pitchTorque * Time.deltaTime;

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

    void HandleCrossHair()
    {
        // Get mouse input
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // Add input to crosshair
        crosshair.position += new Vector3(x * crosshairMultiplier, y * crosshairMultiplier, 0.0f);

        // Move crosshair to origin to apply circular clamp
        crosshairOrigin = crosshair.position - screenCenter;

        // Clamp vector magnitude to 10
        if (crosshairOrigin.sqrMagnitude > radius)
        {
            crosshairOrigin.Normalize();
            crosshairOrigin *= normalizeMagnitude;
        }

        // Update crosshair position
        crosshair.position = crosshairOrigin + screenCenter;

        Debug.Log(crosshairOrigin);
        //Debug.Log(Input.mousePosition.x + ", " + Input.mousePosition.y);
    }
}