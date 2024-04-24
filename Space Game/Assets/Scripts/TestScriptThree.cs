using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptThree : MonoBehaviour
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        HandleAcceleration();
        HandleElevation();
        HandleClamping();

        Debug.Log("worldVelocity: " + worldVelocity);
        Debug.Log("worldToLocalVelocity: " + worldToLocalVelocity);
    }

    private void HandleAcceleration()
    {
        if (accelerationInput > 0.0f)
        {
            rb.AddForce(accelerationForce * transform.forward);
        }
        else if (accelerationInput < 0.0f)
        {
            rb.AddForce(decelerationForce * -transform.forward);
        }
    }

    private void HandleElevation()
    {
        if (elevationInput > 0.0f)
        {
            rb.AddForce(elevationForce * transform.up);
        }
        else if (elevationInput < 0.0f)
        {
            rb.AddForce(elevationForce * -transform.up);
        }
    }

    private void HandleClamping()
    {
        worldToLocalVelocity.z = Mathf.Clamp(worldToLocalVelocity.z, -maxBackwardVelocity, maxForwardVelocity);
        worldToLocalVelocity.y = Mathf.Clamp(worldToLocalVelocity.y, -maxElevationVelocity, maxElevationVelocity);

        rb.velocity = transform.TransformDirection(worldToLocalVelocity);
    }
}
