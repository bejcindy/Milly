using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : PickUpObject
{
    public bool inBox;
    public PizzaLid lid;
    public PizzaBox pizzaBox;

    protected override void Start()
    {
        base.Start();
        if (inBox)
        {
            pizzaBox = transform.parent.GetComponent<PizzaBox>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    protected override void Update()
    {
        if (inBox)
        {
            activated = true;
            if (lid.openLid)
            {
                base.Update();
            }

            if (inHand)
            {
                inBox = false;
                pizzaBox.RemovePizza(transform);
            }
        }
        else
        {
            base.Update();
        }

        if (selected && !thrown)
        {
            transform.GetChild(1).gameObject.layer = 9;
        }
        else
        {
            transform.GetChild(1).gameObject.layer = 7;
        }


    }


    private void OnMouseOver()
    {
        if(lid.openLid && inBox)
            selected = true;
        if (!lid.openLid)
            selected = false;
    }

    private void OnMouseExit()
    {
        //if(lid.openLid && inBox)
            selected = false;
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        selected = false;
    }
}
