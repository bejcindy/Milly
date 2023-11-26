using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignatedSpot : MonoBehaviour
{
    public LookingObject[] lookingObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(LookingObject lookingObject in lookingObjects)
            {
                lookingObject.inSpot = true;
            }
        }
    }
}
