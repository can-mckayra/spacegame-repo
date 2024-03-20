using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 relativeVector;
    //[SerializeField] private float relativeVectorMagnitude;
    [SerializeField] private float maxRange = 250f;
    [SerializeField] private bool lineOfSightClear = false;

    [SerializeField] private LayerMask obstacleLayer;

    private void Update()
    {
        relativeVector = target.transform.position - transform.position;
        //relativeVectorMagnitude = relativeVector.magnitude;
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
