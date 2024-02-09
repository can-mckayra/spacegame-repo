using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Reference to the spaceship GameObject
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the spaceship

    public float damping = 0f; // Damping factor for smooth camera movement

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, damping * Time.deltaTime);
        transform.position = smoothedPosition;

        // Calculate the desired rotation for the camera
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, target.up);

        // Smoothly rotate the camera towards the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, damping * Time.deltaTime);
    }
}