using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingObject : LivableObject
{
    bool interacted;

    protected override void Update()
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
<<<<<<< HEAD

            if (interacted)
            {
                base.Unfocus(interacted);
                //Debug.Log(gameObject.name + ": " + interacted);
            }
=======
            if(interacted)
                Unfocus(interacted);
>>>>>>> df17efe46e7f9cce5c908066e88c39f729ff6256
        }

        //if (firstActivated)
        //    gameObject.layer = 0;

    }

}
