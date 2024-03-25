using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MissileHandler : MonoBehaviour
{
    [SerializeField] private RadarCheck radarCheck;
    [SerializeField] private MissileSpawner missileSpawner;

    [SerializeField] private GameObject lockedObject;

    private enum MissileState
    {
        Dropping,
        //Boosting,
        Homing
    }

    [SerializeField] private MissileState missileState = MissileState.Dropping;

    [SerializeField] private float dropDuration = 0.5f;
    [SerializeField] private float dropForce = 0.5f;
    //[SerializeField] private float boostDuration = 1f;
    //[SerializeField] private float boostForce = 1.5f;
    [SerializeField] private float homeForce;
    [SerializeField] private float maxVelocity = 100f;
    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        radarCheck = FindObjectOfType<RadarCheck>();
        lockedObject = radarCheck.targetObject;
        missileSpawner = FindObjectOfType<MissileSpawner>();
        maxVelocity += missileSpawner.spaceshipVelocityMagnitude;
        homeForce = maxVelocity / 10;
        StartCoroutine(DropPhaseEndBoostPhaseStart(dropDuration));
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        switch (missileState)
        {
            case MissileState.Dropping:
                Drop();
                break;
            //case MissileState.Boosting:
            //    Boost();
            //    break;
            case MissileState.Homing:
                Home();
                break;
        }

        Debug.Log(rb.velocity.magnitude);
        //Debug.Log("maxVelocity; " + maxVelocity + "\thomeForce: " + homeForce);
    }

    private void Drop()
    {
        rb.AddForce(transform.up * -dropForce);
    }

    /*
    private void Boost()
    {
        // Counteract drop force to stabilize
        //rb.AddForce(transform.up * dropForce);

        // Boost
        rb.AddForce(transform.forward * boostForce);
    }
    */

    private void Home()
    {
        //transform.LookAt(lockedObject.transform);

        // Calculate the direction to the target
        Vector3 targetDirection = lockedObject.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.AddForce(transform.forward * homeForce);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private IEnumerator DropPhaseEndBoostPhaseStart(float dropDuration)
    {
        yield return new WaitForSeconds(dropDuration);
        missileState = MissileState.Homing;
        //StartCoroutine(BoostPhaseEndHomePhaseStart(boostDuration));
    }

    /*
    private IEnumerator BoostPhaseEndHomePhaseStart(float boostDuration)
    {
        yield return new WaitForSeconds(boostDuration);
        missileState = MissileState.Homing;
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
