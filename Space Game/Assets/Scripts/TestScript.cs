using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float forwardForce = 50.0f;

    [SerializeField] private float accelerationForce = 200f;
    [SerializeField] private float decelerationForce = 100f;
    [SerializeField] private float maxForwardVelocity = 250f;
    [SerializeField] private float maxBackwardVelocity = 50f;
    [SerializeField] private float elevationForce = 100f;
    [SerializeField] private float maxElevationVelocity = 100f;

    private float accelerationInput;
    private float elevationInput;

    private Vector3 worldVelocity;
    private Vector3 worldToLocalVelocity;
    private Vector3 localToWorldVelocity;

    private Vector3 accelerationVector = new(0.0f, 0.0f, 200.0f);
    private Vector3 decelerationVector = new(0.0f, 0.0f, 100.0f);
    private Vector3 elevationVector = new(0.0f, 100.0f, 0.0f);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.AddForce(new Vector3(0.0f, 0.0f, forwardForce));
    }

    private void Update()
    {
        accelerationInput = Input.GetAxisRaw("Vertical");
        elevationInput = Input.GetAxisRaw("Elevation");
    }

    private void FixedUpdate()
    {
        worldVelocity = rb.velocity;
        worldToLocalVelocity = transform.InverseTransformDirection(worldVelocity);
        localToWorldVelocity = transform.TransformDirection(worldToLocalVelocity);

        //HandleAcceleration();
        //HandleElevation();
        HandleMovement();

        Debug.Log("worldVelocity: " + worldVelocity);
        Debug.Log("worldToLocalVelocity: " + worldToLocalVelocity);
        Debug.Log("localToWorldVelocity: " + localToWorldVelocity);
    }

    /*
    private void HandleAcceleration()
    {
        if (accelerationInput > 0.0f)
        {
            rb.AddForce(transform.TransformDirection(accelerationVector));
        }
        else if (accelerationInput < 0.0f)
        {
            rb.AddForce(transform.TransformDirection(-decelerationVector));
        }

        Vector3 forwardVelocityVector = new(0.0f, 0.0f, worldToLocalVelocity.z);
        Vector3 remainingVelocityVector = new(worldToLocalVelocity.x, worldToLocalVelocity.y, 0.0f);

        forwardVelocityVector = Vector3.ClampMagnitude(forwardVelocityVector, maxForwardVelocity);
        rb.velocity = transform.TransformDirection(forwardVelocityVector + remainingVelocityVector);
    }

    private void HandleElevation()
    {
        if (elevationInput > 0.0f)
        {
            rb.AddForce(transform.TransformDirection(elevationVector));
        }
        else if (elevationInput < 0.0f)
        {
            rb.AddForce(transform.TransformDirection(-elevationVector));
        }

        Vector3 upwardVelocityVector = new(0.0f, worldToLocalVelocity.y, 0.0f);
        Vector3 remainingVelocityVector = new(worldToLocalVelocity.x, 0.0f, worldToLocalVelocity.z);

        upwardVelocityVector = Vector3.ClampMagnitude(upwardVelocityVector, maxElevationVelocity);
        rb.velocity = transform.TransformDirection(upwardVelocityVector + remainingVelocityVector);
    }
    */

    private void HandleMovement()
    {
        if (accelerationInput > 0.0f)
        {
            rb.AddForce(accelerationForce * transform.forward);
        }
        else if (accelerationInput < 0.0f)
        {
            rb.AddForce(decelerationForce * -transform.forward);
        }

        //Vector3 forwardVelocityVector = new(0.0f, 0.0f, worldToLocalVelocity.z);
        //forwardVelocityVector = Vector3.ClampMagnitude(forwardVelocityVector, maxForwardVelocity);

        worldToLocalVelocity.z = Mathf.Clamp(worldToLocalVelocity.z, -maxBackwardVelocity, maxForwardVelocity);

        if (elevationInput > 0.0f)
        {
            rb.AddForce(elevationForce * transform.up);
        }
        else if (elevationInput < 0.0f)
        {
            rb.AddForce(elevationForce * -transform.up);
        }

        //Vector3 upwardVelocityVector = new(0.0f, worldToLocalVelocity.y, 0.0f);
        //upwardVelocityVector = Vector3.ClampMagnitude(upwardVelocityVector, maxElevationVelocity);

        worldToLocalVelocity.y = Mathf.Clamp(worldToLocalVelocity.y, -maxElevationVelocity, maxElevationVelocity);

        //Vector3 remainingVelocityVector = new(worldToLocalVelocity.x, 0.0f, 0.0f);

        rb.velocity = transform.TransformDirection(worldToLocalVelocity);
    }
}
