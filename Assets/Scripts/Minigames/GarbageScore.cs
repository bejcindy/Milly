using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageScore : MonoBehaviour
{
    [SerializeField]
    int score;

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
        if (other.GetComponent<PickUpObject>())
        {
            score++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PickUpObject>())
        {
            score--;
        }
    }
}
