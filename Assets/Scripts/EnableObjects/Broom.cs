using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : PickUpObject
{
    public static bool hasBroom;
    public bool sweeping;
    public GroundDirt selectedDirt;
    float originalVerticalSpeed, originalHorizontalSpeed;

    CinemachinePOV pov;
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.BROOM;
        pov = ReferenceTool.playerPOV;
        originalHorizontalSpeed = pov.m_HorizontalAxis.m_MaxSpeed;
        originalVerticalSpeed = pov.m_VerticalAxis.m_MaxSpeed;
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
            if (Input.GetMouseButton(0))
            {
                sweeping = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                sweeping = false;
            }
        }
        else
        {
            sweeping = false;
        }

        if (!sweeping)
        {
            pov.m_HorizontalAxis.m_MaxSpeed = originalHorizontalSpeed;
            pov.m_VerticalAxis.m_MaxSpeed = originalVerticalSpeed;
        }
        else{

            pov.m_HorizontalAxis.m_MaxSpeed = 0;
            pov.m_VerticalAxis.m_MaxSpeed = 0;
        }
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
