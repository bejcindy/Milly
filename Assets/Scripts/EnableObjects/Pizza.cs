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
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.name);
                }

            }
            else
            {
                selected = false;
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
            transform.GetChild(1).gameObject.layer = 9;
        else if(inHand)
            transform.GetChild(1).gameObject.layer = 7;
        else if(activated)
            transform.GetChild(1).gameObject.layer = 17;
        else
            transform.GetChild(1).gameObject.layer = 0;
        

        if (!IsObjectVisible(rend))
            selected = false;
    }


    private void OnMouseOver()
    {
        if(lid.openLid && inBox)
        {
            selected = true;
            playerHolding.selectedObj = gameObject;
        }

        if (!lid.openLid)
            selected = false;
    }

    private void OnMouseExit()
    {
        if(lid.openLid && inBox)
        {
            selected = false;
            if (playerHolding.selectedObj == this)
                playerHolding.selectedObj = null;
        }

    }

    //protected override void OnBecameInvisible()
    //{
    //    base.OnBecameInvisible();
    //    selected = false;
    //}
}
