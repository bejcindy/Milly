using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : PickUpObject
{
    public static bool hasBroom;
    public GroundDirt selectedDirt;
    public CharacterTattoo broomTat;
    bool tatTriggered;

    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.BROOM;
        hasBroom = false;
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
            ReferenceTool.playerHolding.dirtObj = selectedDirt.gameObject;
            if (!selectedDirt.cleaned)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Invoke(nameof(SweepDirt), 0.5f);
                }
            }
            else
            {
                selectedDirt = null;
                ReferenceTool.playerHolding.dirtObj = null;
            }


        }
        else
        {
            ReferenceTool.playerHolding.dirtObj = null;
        }


    }

    public void TriggerTat()
    {
        if (!tatTriggered)
        {
            broomTat.triggered = true;
            tatTriggered = true;
        }
    }

    void SweepDirt()
    {
        if(selectedDirt)
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
