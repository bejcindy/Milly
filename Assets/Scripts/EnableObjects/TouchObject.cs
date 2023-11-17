using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : LivableObject
{
    public float coolDown = 2f;
    public bool inCD;

    public Animator toiletAnim;

    protected override void Update()
    {
        base.Update();

        if (interactable && !inCD)
        {
            UseTouchable();
        }
        else if(inCD)
        {
            gameObject.layer = 0;
            if(coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }
            else
            {
                coolDown = 2f;
                inCD = false;
            }
        }
        else
        {
            gameObject.layer = 0;
        }
    }

    void UseTouchable()
    {
        gameObject.layer = 9;
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.layer = 0;
            inCD = true;
            UseResult();
        }
    }

    void UseResult()
    {
        if (!toiletAnim.isActiveAndEnabled)
            toiletAnim.enabled = true;
        else
            toiletAnim.Play("ToiletFlush", 0, 0);
    }
}
