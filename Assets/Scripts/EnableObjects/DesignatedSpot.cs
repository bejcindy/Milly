using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignatedSpot : MonoBehaviour
{
    public LookingObject[] lookingObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
