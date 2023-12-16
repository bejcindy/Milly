using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrontDetector : MonoBehaviour
{
    public bool objectInFront;
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
        if(!other.isTrigger)
        {
            //Debug.Log(other.name);
            objectInFront = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == null)
        {
            objectInFront = false;
        }
    }
}
