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
            if(!firstActivated)
                gameObject.layer = 9;
            activated = true;
        }
        else
        {
            gameObject.layer = 0;
            activated = false;
        }

        if (firstActivated)
            gameObject.layer = 0;

    }

}
