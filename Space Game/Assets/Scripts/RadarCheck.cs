using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCheck : MonoBehaviour
{
    public bool targetLocked = false;
    public MissileSpawner missileSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team 1"))
        {
            targetLocked = true;
            if (missileSpawner != null)
            {
                missileSpawner.targetLocked = targetLocked;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Team 1"))
        {
            targetLocked = false;
            if (missileSpawner != null)
            {
                missileSpawner.targetLocked = targetLocked;
            }
        }
    }
}
