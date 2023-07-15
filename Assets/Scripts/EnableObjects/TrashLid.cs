using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLid : LivableObject
{
    public float rotateSpeed;


    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * 75;

            if (Input.GetMouseButton(0))
            {
                activated = true;
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


    void RotateLid(float zTargetAngle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.LerpAngle(transform.eulerAngles.z, zTargetAngle, Time.deltaTime * rotateSpeed));

    }


}
