using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using System;
using PixelCrushers;

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
    List<GameObject> kickObjects;
    public GameObject[] containers;
    public GameObject selectedObj;
    public GameObject focusedObj;
    public Transform holdingObject;
    PlayerLeftHand leftHand;

    public ContainerObject currentContainer;
    public bool tableControl;

    PlayerMovement pm;
    float kickForce = 6f;
    public GameObject midAirKickable;
    #region For Object Tracking UI
    public Image objectUI;
    public GameObject dragUIAnimation;
    public String dragAnimDirection;
    public Sprite pickUpSprite, dragSprite, grabingSprite, lookingSprite, talkSprite, kickSprite, sitSprite, clickSprite, catSprite, sweepSprite;
    RectTransform objectUIRect, objectUIRect2;
    public RectTransform CanvasRect;

    //[HideInInspector]
    public GameObject doorHandle, kickableObj, talkingTo, lidObj, sitObj, clickableObj, catboxObj, vinylObj, dirtObj;
    bool noControlReset;
    bool displayedLeftHandUI;
    bool displayedFocusHint;
    bool hintDone;
    bool hintHiden, kickHidden, talknHidden, dragHidden, sitHidden, clickHidden, catHidden, doorHidden, vinylHidden, dirtHidden;
    bool showedSoccerHint,soccerHidden;
    List<GameObject> trackedObjs;
    List<GameObject> UIs;
    List<Sprite> usedSprites;

    Dictionary<Sprite, GameObject> instantiatedIcons = new Dictionary<Sprite, GameObject> ();


    #endregion
    public bool objectLerping;

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
        if (MainTattooMenu.tatMenuOn)
        {
            pickUpObjects.Clear();
            lookingObjects.Clear();
            doorHandle = null;
            kickableObj = null;
            talkingTo = null; lidObj = null;
            sitObj = null;
            clickableObj = null;
            catboxObj = null;
            selectedObj = null;
            focusedObj = null;
        }

        holdingObject = leftHand.holdingObj;


        //BASIC HAND AND LOOK SELECTION
        if (!leftHand.gettingCig)
            ChooseInteractable();
        else if (leftHand.gettingCig)
        {
            if (selectedObj)
                selectedObj.GetComponent<PickUpObject>().selected = false;
            selectedObj = null;
            pickUpObjects.Clear();
        }

        if (!inDialogue)
            ChooseLookable();

        if(!selectedObj && !doorHandle && !lidObj)
        {
            HideUI(pickUpSprite);
            HideUI(dragSprite);
        }

        if (pickUpObjects.Count >= 1 || midAirKickable)
        {
            DetectKick();
            if (TrashSoccerScoreBoard.startedSoccerGame && !showedSoccerHint)
            {
                DataHolder.ShowHint(DataHolder.hints.soccerHint);
                showedSoccerHint = true;
                soccerHidden = false;
            }
        }
        else if (!soccerHidden)
        {
            DataHolder.HideHint(DataHolder.hints.soccerHint);
            soccerHidden = true;
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

        //LOOKING HINT
        if (focusedObj != null)
        {
            if (!displayedFocusHint)
            {
                DataHolder.ShowHint(DataHolder.hints.lookHint);
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


        ObjectUICheck();


        if (leftHand.smoking)
            smoking = true;
        else
            smoking = false;
        
        if (DialogueManager.IsConversationActive)
        {
            inDialogue = true;
        }
        else
        {
            inDialogue = false;
        }
    }

    void ObjectUICheck()
    {
        //DOOR HANDLE UI
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

        if (kickableObj)
            DisplayUI(kickableObj, kickSprite);
        else
            HideUI(kickSprite);


        if (talkingTo)
            DisplayUI(talkingTo, talkSprite);
        else
            HideUI(talkSprite);
        if (lidObj)
        {
            dragHidden = false;
            GrabDragSpriteSwap(lidObj);
        }

        else
        {
            if (!selectedObj && !doorHandle)
            {
                HideUI(grabingSprite);
                HideUI(dragSprite);
            }
            else if (!doorHandle)
            {
                if (!dragHidden)
                {
                    dragHidden = true;
                    HideUI(dragSprite);
                }

            }
        }

        if (sitObj)
        {
            DisplayUI(sitObj, sitSprite);
        }
        else
        {
            HideUI(sitSprite);
        }
        if (clickableObj)
        {
            DisplayUI(clickableObj, clickSprite);
        }
        else
        {
            HideUI(clickSprite);
        }
        if (catboxObj)
        {
            DisplayUI(catboxObj, catSprite);
        }
        else
        {
            HideUI(catSprite);
        }
        if (vinylObj)
        {
            DisplayUI(vinylObj, pickUpSprite);
        }
        else
        {
            if (!selectedObj && !doorHandle && !lidObj)
            {
                HideUI(pickUpSprite);
            }
        }
        if (dirtObj)
        {
            DisplayUI(dirtObj, sweepSprite);
        }
        else
        {
            HideUI(sweepSprite);
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

    public bool LidUsable(Lid lid)
    {
        if(lidObj == null && selectedObj == null)
        {
            return true;
        } 
        else if(lidObj == lid.transform.GetChild(0).gameObject && selectedObj == null) {

            return true;
        }
        return false;
    }

    #endregion

    #region Object-Tracking UI
    void DisplayUI(GameObject trackingObject, Sprite interactionSprite)
    {

        if(!instantiatedIcons.TryGetValue(interactionSprite, out _))
        {
            Vector2 ViewportPosition;
            ViewportPosition = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            GameObject instantiatedUI = Instantiate(objectUI.gameObject, CanvasRect.transform);

            if (interactionSprite == grabingSprite)
                InstantiateDragUIAnimation(instantiatedUI.transform);
            instantiatedUI.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
            if (interactionSprite == talkSprite || interactionSprite == sweepSprite)
                instantiatedUI.GetComponent<RectTransform>().localScale = new Vector3(.15f, .15f, .15f);
            else
                instantiatedUI.GetComponent<RectTransform>().localScale = new Vector3(.1f, .1f, .1f);

            instantiatedUI.SetActive(true);
            instantiatedUI.GetComponent<TrackObject>().sprite = interactionSprite;
            instantiatedUI.GetComponent<TrackObject>().trackThis = trackingObject;

            instantiatedIcons.Add(interactionSprite, instantiatedUI);
        }
        else
        {
            instantiatedIcons[interactionSprite].GetComponent<TrackObject>().trackThis = trackingObject;
        }
        
    }

    void HideUI(Sprite toHide)
    {
        if (!toHide)
        {
            if (UIs.Count != 0)
            {
                instantiatedIcons.Clear();
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
            if(instantiatedIcons.TryGetValue(toHide, out _))
            {
                Debug.Log("Something is hidden");
                GameObject target = instantiatedIcons[toHide];
                instantiatedIcons.Remove(toHide);
                usedSprites.Remove(target.GetComponent<TrackObject>().sprite);
                trackedObjs.Remove(target.GetComponent<TrackObject>().trackThis);
                if (toHide == grabingSprite)
                    dragAnimDirection = "";
                UIs.Remove(target);
                Destroy(target);

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


    void GrabDragSpriteSwap(GameObject obj)
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
    void UITriggerdByOtherObj(GameObject obj, Sprite sprite, bool hidden)
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
                Debug.Log("setting hidden to true");
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
        if (lookingObjects.Count <= 0)
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





    public void OccupyLeft(Transform obj)
    {
        RemoveInteractable(obj.gameObject);
        leftHand.isHolding = true;
        leftHand.holdingObj = obj;
        PickUpObject pickUp = obj.GetComponent<PickUpObject>();
        leftHand.AssignRefs(pickUp);

        if (selectedObj != null)
        {
            selectedObj.GetComponent<PickUpObject>().selected = false;
            selectedObj = null;
        }
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
                StartCoroutine(LerpRotation(obj, Quaternion.Euler(new Vector3(0, 160, 0)), 1f));
                //obj.localEulerAngles = new Vector3(0, 160, 0);
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

    void DetectKick()
    {
        //DataHolder.ShowHint(DataHolder.hints.kickHint);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (GameObject obj in pickUpObjects)
            {
                if (obj.GetComponent<Rigidbody>())
                {
                    Rigidbody rigid = obj.GetComponent<Rigidbody>();
                    Vector3 kickDir = Vector3.ProjectOnPlane(obj.transform.position - transform.position, Vector3.up);
                    rigid.AddForce(kickDir * kickForce + Vector3.up * 2f, ForceMode.Impulse);
                    obj.GetComponent<PickUpObject>().kicked = true;
                }
            }
            if (midAirKickable)
            {
                if (midAirKickable.GetComponent<Rigidbody>())
                {
                    Rigidbody rigid = midAirKickable.GetComponent<Rigidbody>();
                    Vector3 kickDir = Vector3.ProjectOnPlane(midAirKickable.transform.position - transform.position, Vector3.up);
                    rigid.AddForce(kickDir * kickForce + Vector3.up * 2f, ForceMode.Impulse);
                    midAirKickable.GetComponent<PickUpObject>().kicked = true;
                    midAirKickable = null;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = true;

        if (other.CompareTag("DoorDetector"))
        {
            if (leftHand.isHolding)
                leftHand.enabled = false;
        }

        if (other.CompareTag("Container"))
        {
            atContainer = true;
            currentContainer = other.GetComponent<ContainerObject>();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interior"))
            atInterior = false;

        if (other.CompareTag("DoorDetector"))
            leftHand.enabled = true;

        if (other.CompareTag("Container"))
        {
            atContainer = false;
            currentContainer = null;
        }
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
        objectLerping = true;
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
        objectLerping = false;
        Invoke(nameof(EnableThrowLeft), 0.1f);
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
