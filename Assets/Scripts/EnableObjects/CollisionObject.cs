using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : LivableObject
{
    private void OnCollisionEnter(Collision collision)
    {
        activated = true;
    }
}
