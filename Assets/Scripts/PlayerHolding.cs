using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PlayerHolding : MonoBehaviour
{

    public bool fullHand;
    public bool inDialogue;
    public List<PickUpObject> pickUpObjects;
    public PickUpObject selectedObj;
    PlayerLeftHand leftHand;
    PlayerRightHand rightHand;

    public GameObject leftHandUI;
    public GameObject rightHandUI;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        rightHand = GetComponent<PlayerRightHand>();
    }

    // Update is called once per frame
    void Update()
    {
        GetFullHand();
        ChooseInteractable();

        if(selectedObj!= null)
        {
            if (GetLeftHand())
            {
                leftHandUI.SetActive(true);
            }
            else if (GetRightHand())
            {
                rightHandUI.SetActive(true);
            }

        }
        else
        {
            leftHandUI.SetActive(false);
            rightHandUI.SetActive(false);
        }
    }




    public void AddInteractable(PickUpObject obj)
    {
        if (!pickUpObjects.Contains(obj))
        {
            pickUpObjects.Add(obj);
        }
    }

    public void RemoveInteractable(PickUpObject obj)
    {
        if (pickUpObjects.Contains(obj))
        {
            pickUpObjects.Remove(obj);
        }
    }

    public void ChooseInteractable()
    {
        if (pickUpObjects.Count <= 0)
        {
            selectedObj = null;
        }
        else if (pickUpObjects.Count == 1)
        {
            if (selectedObj != this && selectedObj != null)
                selectedObj.selected = false;
            selectedObj = pickUpObjects[0];
            selectedObj.selected = true;
        }
        else
        {
            float minDist = Vector3.Distance(pickUpObjects[0].transform.position, transform.position);
            selectedObj = pickUpObjects[0];
            foreach (PickUpObject obj in pickUpObjects)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < minDist)
                {
                    minDist = distance;
                    if (selectedObj != this)
                    {
                        selectedObj.selected = false;
                    }
                    selectedObj = obj;
                }
            }
            selectedObj.selected = true;
        }
    }


    public bool GetLeftHand()
    {
        if (leftHand.isHolding)
        {
            return false;
        }
        leftHand.noThrow = true;
        return true;
    }

    public bool GetRightHand()
    {
        if (rightHand.isHolding)
        {
            return false;
        }
        rightHand.noThrow = true;
        return true;
    }

    public void GetFullHand()
    {
        if (leftHand.isHolding && rightHand.isHolding)
            fullHand = true;
        else
            fullHand = false;
    }

    public void OccupyLeft(Transform obj)
    {
        leftHand.isHolding = true;
        leftHand.holdingObj = obj;
        obj.SetParent(Camera.main.transform);
        obj.localPosition = leftHand.holdingPosition;
        Invoke(nameof(EnableThrowLeft), 0.5f);
    }

    public void OccupyRight(Transform obj)
    {
        rightHand.isHolding = true;
        rightHand.holdingObj = obj;
        obj.SetParent(Camera.main.transform);
        obj.localPosition = rightHand.holdingPosition;
        Invoke(nameof(EnableThrowRight), 0.5f);
    }

    public void UnoccupyLeft()
    {
        leftHand.isHolding = false;
        leftHand.holdingObj = null;
        leftHand.noThrow = true;
    }

    public void UnoccupyRight()
    {
        rightHand.isHolding = false;
        rightHand.holdingObj = null;
        rightHand.noThrow = true;
    }

    public void EnableThrowLeft()
    {
        leftHand.noThrow = false;
    }

    public void EnableThrowRight()
    {
        rightHand.noThrow = false;
    }
}
