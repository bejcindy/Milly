using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector1 : MonoBehaviour
{
    public float autoCloseTimer;
    private float autoCloseTimerVal;
    public bool hasHuman;
    public Door myDoor;
    void Start()
    {
        autoCloseTimerVal = 10f;
        myDoor = transform.parent.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHuman && myDoor.doorOpen)
        {
            if (autoCloseTimer > 0)
                autoCloseTimer -= Time.deltaTime;
            else
            {
                myDoor.CloseDoor();
                autoCloseTimer = autoCloseTimerVal;
            }

        }
        else
        {
            autoCloseTimer = autoCloseTimerVal;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("Player"))
        {
            hasHuman = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("NPC") || other.CompareTag("Player"))
        {
            hasHuman = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("NPC") || other.CompareTag("Player"))
        {
            hasHuman = false;
        }
    }


}
