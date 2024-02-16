using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class VinylStand : LivableObject
{
    [Foldout("Vinyl Stand")]
    public Vinyl selectedVinyl;
    public Vinyl holdingVinyl;
    public VinylHolder availableSpot;
    public VinylHolder[] holdingPositions;
    public int selectIndex;
    public bool inScrollCD;
    public float scrollCD;
    float scrollCDVal;
    readonly string vinylPlaceSFX = "event:/Sound Effects/ObjectInteraction/Vinyls/Vinyl_Stand";
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
                if (selectedVinyl)
                {
                    if (selectedVinyl.activated)
                    {
                        selectedVinyl.gameObject.layer = 17;
                    }
                    else
                    {
                        selectedVinyl.gameObject.layer = 0;
                    }
                }
                selectedVinyl = null;

                if (playerLeftHand.holdingObj.GetComponent<Vinyl>())
                {

                    holdingVinyl = playerLeftHand.holdingObj.GetComponent<Vinyl>();
                    if(availableSpot && !playerLeftHand.noThrow)
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


                if((playerHolding.selectedObj == null || playerHolding.selectedObj.GetComponent<PickUpObject>().selected))
                {
                    if (!selectedVinyl)
                        SelectInitialVinyl();
                    else
                    {
                        if (!inScrollCD)
                        {
                            selectIndex = selectedVinyl.transform.parent.GetSiblingIndex();
                            PlayerChooseVinyl();
                        }

                        selectedVinyl.gameObject.layer = 9;
                        selectedVinyl.songInfo.SetActive(true);
                        playerHolding.vinylObj = selectedVinyl.gameObject;
                        DataHolder.ShowHint(DataHolder.hints.vinylStandHint);
                        if (Input.GetMouseButtonDown(0))
                        {
                            RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Vinyls/Vinyl_Grab", Camera.main.transform.position);
                            selectedVinyl.holder.ReleaseVinyl();
                            playerHolding.OccupyLeft(selectedVinyl.transform);
                            selectedVinyl.songInfo.SetActive(false);
                            selectedVinyl = null;
                            playerHolding.vinylObj = null;                                                      
                            DataHolder.HideHint(DataHolder.hints.vinylStandHint);
                        }
                    }
                }

            }
        }
        else
        {
            if (selectedVinyl)
            {
                if (selectedVinyl.activated)
                {
                    selectedVinyl.gameObject.layer = 17;
                }
                else
                {
                    selectedVinyl.gameObject.layer = 0;
                }
                selectedVinyl.songInfo.SetActive(false);
            }
            selectedVinyl = null;
            holdingVinyl = null;
            availableSpot = null;
            playerHolding.vinylObj = null;
            DataHolder.HideHint(DataHolder.hints.vinylStandHint);
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
        foreach(VinylHolder t in holdingPositions)
        {
            if(t.hasVinyl)
            {
                selectedVinyl = t.myVinyl;
                return;
            }
        }
    }

    Vinyl FindNextVinyl(int startIndex, bool forward)
    {
        if (forward)
        {
            for (int i = startIndex; i < holdingPositions.Length; i++)
            {
                if (holdingPositions[i].hasVinyl)
                {
                    return holdingPositions[i].myVinyl;
                }
            }
            return selectedVinyl;

        }
        else
        {
            for (int i = startIndex; i >= 0; i--)
            {
                if (holdingPositions[i].hasVinyl)
                {
                    return holdingPositions[i].myVinyl;
                }
            }
            return selectedVinyl;
        }

    }

    void PlayerChooseVinyl()
    {
        
        if (Input.mouseScrollDelta.y > 0)
        {
            inScrollCD = true;
            if (selectIndex < holdingPositions.Length - 1)
            {
                if (selectedVinyl.activated)
                {
                    selectedVinyl.gameObject.layer = 17;

                }
                else
                {
                    selectedVinyl.gameObject.layer = 0;
                }
                selectedVinyl.songInfo.SetActive(false);
                selectedVinyl = FindNextVinyl(selectIndex +1, true);

            }
            else
            {
                if (selectedVinyl.activated)
                {
                    selectedVinyl.gameObject.layer = 17;
                }
                else
                {
                    selectedVinyl.gameObject.layer = 0;
                }
                selectedVinyl.songInfo.SetActive(false);
                selectedVinyl = FindNextVinyl(0, true);
            }


        }

        if(Input.mouseScrollDelta.y < 0)
        {
            inScrollCD = true;
            if (selectIndex > 0)
            {
                if (selectedVinyl.activated)
                {
                    selectedVinyl.gameObject.layer = 17;
                }
                else
                {
                    selectedVinyl.gameObject.layer = 0;
                }
                selectedVinyl.songInfo.SetActive(false);
                selectedVinyl = FindNextVinyl(selectIndex - 1, false);

            }
            else
            {
                if (selectedVinyl.activated)
                {
                    selectedVinyl.gameObject.layer = 17;
                }
                else
                {
                    selectedVinyl.gameObject.layer = 0;
                }
                selectedVinyl.songInfo.SetActive(false);
                selectedVinyl = FindNextVinyl(holdingPositions.Length-1, false);
            }
        }
    }

    void DetectVinylPlacement()
    {
        gameObject.layer = 9;
        if (Input.GetMouseButtonDown(0))
        {
            RuntimeManager.PlayOneShot(vinylPlaceSFX, Camera.main.transform.position);
            availableSpot.PlaceVinyl(holdingVinyl);
            playerHolding.UnoccupyLeft();
            holdingVinyl = null;
            availableSpot = null;
            activated = true;
        }
    }

    public void PlaceVinyl(Vinyl vinyl)
    {
        if (!availableSpot)
        {
            CheckAvailableSpot();
        }
        availableSpot.PlaceVinyl(vinyl);
        availableSpot = null;
    }


    bool CheckAvailableSpot()
    {
        foreach(var obj in holdingPositions)
        {
            if (!obj.hasVinyl)
            {
                availableSpot = obj;
                return true;
            }
        }
        availableSpot = null;
        return false;
    }
}
