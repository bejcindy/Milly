using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolding : MonoBehaviour
{
    public bool isHolding;
    public Vector3 holdingPosition;
    public Vector2 throwForce = new Vector2(400f, 200f);
    public Transform holdingObj;

    public bool firstClickDone;
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
            if (Input.GetMouseButtonUp(0))
            {
                firstClickDone = true;
            }
            if (Input.GetMouseButtonDown(1) && firstClickDone)
            {
                isHolding = false;
                firstClickDone = false;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
            }
            if (Input.GetMouseButtonDown(0) && firstClickDone)
            {
                isHolding = false;
                firstClickDone = false;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x*transform.forward+new Vector3(0,throwForce.y,0));
            }
        }
    }
}
