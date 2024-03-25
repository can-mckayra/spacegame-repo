using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        rb.AddForce(new Vector3(0f, 0f, 50f), ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z > 2430f)
        {
            transform.position = initialPosition;
        }
        //Debug.Log(rb.velocity);
    }
}
