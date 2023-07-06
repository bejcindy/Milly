using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Doorbell : FixedCameraObject
{


    protected override void Update()
    {
        base.Update();
        if (isInteracting)
        {
            Cursor.lockState = CursorLockMode.None;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.transform != null)
                    {
                        Debug.Log(raycastHit.transform.name);
                    }
                }
            }
        }
    }
}
