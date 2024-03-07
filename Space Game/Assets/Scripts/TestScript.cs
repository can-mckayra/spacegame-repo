using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform launchPoint; // The point under the spaceship where the missile will be instantiated

    [Header("Spherecast Variables")]
    [SerializeField] private float radius = 25f;
    [SerializeField] private float range = 1500f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string targetTag = "Team 1";
    [SerializeField] private bool targetLocked = false;

    // For visualization
    //[SerializeField] private GameObject currentHitObject;
    [SerializeField] private float currentHitDistance;

    private void Update()
    {
        Radar();
        
        if (Input.GetKeyDown(KeyCode.V) && targetLocked == true)
        {
            SpawnMissile();
            Debug.Log("Target locked! Firing!");
        }
        else if (Input.GetKeyDown(KeyCode.V) && targetLocked == false)
        {
            Debug.Log("Target not found!");
        }
    }

    private void Radar()
    {
        if (Physics.SphereCast(transform.position + new Vector3(0f, 0f, 0f), radius, transform.forward, out RaycastHit hit, range, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                // For visualization
                //currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                
                targetLocked = true;
            }
        }
        else
        {
            // For visualization
            //currentHitObject = null;
            currentHitDistance = range;
            
            targetLocked = false;
        }
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

    // For visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward * currentHitDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, radius);
    }
}
