using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCheck : MonoBehaviour
{
    public MissileSpawner missileSpawner;

    // Line of sight checkers
    [SerializeField] private bool lineOfSightClear = false;
    [SerializeField] private GameObject target = null;
    [SerializeField] private Vector3 relativeVector;
    [SerializeField] private float relativeVectorMagnitude;
    [SerializeField] private float maxRange = 1500f;
    [SerializeField] private LayerMask obstacleLayer;

    private void Update()
    {
        if (missileSpawner != null)
        {
            if (lineOfSightClear)
            {
                missileSpawner.targetLocked = true;
            }
            else
            {
                missileSpawner.targetLocked = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team 1"))
        {
            target = other.gameObject;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        LineOfSightCheck();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Team 1"))
        {
            target = null;
        }
    }

    private void LineOfSightCheck()
    {
        if (target != null)
        {
            relativeVector = target.transform.position - transform.position;
            relativeVectorMagnitude = relativeVector.magnitude;
            if (relativeVector.magnitude <= maxRange)
            {
                if (Physics.Raycast(transform.position, relativeVector, out RaycastHit hit, maxRange, obstacleLayer))
                {
                    if (hit.collider != null)
                    {
                        lineOfSightClear = false;
                        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                        Debug.Log("Line of sight NOT clear with raycast hitting layer: " + hit.collider.gameObject.layer);
                    }
                    else
                    {
                        Debug.Log("Error");
                    }
                }
                else
                {
                    lineOfSightClear = true;
                    Debug.DrawRay(transform.position, relativeVector, Color.green);
                    Debug.Log("Line of sight clear!");
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * maxRange, Color.red);
                Debug.Log("Out of range!");
            }
        }
    }
}
