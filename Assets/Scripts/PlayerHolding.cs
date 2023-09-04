using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PlayerHolding : MonoBehaviour
{
    public bool smoking;
    public bool fullHand;
    public bool inDialogue;
    public bool throwing;
    List<GameObject> pickUpObjects;
    public GameObject selectedObj;
    PlayerLeftHand leftHand;
    PlayerRightHand rightHand;

    public GameObject leftHandUI;
    public GameObject rightHandUI;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        rightHand = GetComponent<PlayerRightHand>();
        pickUpObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        GetFullHand();
        ChooseInteractable();

        if (selectedObj != null)
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

        if (DialogueManager.IsConversationActive)
        {
            inDialogue = true;
        }
        else
        {
            inDialogue = false;
        }

        if (leftHand.smoking || rightHand.smoking)
            smoking = true;
        else
            smoking = false;
    }



    public void AddInteractable(GameObject obj)
    {
        if (!pickUpObjects.Contains(obj))
        {
            pickUpObjects.Add(obj);
        }
    }

    public void RemoveInteractable(GameObject obj)
    {
        if (pickUpObjects.Contains(obj))
        {
            obj.GetComponent<PickUpObject>().selected = false;
            pickUpObjects.Remove(obj);

        }
    }

    public bool CheckInteractable(GameObject obj)
    {
        if (pickUpObjects.Count == 0)
            return false;
        if (pickUpObjects.Contains(obj))
        {
            return true;
        }
        return false;
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
                selectedObj.GetComponent<PickUpObject>().selected = false;
            selectedObj = pickUpObjects[0];
            selectedObj.GetComponent<PickUpObject>().selected = true;
        }
        else
        {
            float minDist = Vector3.Distance(pickUpObjects[0].transform.position, transform.position);
            selectedObj = pickUpObjects[0];
            foreach (GameObject obj in pickUpObjects)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < minDist)
                {
                    minDist = distance;
                    if (selectedObj != this)
                    {
                        selectedObj.GetComponent<PickUpObject>().selected = false;
                    }
                    selectedObj = obj;
                }
            }
            selectedObj.GetComponent<PickUpObject>().selected = true;
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

    public bool GetLeftHandSmoking()
    {
        if (leftHand.isHolding)
            return false;
        if (leftHand.smoking)
            return false;
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

    public bool GetRightHandSmoking()
    {
        if (rightHand.isHolding)
            return false;
        if (rightHand.smoking)
            return false;
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
        if (obj.GetComponent<PickUpObject>().cigarette)
        {
            obj.localEulerAngles = new Vector3(0, 160, 0);
            leftHand.smoking = true;
        }
        if (obj.GetComponent<PickUpObject>().freezeRotation)
        {
            obj.localRotation = Quaternion.Euler(0, 0, 0);
        }
        Invoke(nameof(EnableThrowLeft), 0.5f);
    }

    public void OccupyRight(Transform obj)
    {
        rightHand.isHolding = true;
        rightHand.holdingObj = obj;
        obj.SetParent(Camera.main.transform);
        obj.localPosition = rightHand.holdingPosition;
        if (obj.GetComponent<PickUpObject>().cigarette)
        {
            obj.localEulerAngles = new Vector3(0, 160, 0);
            rightHand.smoking = true;
        }
        if (obj.GetComponent<PickUpObject>().freezeRotation)
        {
            obj.localRotation = Quaternion.Euler(0, 0, 0);
        }
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
