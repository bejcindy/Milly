using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolding : MonoBehaviour
{
    public bool isHolding;
    public Vector3 holdingPosition;
    public Vector2 minThrowForce = new Vector2(100f, 50f);
    public Vector2 maxThrowForce = new Vector2(500f, 200f);
    public Transform holdingObj;

    public bool noFirstThrow;

    float holdTime = 2f;
    [SerializeField]
    float holdTimer;
    Vector2 throwForce;

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
                noFirstThrow = !noFirstThrow;
                holdTimer = 0;
                if (noFirstThrow)
                {
                    isHolding = false;
                    //firstClickDone = false;
                    holdingObj.SetParent(null);
                    holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                    holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * transform.forward + new Vector3(0, throwForce.y, 0));
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                isHolding = false;
                noFirstThrow = true;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
            }
            if (Input.GetMouseButton(0) && !noFirstThrow)
            {
                if (holdTimer < holdTime)
                {
                    holdTimer += Time.deltaTime;
                    holdingObj.position -= Camera.main.transform.forward * Time.deltaTime * .1f;
                }
                float throwForceX = Mathf.Lerp(minThrowForce.x, maxThrowForce.x, Mathf.InverseLerp(0, holdTime, holdTimer));
                float throwForceY = Mathf.Lerp(minThrowForce.y, maxThrowForce.y, Mathf.InverseLerp(0, holdTime, holdTimer));
                throwForce = new Vector2(throwForceX, throwForceY);
            }
        }
    }
}
