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
    public bool looking;
    public bool atContainer;
    public bool atTable;

    public bool positionFixedWithMouse;


    public Transform handContainer;
    public Transform chopsticksContainer;
    public Transform doubleHandContainer;
    public bool atInterior;


    public List<GameObject> pickUpObjects;
    public List<GameObject> lookingObjects;
    public GameObject[] containers;
    public GameObject selectedObj;
    public GameObject focusedObj;
    public Transform holdingObject;
    PlayerLeftHand leftHand;

    public ContainerObject currentContainer;
    public bool tableControl;

    PlayerMovement pm;

    #region For Object Tracking UI
    public Image objectUI;
    public GameObject dragUIAnimation;
    public String dragAnimDirection;
    public Sprite pickUpSprite, dragSprite, grabingSprite, lookingSprite, talkSprite, kickSprite, sitSprite, clickSprite, catSprite;
    RectTransform objectUIRect,objectUIRect2;
    public RectTransform CanvasRect;

    //[HideInInspector]
    public GameObject doorHandle, kickableObj, talkingTo, lidObj, sitObj, clickableObj,catboxObj;
    bool noControlReset;
    bool displayedLeftHandUI;
    bool displayedFocusHint;
    bool hintDone;
    bool hintHiden, kickHidden, talknHidden, dragHidden, sitHidden, clickHidden,catHidden,doorHidden;
    List<GameObject> trackedObjs;
    List<GameObject> UIs;
    List<Sprite> usedSprites;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        pm = GetComponent<PlayerMovement>();
        pickUpObjects = new List<GameObject>();
        lookingObjects = new List<GameObject>();
        containers = GameObject.FindGameObjectsWithTag("Container");
        objectUIRect = objectUI.GetComponent<RectTransform>();
        trackedObjs = new List<GameObject>();
        UIs = new List<GameObject>();
        usedSprites = new List<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        holdingObject = leftHand.holdingObj;
        if (!looking)
        {
            GetFullHand();
            if (!leftHand.inPizzaBox)
                ChooseInteractable();

                if (selectedObj)
                    DisplayUI(selectedObj, pickUpSprite);
                else
                    HideUI(pickUpSprite);


            if (!inDialogue)
                ChooseLookable();


            #region UI and Hints
            if (lookingObjects.Count <= 0 && pickUpObjects.Count <= 0 && !doorHandle && !talkingTo && !kickableObj && !lidObj && !sitObj && !catboxObj)
            {
                HideUI(null);
            }
            if (atTable)
            {
                if (!tableControl)
                    FakeHideUI();
                else
                    FakeDisplayUI();
            }
            else
                FakeDisplayUI();

            atContainer = CheckContainer();

            if (focusedObj != null)
            {
                if (!displayedFocusHint)
                {
                    //Debug.Log("trying");
                    DataHolder.ShowHint(DataHolder.hints.lookHint);
                    //displayedFocusHint = true;
                    hintHiden = false;
                    if (focusedObj.GetComponent<LookingObject>().focusingThis)
                        displayedFocusHint = true;
                }
                else
                {
                    DataHolder.HideHint(DataHolder.hints.lookHint);
                }
            }
            else
            {
                if (!hintHiden)
                {
                    DataHolder.HideHint(DataHolder.hints.lookHint);
                    hintHiden = true;
                    displayedFocusHint = false;
                }
            }

            if (doorHandle)
            {

                if (Input.GetMouseButton(0))
                {
                    DisplayUI(doorHandle, grabingSprite);
                    HideUI(dragSprite);
                }
                else if (!doorHandle.GetComponentInParent<Door>().doorMoving)
                {
                    DisplayUI(doorHandle, dragSprite);
                    HideUI(grabingSprite);
                }
                else
                {
                    HideUI(dragSprite);
                    HideUI(grabingSprite);
                }

            }
            //if(kickableObj)
            UITriggerdByOtherObj(kickableObj, kickSprite, kickHidden);
            //if(talkingTo)
            UITriggerdByOtherObj(talkingTo, talkSprite, talknHidden);
            if (lidObj)
                UITriggerdByOtherObj(lidObj, dragSprite, dragHidden);
            //if(sitObj)
            UITriggerdByOtherObj(sitObj, sitSprite, sitHidden);
            //if (clickableObj)
            UITriggerdByOtherObj(clickableObj, clickSprite, clickHidden);
            //if(catboxObj)
            UITriggerdByOtherObj(catboxObj, catSprite, catHidden);
            #endregion

            

            if (leftHand.smoking)
                smoking = true;
            else
                smoking = false;
        }
        if (DialogueManager.IsConversationActive)
        {
            inDialogue = true;
        }
        else
        {
            inDialogue = false;
        }



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
        //check if current object is tracked already
        bool instantiated = false;
        if (!instantiated)
        {
            if (trackedObjs.Count == 0 || !trackedObjs.Contains(trackingObject))
            {
                if (usedSprites.Count == 0 || !usedSprites.Contains(interactionSprite))
                {
                    PizzaLid pizzaLid = trackingObject.GetComponent<PizzaLid>();
                    Vector2 ViewportPosition;
                    if (pizzaLid == null)
                    {
                        ViewportPosition = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
                    }
                    else
                    {
                        ViewportPosition = Camera.main.WorldToViewportPoint(pizzaLid.uiHint.transform.position);
                    }

                    Vector2 WorldObject_ScreenPosition = new Vector2(
                    ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                    ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
                    GameObject instantiatedUI = Instantiate(objectUI.gameObject, CanvasRect.transform);
                    if (interactionSprite == grabingSprite)
                        InstantiateDragUIAnimation(instantiatedUI.transform);
                    instantiatedUI.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
                    if (interactionSprite == talkSprite)
                        instantiatedUI.GetComponent<RectTransform>().localScale = new Vector3(.15f, .15f, .15f);
                    else
                        instantiatedUI.GetComponent<RectTransform>().localScale = new Vector3(.1f, .1f, .1f);
                    instantiatedUI.SetActive(true);
                    instantiatedUI.GetComponent<TrackObject>().sprite = interactionSprite;
                    instantiatedUI.GetComponent<TrackObject>().trackThis = trackingObject;
                    UIs.Add(instantiatedUI);
                    trackedObjs.Add(trackingObject);
                    usedSprites.Add(interactionSprite);
                    instantiated = true;
                }
                else
                {
                    for (int i = 0; i < UIs.Count; i++)
                    {
                        if (UIs[i].GetComponent<TrackObject>().sprite == interactionSprite)
                        {
                            trackedObjs.Remove(UIs[i].GetComponent<TrackObject>().trackThis);
                            UIs[i].GetComponent<TrackObject>().trackThis = trackingObject;
                            trackedObjs.Add(trackingObject);
                            instantiated = true;
                        }
                    }
                }
            }
        }
    }

    void HideUI(Sprite toHide)
    {
        if (!toHide)
        {
            if (UIs.Count != 0)
            {
                for (int i = 0; i < UIs.Count; i++)
                {
                    Destroy(UIs[i]);
                }
                usedSprites.Clear();
                trackedObjs.Clear();
                UIs.Clear();
                dragAnimDirection = "";
            }
        }
        else
        {
            for (int i = 0; i < UIs.Count; i++)
            {
                if (UIs[i].GetComponent<TrackObject>().sprite == toHide)
                {
                    Destroy(UIs[i]);
                    usedSprites.Remove(UIs[i].GetComponent<TrackObject>().sprite);
                    trackedObjs.Remove(UIs[i].GetComponent<TrackObject>().trackThis);
                    if (toHide == grabingSprite)
                        dragAnimDirection = "";
                    UIs.Remove(UIs[i]);
                }
            }
        }
    }
    void FakeHideUI()
    {
        for (int i = 0; i < UIs.Count; i++)
        {
            Image img = UIs[i].GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }
    }
    void FakeDisplayUI()
    {
        for (int i = 0; i < UIs.Count; i++)
        {
            Image img = UIs[i].GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        }
    }
    void UITriggerdByOtherObj(GameObject obj,Sprite sprite,bool hidden)
    {
        if (obj)
        {
            if (obj == lidObj)
            {
                if (Input.GetMouseButton(0))
                {
                    DisplayUI(obj, grabingSprite);
                    HideUI(dragSprite);
                }
                else
                {
                    DisplayUI(obj, dragSprite);
                    HideUI(grabingSprite);
                }
                
            }
            else
                DisplayUI(obj, sprite);
            hidden = false;
        }
        else
        {
            if (!hidden)
            {
                HideUI(sprite);
                hidden = true;
            }
        }
    }
    void InstantiateDragUIAnimation(Transform parent)
    {
        GameObject obj = Instantiate(dragUIAnimation, parent);
        Animator dragAnim = obj.GetComponent<Animator>();
        switch (dragAnimDirection)
        {
            case "Left":
                dragAnim.SetBool("Left", true);
                dragAnim.SetBool("Right", false);
                break;
            case "Right":
                dragAnim.SetBool("Left", false);
                dragAnim.SetBool("Right", true);
                break;
            case "LeftRight":
                dragAnim.SetBool("Left", true);
                dragAnim.SetBool("Right", true);
                break;
            case "Up":
                dragAnim.SetBool("Up", true);
                dragAnim.SetBool("Down", false);
                break;
            case "Down":
                dragAnim.SetBool("Up", false);
                dragAnim.SetBool("Down", true);
                break;
            case "UpDown":
                dragAnim.SetBool("Up", true);
                dragAnim.SetBool("Down", true);
                break;
        }
        obj.SetActive(true);
    }
    #endregion
    public void ChooseInteractable()
    {
        if (pickUpObjects.Count <= 0)
        {
            selectedObj = null;
            if (!doorHandle)
                HideUI(pickUpSprite);
        }
        else if (pickUpObjects.Count == 1)
        {
            if (selectedObj != pickUpObjects[0] && selectedObj != null)
            {
                selectedObj.GetComponent<PickUpObject>().selected = false;
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
                    }
                    minDist = distance;
                    selectedObj = obj;
                }
            }
            if (selectedObj)
            {
                selectedObj.GetComponent<PickUpObject>().selected = true;
                DisplayUI(selectedObj, pickUpSprite);
            }

        }
    }

    public void ChooseLookable()
    {
        if (focusedObj == null)
            HideUI(lookingSprite);
        if(lookingObjects.Count <= 0)
        {
            focusedObj = null;
            HideUI(lookingSprite);
        }
        else if (lookingObjects.Count == 1)
        {
            if (focusedObj != lookingObjects[0] && focusedObj != null)
            {
                focusedObj.GetComponent<LookingObject>().selected = false;
            }
            focusedObj = lookingObjects[0];
            focusedObj.GetComponent<LookingObject>().selected = true;
            if (!focusedObj.GetComponent<LookingObject>().focusingThis && !DataHolder.camBlendDone && !DataHolder.camBlended)
            {
                DisplayUI(focusedObj, lookingSprite);
            }
            else
            {
                HideUI(lookingSprite);
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
                        HideUI(lookingSprite);
                }
            }
        }
    }

    public void HideLookingHint()
    {
        HideUI(lookingSprite);
        DataHolder.HideHint(DataHolder.hints.lookHint);
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



    public void GetFullHand()
    {
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
        PickUpObject pickUp = obj.GetComponent<PickUpObject>();
        switch (pickUp.objType)
        {
            case HandObjectType.CHOPSTICKS:
                obj.parent.SetParent(chopsticksContainer);
                StartCoroutine(LerpPosition(obj.parent, Vector3.zero, 1f));
                StartCoroutine(LerpRotation(obj.parent, Quaternion.Euler(pickUp.targetRot), 1f));
                break;
            case HandObjectType.DOUBLE:
                obj.SetParent(doubleHandContainer);
                StartCoroutine(LerpPosition(obj, Vector3.zero, 1f));
                Quaternion objRot = Quaternion.Euler(new Vector3(0, -90, 0));
                StartCoroutine(LerpRotation(obj, objRot, 1f));
                break;
            case HandObjectType.CIGARETTE:
                obj.SetParent(handContainer);
                obj.localEulerAngles = new Vector3(0, 160, 0);
                StartCoroutine(LerpPosition(obj, Vector3.zero, 1f));
                leftHand.smoking = true;
                break;
            default:
                obj.SetParent(handContainer);
                StartCoroutine(LerpPosition(obj, Vector3.zero, 1f));
                StartCoroutine(LerpRotation(obj, Quaternion.Euler(pickUp.targetRot), 1f));
                break;

        }


        if (obj.GetComponent<PickUpObject>().freezeRotation)
        {
            obj.localRotation = Quaternion.Euler(0, 0, 0);
        }

        pickUpObjects.Clear();
    }



    public void UnoccupyLeft()
    {
        leftHand.isHolding = false;
        leftHand.holdingObj = null;
        leftHand.noThrow = true;
    }



    public void EnableThrowLeft()
    {
        leftHand.noThrow = false;
    }


    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = true;

        if (other.CompareTag("DoorDetector"))
        {
            if (leftHand.isHolding)
                leftHand.enabled = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = false;

        if (other.CompareTag("DoorDetector"))
            leftHand.enabled = true;
    }
    
    public void ClearPickUp()
    {
        pickUpObjects.Clear();
        HideUI(null);
    }

    public void SetAtTableFalse()
    {
        atTable = false;
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
