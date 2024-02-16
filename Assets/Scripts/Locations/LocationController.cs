using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    public Transform myTitle;
    public bool inZone;
    public Transform playerSpawnPos;

    bool nameDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }
}
