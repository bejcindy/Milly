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
        }
    }

    protected override void Update()
    {
        if (inBox)
        {
            if (!lid.openLid)
            {
                interactable = false;
                isVisible = false;
            }

            else
            {
                interactable = true;
                isVisible = true;
            }
                
        }
        else
        {
            base.Update();
        }
    }
}
