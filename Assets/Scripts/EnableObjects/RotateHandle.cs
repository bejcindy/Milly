using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHandle : LivableObject
{

    public bool holdingHandle;
    public bool frontBackRot;
    public CinemachineVirtualCamera playerCinemachine;
    
    PlayerHolding playerHolding;


    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            if (!holdingHandle)
            {
                if (Input.GetMouseButtonDown(2))
                {
                    holdingHandle = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(2))
                {
                    holdingHandle = false;
                }
            }

        }
        else
        {
            holdingHandle = false;
        }

        if (holdingHandle)
        {
            activated = true;
            if(Input.mouseScrollDelta.y > 0)
            {
                if(!frontBackRot)
                    transform.Rotate(Vector3.left * 100 * Time.deltaTime, Space.Self);
                else
                    transform.Rotate(Vector3.back * 100 * Time.deltaTime, Space.Self);
            }
            if(Input.mouseScrollDelta.y < 0)
            {
                if(!frontBackRot)
                    transform.Rotate(Vector3.right * 100 * Time.deltaTime, Space.Self);
                else
                    transform.Rotate(Vector3.forward * 100 * Time.deltaTime, Space.Self);
            }
        }
    }
}
