using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLid : LivableObject
{
    public float rotateSpeed;
    public bool interacting;
    public bool fixedPos;
    public PlayerHolding playerHolding;

    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
    }
    protected override void Update()
    {
        base.Update();
        if (transform.eulerAngles.z == 0 || transform.eulerAngles.z >= 359 || transform.eulerAngles.z == 270)
            fixedPos = true;
        else
            fixedPos = false;
        if (interactable)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;
            Debug.Log(transform.eulerAngles.z);
            if (playerHolding.GetLeftHand())
            {
                if (Input.GetMouseButton(0))
                {
                    activated = true;
                    interacting = true;
                    if (verticalInput < 0)
                    {
                        RotateLid(0);
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                    }
                }
            }
            if (playerHolding.GetRightHand())
            {
                if (Input.GetMouseButton(1))
                {
                    activated = true;
                    interacting = true;
                    if (verticalInput < 0)
                    {
                        RotateLid(0);
                    }
                    else if (verticalInput > 0)
                    {
                        RotateLid(270);
                    }
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            interacting = false;

        }
        if (!fixedPos && !interacting)
        {
            if (transform.eulerAngles.z < 290)
            {
                RotateLid(270);
            }
            else if (transform.eulerAngles.z != 360)
            {
                RotateLid(0);
            }
        }

    }


    void RotateLid(float zTargetAngle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.LerpAngle(transform.eulerAngles.z, zTargetAngle, Time.deltaTime * rotateSpeed));

    }


}
