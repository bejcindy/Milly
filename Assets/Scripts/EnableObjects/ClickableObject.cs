using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : LivableObject
{
    protected override void Update()
    {
        base.Update();
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 6f))
                {
                    if (raycastHit.transform.name == transform.name)
                    {
                        activated = true;
                    }
                }
            }
        }
    }
}
