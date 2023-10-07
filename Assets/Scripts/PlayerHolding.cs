using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using System;

public class PlayerHolding : MonoBehaviour
{
    public bool smoking;
    public bool fullHand;
    public bool inDialogue;
    public bool throwing;
    public bool atContainer;
    public bool positionFixedWithMouse;


    public Transform handContainer;
    public Transform chopsticksContainer;
    public bool atInterior;


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

    public Image objectUI;
    public Sprite pickUpSprite, lookingSprite, talkSprite, kickSprite,sitSprite,clickSprite;
    RectTransform objectUIRect;
    public RectTransform CanvasRect;

    //for object tracking ui
    public GameObject doorHandle;
    bool displayedLeftHandUI;
    bool displayedFocusHint;
    bool hintDone;
    bool hintHiden;
    public GameObject kickableObj;
    bool kickHidden;
    public GameObject talkingTo;
    public bool talknHidden;
    public GameObject lidObj;
    bool dragHidden;
    public GameObject sitObj;
    bool sitHidden;
    public GameObject clickableObj;
    bool clickHidden;


    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        rightHand = GetComponent<PlayerRightHand>();
        pickUpObjects = new List<GameObject>();
        lookingObjects = new List<GameObject>();
        focusedHint = GameObject.Find("QTEPanel").transform.GetChild(3).gameObject;
        containers = GameObject.FindGameObjectsWithTag("Container");
        objectUIRect = objectUI.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        GetFullHand();
        if (!StartSequence.noControl)
        {
            ChooseInteractable();
            ChooseLookable();
        }


        #region UI and Hints
        if (lookingObjects.Count <= 0 && pickUpObjects.Count <= 0 && !doorHandle && !talkingTo)
        {
            HideUI();
        }
        atContainer = CheckContainer();
        if (doorHandle)
        {
            //Debug.Log("doorhandeled");
            if (!doorHandle.GetComponentInParent<Door>().doorMoving)
                DisplayUI(doorHandle, pickUpSprite);
            else
                HideUI();
        }
        if (!hintDone)
        {
            if (focusedObj != null)
            {
                if (!displayedFocusHint)
                {
                    //Debug.Log("trying");
                    //focusedHint.SetActive(true);
                    DataHolder.ShowHint(DataHolder.hints.lookHint);
                    //displayedFocusHint = true;
                    hintHiden = false;
                    if (focusedObj.GetComponent<LookingObject>().focusingThis)
                        displayedFocusHint = true;
                }
                else
                {
                    DataHolder.HideHint();
                    hintDone = true;
                }
            }
            else
            {
                //focusedHint.SetActive(false);
                if (!hintHiden)
                {
                    DataHolder.HideHint();
                    hintHiden = true;
                }
                //hintDone = true;
            }
        }
        if(kickableObj)
            UITriggerdByOtherObj(kickableObj, kickSprite, kickHidden);
        if(talkingTo)
            UITriggerdByOtherObj(talkingTo, talkSprite, talknHidden);
        if(lidObj)
            UITriggerdByOtherObj(lidObj, pickUpSprite, dragHidden);
        if(sitObj)
            UITriggerdByOtherObj(sitObj, sitSprite, sitHidden);
        if (clickableObj)
            UITriggerdByOtherObj(clickableObj, clickSprite, clickHidden);
        #endregion

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

    #region Interact & Look Related

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
    #endregion
    #region Object-Tracking UI
    void DisplayUI(GameObject trackingObject,Sprite interactionSprite)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
        //Debug.Log(trackingObject.name + ": position is: " + ViewportPosition);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        objectUIRect.anchoredPosition = WorldObject_ScreenPosition;
        objectUI.sprite = interactionSprite;
        objectUI.SetNativeSize();
        if (interactionSprite == talkSprite)
            objectUIRect.localScale = new Vector3(.15f, .15f, .15f);
        else
            objectUIRect.localScale = new Vector3(.1f, .1f, .1f);
        if (!objectUI.gameObject.activeSelf)
            objectUI.gameObject.SetActive(true);
    }

    void HideUI()
    {
        if (objectUI.gameObject.activeSelf)
        {
            objectUI.gameObject.SetActive(false);
            objectUI.sprite = null;
        }
    }
    void UITriggerdByOtherObj(GameObject obj,Sprite sprite,bool hidden)
    {
        if (obj)
        {
            DisplayUI(obj, sprite);
            hidden = false;
        }
        else
        {
            if (!hidden)
            {
                HideUI();
                hidden = true;
            }
        }
    }
    #endregion
    public void ChooseInteractable()
    {
        if (pickUpObjects.Count <= 0)
        {
            selectedObj = null;
            //HideUI();
        }
        else if (pickUpObjects.Count == 1)
        {
            if (selectedObj != pickUpObjects[0] && selectedObj != null)
            {
                selectedObj.GetComponent<PickUpObject>().selected = false;
                //HideUI();
            }
            selectedObj = pickUpObjects[0];
            selectedObj.GetComponent<PickUpObject>().selected = true;
            DisplayUI(selectedObj, pickUpSprite);
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
                    {
                        selectedObj.GetComponent<PickUpObject>().selected = false;
                        //HideUI();
                    }
                    minDist = distance;
                    selectedObj = obj;
                }
            }
            selectedObj.GetComponent<PickUpObject>().selected = true;
            DisplayUI(selectedObj, pickUpSprite);
        }
    }

    public void ChooseLookable()
    {
        if(lookingObjects.Count <= 0)
        {
            focusedObj = null;
            //HideUI();
        }
        else if (lookingObjects.Count == 1)
        {
            if (focusedObj != lookingObjects[0] && focusedObj != null)
            {
                focusedObj.GetComponent<LookingObject>().selected = false;
                //HideUI();
            }
            focusedObj = lookingObjects[0];
            focusedObj.GetComponent<LookingObject>().selected = true;
            if (!focusedObj.GetComponent<LookingObject>().focusingThis && !DataHolder.camBlendDone && !DataHolder.camBlended)
            {
                DisplayUI(focusedObj, lookingSprite);
            }
            else
            {
                    HideUI();
            }
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
                    if (focusedObj != null)
                    {
                        focusedObj.GetComponent<LookingObject>().selected = false;
                        //HideUI();
                    }
                    toScreenDist = distance;
                    focusedObj = obj;
                }

            }
            if (focusedObj)
            {
                focusedObj.GetComponent<LookingObject>().selected = true;
                if (!focusedObj.GetComponent<LookingObject>().focusingThis && !DataHolder.camBlendDone && !DataHolder.camBlended)
                {
                    DisplayUI(focusedObj, lookingSprite);
                }
                else
                {
                        HideUI();
                }
            }
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

    #region Hands Related
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
        //if (leftHand.isHolding && rightHand.isHolding)
        //    fullHand = true;
        if (leftHand.isHolding)
            fullHand = true;
        else
            fullHand = false;
    }

    public void OccupyLeft(Transform obj)
    {
        RemoveInteractable(obj.gameObject);
        leftHand.isHolding = true;
        leftHand.holdingObj = obj;

        if (!obj.GetComponent<Chopsticks>())
        {
            obj.SetParent(handContainer);
            StartCoroutine(LerpPosition(obj, Vector3.zero, 1f));
        }

        else
        {
            obj.parent.SetParent(chopsticksContainer);
            StartCoroutine(LerpPosition(obj.parent, Vector3.zero, 1f));
        }




        if(!obj.GetComponent<Cigarette>() && !obj.GetComponent<Chopsticks>())
            StartCoroutine(LerpRotation(obj, Quaternion.Euler(Vector3.zero), 1f));
        else if (obj.GetComponent<Chopsticks>())
            StartCoroutine(LerpRotation(obj.parent, Quaternion.Euler(Vector3.zero), 1f));
        if (obj.GetComponent<PickUpObject>().cigarette)
        {
            obj.localEulerAngles = new Vector3(0, 160, 0);
            leftHand.smoking = true;
        }
        if (obj.GetComponent<PickUpObject>().freezeRotation)
        {
            obj.localRotation = Quaternion.Euler(0, 0, 0);
        }

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
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = true;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = false;
    }

    IEnumerator LerpPosition(Transform obj, Vector3 targetPosition, float duration)
    {
        if (leftHand.holdingObj.GetComponent<Chopsticks>())
            leftHand.holdingObj.GetComponent<Chopsticks>().chopMoving = true;

        float time = 0;
        Vector3 startPosition = obj.localPosition;
        while (time < duration)
        {
            obj.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obj.localPosition = targetPosition;

        if (leftHand.holdingObj.GetComponent<Chopsticks>())
            leftHand.holdingObj.GetComponent<Chopsticks>().chopMoving = false;
        Invoke(nameof(EnableThrowLeft), 0.2f);
    }

    IEnumerator LerpRotation(Transform obj, Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = obj.localRotation;
        while (time < duration)
        {
            obj.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obj.localRotation = endValue;
    }


}
