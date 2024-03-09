using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairColliderTrigger : MonoBehaviour
{    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<PassiveActivation>().activated = true;
        }
    }
}
