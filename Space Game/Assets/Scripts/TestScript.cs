using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject targetObject;
    public float rotationSpeed = 1.0f;

    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 targetDirection = targetObject.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
