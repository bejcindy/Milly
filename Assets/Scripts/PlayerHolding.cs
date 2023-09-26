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
    public bool atContainer;
    public bool positionFixedWithMouse;

    List<GameObject> pickUpObjects;
    public List<GameObject> lookingObjects;
    public GameObject[] containers;
    public GameObject selectedObj;
    public GameObject focusedObj;
    PlayerLeftHand leftHand;
    PlayerRightHand rightHand;

    public GameObject leftHandUI;
    public GameObject rightHandUI;
    public GameObject duoHandUI;
    public GameObject focusedHint;

    public ContainerObject currentContainer;


    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        rightHand = GetComponent<PlayerRightHand>();
        pickUpObjects = new List<GameObject>();
        lookingObjects = new List<GameObject>();
        focusedHint = GameObject.Find("QTEPanel").transform.GetChild(3).gameObject;
        containers = GameObject.FindGameObjectsWithTag("Container");
    }

    // Update is called once per frame
    void Update()
    {
        GetFullHand();
        ChooseInteractable();
        ChooseLookable();
        atContainer = CheckContainer();
        if (selectedObj != null && !fullHand)
        {
            if (GetLeftHand() && GetRightHand())
            {
                duoHandUI.SetActive(true);
                leftHandUI.SetActive(false);
                rightHandUI.SetActive(false);
            }
            else if (GetRightHand())
            {
                rightHandUI.SetActive(true);
                duoHandUI.SetActive(false);
                leftHandUI.SetActive(false);
            }
            else if (GetLeftHand())
            {
                leftHandUI.SetActive(true);
                duoHandUI.SetActive(false);
                rightHandUI.SetActive(false);
            }

        }
        else
        {
            leftHandUI.SetActive(false);
            rightHandUI.SetActive(false);
            duoHandUI.SetActive(false);
        }

        if(focusedObj != null)
        {
            focusedHint.SetActive(true);
        }
        else
        {
            focusedHint.SetActive(false);
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
            if (selectedObj == obj)
                selectedObj = null; 
        }
    }

    public void AddLookable(GameObject obj)
    {
        if (!lookingObjects.Contains(obj))
        {
            lookingObjects.Add(obj);
        }
    }

    public void RemoveLookable(GameObject obj)
    {
        if (lookingObjects.Contains(obj))
        {
            obj.GetComponent<LookingObject>().selected = false;
            lookingObjects.Remove(obj);
            if (focusedObj == obj)
                focusedObj = null;
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
            if (selectedObj != pickUpObjects[0] && selectedObj != null)
                selectedObj.GetComponent<PickUpObject>().selected = false;
            selectedObj = pickUpObjects[0];
            selectedObj.GetComponent<PickUpObject>().selected = true;
        }
        else
        {
            Vector3 toScreen = Camera.main.transform.InverseTransformPoint(pickUpObjects[0].transform.position).normalized;
            float minDist = Vector3.Dot(toScreen, Vector3.forward);


            foreach (GameObject obj in pickUpObjects)
            {
                Vector3 objToScreen = Camera.main.transform.InverseTransformPoint(obj.transform.position).normalized;
                float distance = Vector3.Dot(objToScreen, Vector3.forward);
                if (distance > minDist)
                {
                    if (selectedObj != null)
                        selectedObj.GetComponent<PickUpObject>().selected = false;
                    minDist = distance;
                    selectedObj = obj;
                }
            }
            selectedObj.GetComponent<PickUpObject>().selected = true;
        }
    }

    public void ChooseLookable()
    {
        if(lookingObjects.Count <= 0)
        {
            focusedObj = null;
        }
        else if (lookingObjects.Count == 1)
        {
            if(focusedObj != lookingObjects[0] && focusedObj !=null)
                focusedObj.GetComponent<LookingObject>().selected = false;
            focusedObj = lookingObjects[0];
            focusedObj.GetComponent<LookingObject>().selected = true;
        }
        else
        {

            Vector3 toScreen = Camera.main.transform.InverseTransformPoint(lookingObjects[0].transform.position).normalized;
            float toScreenDist = Vector3.Dot(toScreen, Vector3.forward);

            foreach (GameObject obj in lookingObjects)
            {
                Vector3 objtoScreen = Camera.main.transform.InverseTransformPoint(obj.transform.position).normalized;
                float distance = Vector3.Dot(objtoScreen, Vector3.forward);
                if (distance > toScreenDist)
                {
                    if(focusedObj!= null)
                        focusedObj.GetComponent<LookingObject>().selected = false;
                    toScreenDist = distance;
                    focusedObj = obj;
                }

            }
            if(focusedObj)
                focusedObj.GetComponent<LookingObject>().selected = true;
        }
    }

    bool CheckContainer()
    {
        foreach(GameObject obj in containers)
        {
            if (obj.GetComponent<ContainerObject>().interactable)
            {
                currentContainer = obj.GetComponent<ContainerObject>();
                return true;
            }
        }
        currentContainer = null;
        return false;
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
        RemoveInteractable(obj.gameObject);
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
        Invoke(nameof(EnableThrowLeft), 0.2f);
    }

    public void OccupyRight(Transform obj)
    {
        RemoveInteractable(obj.gameObject);
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
        Invoke(nameof(EnableThrowRight), 0.2f);
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
