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

        if (selected)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }


    }


    private void OnMouseOver()
    {
        if(lid.openLid && inBox)
            selected = true;
    }

    private void OnMouseExit()
    {
        if(lid.openLid && inBox)
            selected = false;
    }
}
