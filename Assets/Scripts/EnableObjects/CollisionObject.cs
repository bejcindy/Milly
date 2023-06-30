using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : LivableObject
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            activated = true;
        }
    }
}
