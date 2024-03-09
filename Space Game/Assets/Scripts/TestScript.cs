using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Rigidbody rb;

    public float force = 1;
    public float maxVelocity = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
    }
    
    private void FixedUpdate()
    {
        //rb.AddForce(transform.forward * force);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        Debug.Log(rb.velocity);
    }
}
