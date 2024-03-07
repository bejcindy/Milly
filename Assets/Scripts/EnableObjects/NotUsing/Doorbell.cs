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
        }
    }
}
