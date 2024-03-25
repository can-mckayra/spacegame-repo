using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField] private RadarCheck radarCheck;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform launchPoint; // The point under the spaceship where the missile will be instantiated

    public bool targetLocked = false;

    public float spaceshipVelocityMagnitude;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && radarCheck != null)
        {
            if (radarCheck.targetLocked == true)
            {
                SpawnMissile();
                Debug.Log("Target locked! Firing!");
            }
        }
        else if (Input.GetKeyDown(KeyCode.V) && radarCheck != null)
        {
            if (radarCheck.targetLocked == false)
            {
                Debug.Log("Target not found!");
            }
        }
    }

    private void FixedUpdate()
    {
        spaceshipVelocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;
    }

    private void SpawnMissile()
    {
        if (missilePrefab != null && launchPoint != null)
        {
            // Set the launch position
            Vector3 missilePosition = launchPoint.position;

            // Get the spaceship's current rotation
            Quaternion spaceshipRotation = transform.rotation;

            // Instantiate the missile at the calculated position with the desired rotation
            GameObject missile = Instantiate(missilePrefab, missilePosition, spaceshipRotation);

            // Get the missile's rigidbody and set its velocity to match the spaceship's velocity
            if (missile.TryGetComponent<Rigidbody>(out var missileRb))
            {
                // Set the initial velocity to match the spaceship's velocity
                missileRb.velocity = GetComponent<Rigidbody>().velocity;
            }
        }
    }
}
