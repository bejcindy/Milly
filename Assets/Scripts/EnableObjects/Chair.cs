using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : FixedCameraObject
{
    
    protected override void Update()
    {
        base.Update();

        if (isInteracting)
            playerHolding.atTable = true;
        else
            playerHolding.atTable = false;
    }
}
