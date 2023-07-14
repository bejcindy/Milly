using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : LivableObject
{
    public string tagName;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == tagName)
            activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player stepped in");
            activated = true;
        }
    }
}
