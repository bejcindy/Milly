using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingObject : LivableObject
{

    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
    }

}
