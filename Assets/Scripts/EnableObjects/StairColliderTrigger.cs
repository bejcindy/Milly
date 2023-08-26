using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairColliderTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<LivableObject>().activated = true;
        }
    }
}
