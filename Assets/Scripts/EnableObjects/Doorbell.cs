using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Doorbell : LivableObject
{
    public CinemachineVirtualCamera fixedCamera;
    public CinemachineVirtualCamera playerCamera;

    public bool isInteracting;

    protected override void Update()
    {
        base.Update();

    }
}
