using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Force and Velocity Controls")]
    [SerializeField] private float accelerationForce = 200f;
    [SerializeField] private float decelerationForce = 100f;
    [SerializeField] private float maxForwardVelocity = 250f;
    [SerializeField] private float maxBackwardVelocity = 50f;
    [SerializeField] private float rollTorque = 20f;
    //public float maxRollVelocity = 500f; reminder to implement angular velocity clamp.
    [SerializeField] private float rollDamping = 20f;
    [SerializeField] private float yawTorque = 25f;
    [SerializeField] private float pitchTorque = 25f;
    //[SerializeField] private float maxYawVelocity = 50f;
    //[SerializeField] private float maxPitchVelocity = 50f;
    [SerializeField] private float elevationForce = 100f;
    //[SerializeField] private float maxElevationVelocity = 100f;
    [SerializeField] private float elevationDamping = 0.1f;

    // Inputs
    private float accelerationInput;
    private float rollInput;
    private float elevationInput;
    private float mouseX;
    private float mouseY;

    [SerializeField] private bool yawPitchLocked = true;

    [Header("Crosshair Handling")]
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float crosshairMultiplier = 10.0f;
    [SerializeField] private float normalizeMagnitude = 100.0f;
    [SerializeField] private float radius = 10000.0f;

    private Vector3 screenCenter;
    private Vector3 crosshairOrigin;

    [SerializeField] private float stabilizationSpeed = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.maxAngularVelocity = 5f;

        Cursor.lockState = CursorLockMode.Locked;
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
        HandleCrosshair();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            yawPitchLocked = !yawPitchLocked;
        }
    }

    void FixedUpdate()
    {
        HandleAcceleration();
        HandleRoll();
        HandleYawAndPitch();
        HandleElevation();

        Stabilize();

        Debug.Log(rb.velocity.magnitude);
        //Debug.Log(currentForwardSpeed);
        //Debug.Log(rb.angularVelocity);
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

        // Cap yaw and pitch speed (Doesn't work)
        //float clampedYawAmount = Mathf.Clamp(yawAmount, -maxYawVelocity, maxYawVelocity);
        //float clampedPitchAmount = Mathf.Clamp(pitchAmount, -maxPitchVelocity, maxPitchVelocity);

        if (!yawPitchLocked)
        {
            // Apply yaw and pitch torques to the rigidbody
            rb.AddTorque(transform.up * yawAmount);
            rb.AddTorque(transform.right * pitchAmount); // Use right instead of -transform.right for inverted pitch
        }
    }

    void HandleElevation()
    {
        rb.AddForce(elevationForce * elevationInput * transform.up);
    }

    void HandleCrosshair()
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
    }

    void Stabilize()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Calculate the opposite force to gradually reduce velocity
            Vector3 oppositeVelocity = stabilizationSpeed * Time.deltaTime * -rb.velocity;
            rb.AddForce(oppositeVelocity, ForceMode.VelocityChange);

            // Calculate the opposite torque to gradually reduce angular velocity
            Vector3 oppositeAngularVelocity = stabilizationSpeed * Time.deltaTime * -rb.angularVelocity;
            rb.AddTorque(oppositeAngularVelocity, ForceMode.VelocityChange);
        }
    }
}
