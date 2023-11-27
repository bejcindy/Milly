using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletTrash : LivableObject
{

    public Animator trash;

    bool inCD;
    bool usingTrash;
    float cdVal = 1f;

    protected override void Update()
    {
        base.Update();


        if (interactable)
        {
            UseTrash();

        }
        else
        {
            gameObject.layer = 0;
            inCD = false;
            cdVal = 1f;
        }
    }

    public void UseTrash()
    {
        if (!inCD && !usingTrash)
        {
            gameObject.layer = 9;
            if (Input.GetKey(KeyCode.Q))
            {
                usingTrash = true;
                trash.SetTrigger("Open");
            }

        }
        else if(usingTrash)
        {
            gameObject.layer = 0;
            if (Input.GetKeyUp(KeyCode.Q))
            {
                inCD = true;
                usingTrash = false;
                trash.ResetTrigger("Open");
                trash.SetTrigger("Close");
            }
        }
        else if (inCD)
        {
            gameObject.layer = 0;
            if (cdVal > 0f)
                cdVal -= Time.deltaTime;
            else
            {
                cdVal = 1f;
                inCD = false;
            }
        }
    }



}
