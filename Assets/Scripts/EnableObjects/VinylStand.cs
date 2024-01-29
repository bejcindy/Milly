using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class VinylStand : LivableObject
{
    [Foldout("Vinyl Stand")]
    public Vinyl selectedVinyl;
    public Vinyl holdingVinyl;
    public Transform availableSpot;
    public Transform[] holdingPositions;
    public int selectIndex;
    public bool inScrollCD;
    public float scrollCD;
    float scrollCDVal;

    protected override void Start()
    {
        base.Start();
        scrollCDVal = scrollCD;
    }

    protected override void Update()
    {
        base.Update();

        if (interactable)
        {
            if (playerLeftHand.isHolding)
            {
                if (!availableSpot)
                    CheckAvailableSpot();
                selectedVinyl = null;

                if (playerLeftHand.holdingObj.GetComponent<Vinyl>())
                {

                    holdingVinyl = playerLeftHand.holdingObj.GetComponent<Vinyl>();
                    if(availableSpot)
                        DetectVinylPlacement();
                }
                else
                {
                    if (activated)
                        gameObject.layer = 17;
                    else
                        gameObject.layer = 0;
                }
            }
            else
            {
                if (activated)
                    gameObject.layer = 17;
                else
                    gameObject.layer = 0;

                if (!selectedVinyl)
                    SelectInitialVinyl();
                else
                {
                    if(!inScrollCD)
                        PlayerChooseVinyl();
                    selectedVinyl.standSelect = true;
                    selectedVinyl.selected = true;
                }
            }
        }
        else
        {
            selectedVinyl = null;
            holdingVinyl = null;
            availableSpot = null;
            if (activated)
                gameObject.layer = 17;
            else
                gameObject.layer = 0;
        }

        if (inScrollCD)
        {
            if(scrollCD > 0)
            {
                scrollCD -= Time.deltaTime;
            }
            else
            {
                inScrollCD = false;
                scrollCD = scrollCDVal;
            }
        }

    }

    void SelectInitialVinyl()
    {
        foreach(Transform t in holdingPositions)
        {
            if(t.childCount > 0)
            {
                selectedVinyl = t.GetChild(0).GetComponent<Vinyl>();
                return;
            }
        }
    }

    void PlayerChooseVinyl()
    {
        selectIndex = selectedVinyl.transform.parent.GetSiblingIndex();
        if (Input.mouseScrollDelta.y > 0)
        {
            inScrollCD = true;
            if(selectIndex < holdingPositions.Length - 1)
            {
                if (holdingPositions[selectIndex++].childCount > 0)
                {
                    selectedVinyl.selected = false;
                    selectedVinyl.standSelect = false;
                    selectedVinyl = holdingPositions[selectIndex++].GetChild(0).GetComponent<Vinyl>();
                }
            }
            else
            {
                if (holdingPositions[0].childCount > 0)
                {
                    selectedVinyl.selected = false;
                    selectedVinyl.standSelect = false;
                    selectedVinyl = holdingPositions[0].GetChild(0).GetComponent<Vinyl>();
                }
            }

        }

        if(Input.mouseScrollDelta.y < 0)
        {
            inScrollCD = true;
            if (selectIndex > 0)
            {
                if (holdingPositions[selectIndex--].childCount > 0)
                {
                    selectedVinyl.selected = false;
                    selectedVinyl.standSelect = false;
                    selectedVinyl = holdingPositions[selectIndex--].GetChild(0).GetComponent<Vinyl>();
                }
            }
            else
            {
                if (holdingPositions[3].childCount > 0)
                {
                    selectedVinyl.selected = false;
                    selectedVinyl.standSelect = false;
                    selectedVinyl = holdingPositions[3].GetChild(0).GetComponent<Vinyl>();
                }
            }
        }
    }

    void DetectVinylPlacement()
    {
        gameObject.layer = 9;
        if (Input.GetMouseButtonDown(0))
        {
            holdingVinyl.inHand = false;
            holdingVinyl.onStand = true;
            holdingVinyl.selected = false;
            holdingVinyl.transform.SetParent(availableSpot);
            holdingVinyl.transform.localPosition = Vector3.zero;
            holdingVinyl.transform.localRotation = Quaternion.identity;
            playerHolding.UnoccupyLeft();
            holdingVinyl = null;
            availableSpot = null;
            activated = true;
        }
    }

    bool CheckAvailableSpot()
    {
        foreach(var obj in holdingPositions)
        {
            if (obj.childCount <1)
            {
                availableSpot = obj;
                return true;
            }
        }
        availableSpot = null;
        return false;
    }
}
