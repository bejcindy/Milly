using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlow : MonoBehaviour
{
    public float force;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("Wind", Random.Range(0, 2), Random.Range(2, 5));
    }

    void Wind()
    {
        rb.AddForce(transform.right * force);
    }
}
