using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadarCheck : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private MissileSpawner missileSpawner;

    public GameObject targetObject;
    //public int targetPlayerID;

    [SerializeField] private bool lineOfSightClear = false;
    [SerializeField] private Vector3 relativeVector;
    [SerializeField] private float raycastMaxRange = 1500f;
    [SerializeField] private LayerMask obstacleLayer;

    private void Update()
    {
        if (targetObject != null)
        {
            LineOfSightCheck();
        }
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
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12)
        {
            if (other.gameObject.layer != player.teamID + 10)
            {
                targetObject = other.gameObject;
                //targetPlayerID = other.gameObject.GetComponent<Player>().playerID;
            }
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        LineOfSightCheck();
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12)
        {
            if (other.gameObject.layer != player.teamID + 10)
            {
                targetObject = null;
                lineOfSightClear = false;
            }
        }
    }
    
    private void LineOfSightCheck()
    {
        if (targetObject != null)
        {
            relativeVector = targetObject.transform.position - transform.position;
            if (relativeVector.magnitude <= raycastMaxRange)
            {
                if (Physics.Raycast(transform.position, relativeVector, out RaycastHit hit, raycastMaxRange, obstacleLayer))
                {
                    if (hit.collider != null)
                    {
                        lineOfSightClear = false;
                        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                        //Debug.Log("Line of sight NOT clear with raycast hitting layer: " + hit.collider.gameObject.layer);
                    }
                    else
                    {
                        //Debug.Log("Error");
                    }
                }
                else
                {
                    lineOfSightClear = true;
                    Debug.DrawRay(transform.position, relativeVector, Color.green);
                    //Debug.Log("Line of sight clear!");
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * raycastMaxRange, Color.red);
                //Debug.Log("Out of range!");
            }
        }
    }
}
