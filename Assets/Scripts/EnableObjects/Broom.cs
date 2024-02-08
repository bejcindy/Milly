using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : PickUpObject
{
    public static bool hasBroom;
    public GroundDirt selectedDirt;

    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.BROOM;

    }

    protected override void Update()
    {
        base.Update();
        LayerDetection();

        if (inHand)
        {
            hasBroom = true;
        }
        else
        {
            hasBroom = false;
        }

        if (selectedDirt)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Invoke(nameof(SweepDirt), 0.5f);   
            }

        }


    }

    void SweepDirt()
    {
        selectedDirt.SweepDirt();
    }

    void LayerDetection()
    {
        if (selected && !thrown)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 9;
            }
        }
        else if (inHand)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 7;
            }
        }
        else if (activated)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 0;
            }
        }
    }
}
