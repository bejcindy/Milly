using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolding : MonoBehaviour
{
    public bool isHolding;
    public Vector3 holdingPosition;

    public Transform holdingObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectHolding();
    }

    private void DetectHolding()
    {
        if (isHolding)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isHolding = false;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
