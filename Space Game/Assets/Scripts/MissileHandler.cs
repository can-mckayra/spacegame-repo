using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MissileHandler : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetTag = "Team 1";

    [SerializeField]
    private enum MissileState
    {
        Dropping,
        Boosting,
        Homing
    }

    [SerializeField] private MissileState missileState = MissileState.Dropping;

    [SerializeField] private float dropDuration = 0.5f;
    [SerializeField] private float dropForce = 0.5f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostForce = 1.5f;
    [SerializeField] private float homeForce = 10f;
    [SerializeField] private float maxVelocity = 100f;

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

        //Debug.Log(rb.velocity.magnitude);
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
        rb.AddForce(transform.forward * homeForce);
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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
