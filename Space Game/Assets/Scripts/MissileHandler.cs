using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MissileHandler : MonoBehaviour
{
    public GameObject targetObject;
    public string targetTag = "Team 1";

    public enum MissileState
    {
        Dropping,
        Boosting,
        Homing,
        Inactive
    }

    public MissileState missileState = MissileState.Dropping;

    public float dropDuration = 0.25f;
    public float dropForce = 0.25f;
    public float boostDuration = 0.25f;
    public float boostForce = 0.25f;
    public float maxVelocity = 25.0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetObject = GameObject.FindGameObjectWithTag(targetTag);
        StartCoroutine(DropPhaseEndBoostPhaseStart(dropDuration));
    }

    private void FixedUpdate()
    {
        switch (missileState)
        {
            case MissileState.Dropping:
                Drop();
                break;
            case MissileState.Boosting:
                Boost();
                break;
            case MissileState.Homing:
                Home();
                break;
        }

        Debug.Log(rb.velocity);
    }

    private void Drop()
    {
        rb.AddForce(transform.up * -dropForce);
    }

    private void Boost()
    {
        // Counteract drop force to stabilize
        rb.AddForce(transform.up * dropForce);

        // Boost
        rb.AddForce(transform.forward * boostForce);
    }

    private void Home()
    {
        transform.LookAt(targetObject.transform);
        rb.AddForce(transform.forward * boostForce);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private IEnumerator DropPhaseEndBoostPhaseStart(float dropDuration)
    {
        yield return new WaitForSeconds(dropDuration);
        missileState = MissileState.Boosting;
        StartCoroutine(BoostPhaseEndHomePhaseStart(boostDuration));
    }

    private IEnumerator BoostPhaseEndHomePhaseStart(float boostDuration)
    {
        yield return new WaitForSeconds(boostDuration);
        missileState = MissileState.Homing;
    }
}
