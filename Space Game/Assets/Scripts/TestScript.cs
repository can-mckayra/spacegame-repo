using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float rollTorque = 10f;
    public float rollDamping = 10f;

    public float rollInput;

    private Rigidbody rb;
    
    public float mouseX;
    public float mouseY;
    public float yawTorque = 500f;
    public float pitchTorque = 500f;
    public float maxYawVelocity = 500f;
    public float maxPitchVelocity = 500f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rollInput = Input.GetAxisRaw("Horizontal");
        mouseX = Input.mousePosition.x - Screen.width / 2f;
        mouseY = Input.mousePosition.y - Screen.height / 2f;
    }

    private void FixedUpdate()
    {
        HandleRoll();
        HandleYawAndPitch();
        Debug.Log(rb.angularVelocity);
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
}
