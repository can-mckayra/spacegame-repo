using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 initialPosition;
    private Vector3 initialVelocity;
    private Quaternion initialRotation;

    void Start()
    {
        // Get the Rigidbody component of the object
        rb = GetComponent<Rigidbody>();

        // Save the initial position, velocity, and rotation
        initialPosition = transform.position;
        initialVelocity = rb.velocity;
        initialRotation = transform.rotation;
    }                                        
                                             
    void Update()                            
    {
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Reset the object's position, speed, and rotation
            ResetPositionSpeedAndRotation();
        }
    }

    void ResetPositionSpeedAndRotation()
    {
        // Reset the position to the initial position
        transform.position = initialPosition;

        // Reset the velocity to the initial velocity
        rb.velocity = initialVelocity;

        // Reset the rotation to the initial rotation
        transform.rotation = initialRotation;

        // Reset angular velocity to zero
        rb.angularVelocity = Vector3.zero;
    }
}
