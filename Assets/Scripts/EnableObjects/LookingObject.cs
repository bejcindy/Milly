using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingObject : LivableObject
{
    bool interacted;

    protected override void Update()
    {
        if (matColorVal > 0)
        {
            base.Update();
            if (interactable)
            {
                if (!firstActivated)
                {
                    gameObject.layer = 12;
                    base.FocusOnThis();
                    //Debug.Log(gameObject.name + ": focused");
                    interacted = true;
                }


                activated = true;
            }
            else
            {
                //gameObject.layer = 0;
                activated = false;
                if (interacted)
                {
                    Unfocus(interacted);
                    Debug.Log("unfocusing");
                }
            }
        }
        else
        {
            Unfocus(interacted);
            Debug.Log(gameObject.name+"uunfocusinig");
        }
        
        //if (firstActivated)
        //    gameObject.layer = 0;

    }

}
