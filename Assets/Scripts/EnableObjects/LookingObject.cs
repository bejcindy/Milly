using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingObject : LivableObject
{

    protected override void Update()
    {
        base.Update();
        if (isVisible)
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
    }

}
