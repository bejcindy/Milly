using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using VInspector;

public class PlayerLeftHand : MonoBehaviour
{
    float xOffset = -50;
    PlayerHolding playerHolding;
    LayerMask playerLayer;

    [Foldout("Hand State")]
    public bool isHolding;
    public bool inPizzaBox;
    public bool smoking;
    public bool inhaling;
    public bool drinking;

    [Foldout("Throwing")]
    public bool bypassThrow;
    public bool noThrow;
    public bool readyToThrow;
    public float throwForce;
    public float throwForceUp;
    public float maxThrowForce;

    [Foldout("Holding Pos")]
    public Vector3 holdingPosition;
    public Vector3 smokingPosition;

    [Foldout("Holding Object")]
    public Transform holdingObj;


    [Foldout("References")]
    public GameObject aimUI;
    public GameObject chopAimUI;
    public GameObject aimHint;
    public Animator handAnim;
    public ParticleSystem smokeVFX;
    public PizzaBox pizzaBox;
    public GameObject eatingDialogue;

    [Foldout("Chopsticks")]
    public float chopHoldVal = 0;
    public bool chopAiming;
    public static int foodAte;
    public Transform selectedFood;
    Chopsticks currentChop;
    bool chopOut;
    float chopHoldTimeMax = 1f;
    bool foodEatingDialogueDone;
    Transform currentFood;



    PickUpObject objPickUp;
    Rigidbody objRb;
    bool notHoldingAnyThing;


    #region UI variables
    bool aimHinted, smokingHinted, drinkHinted;
    public bool aimHintDone, smokingHintDone, drinkHintDone;
    bool chophinted;
    bool canSmoke;
    public bool aiming;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = 8;
        noThrow = true;
        playerHolding = GetComponent<PlayerHolding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            if (!MainTattooMenu.tatMenuOn)
            {
                if (!inPizzaBox)
                {
                    HoldingAction();
                }
                else if (inPizzaBox && holdingObj.GetComponent<Pizza>())
                {
                    DetectPizzaHolding();
                }
                else if (inPizzaBox && !holdingObj.GetComponent<Pizza>())
                {
                    readyToThrow = false;
                    holdingObj.localPosition = Vector3.zero;
                    aimUI.SetActive(false);
                    aimUI.transform.localScale = new Vector3(1, 1, 1);
                }
            }


        }
        else
        {
            inhaling = false;
            drinking = false;
            smoking = false;
            currentChop = null;
        }


        if (foodAte >= 3 && !foodEatingDialogueDone)
        {
            eatingDialogue.SetActive(true);
            foodEatingDialogueDone = true;
        }



        #region Hint Region
        if (smoking)
        {
            DataHolder.ShowHint(DataHolder.hints.smokeHint);
            smokingHintDone = false;
        }
        else if (isHolding && holdingObj.GetComponent<Pizza>() && inPizzaBox)
        {
            DataHolder.ShowHint(DataHolder.hints.pizzaHint);
        }
        else if (isHolding && holdingObj.GetComponent<GroceryBox>() && GroceryBoxGame.hasPlaceableBox)
        {
            DataHolder.ShowHint(DataHolder.hints.pizzaHint);
            DataHolder.HideHint(DataHolder.hints.throwHint);
        }
        else if (currentChop && !chopAiming)
        {
            //DataHolder.HideHint(DataHolder.hints.pickFoodHint);
            DataHolder.ShowHint(DataHolder.hints.eatHint);
            DataHolder.HideHint(DataHolder.hints.chopHint);
            chophinted = true;
        }
        else if (currentChop && chopAiming)
        {
            //关其他的
            DataHolder.ShowHint(DataHolder.hints.chopHint);
            DataHolder.HideHint(DataHolder.hints.eatHint);
            //DataHolder.ShowHint(DataHolder.hints.pickFoodHint);
            chophinted = true;
        }
        else if (currentChop && currentChop.hasFood)
        {
            DataHolder.HideHint(DataHolder.hints.pickFoodHint);
            DataHolder.HideHint(DataHolder.hints.chopHint);
            DataHolder.ShowHint(DataHolder.hints.eatHint);
            chophinted = true;
        }
        else if (!currentChop && chophinted)
        {
            DataHolder.HideHint(DataHolder.hints.chopHint);
            DataHolder.HideHint(DataHolder.hints.pickFoodHint);
            DataHolder.HideHint(DataHolder.hints.eatHint);
            chophinted = false;
        }
        else if (holdingObj && holdingObj.GetComponent<ChiliPowder>())
        {
            DataHolder.ShowHint(DataHolder.hints.powderHint);
        }
        else if (holdingObj && holdingObj.GetComponent<PickUpObject>().objType == HandObjectType.DRINK)
        {
            if (playerHolding.atTable)
                DataHolder.ShowHint(DataHolder.hints.tableDrinkHint);
            else
            {
                DataHolder.ShowHint(DataHolder.hints.drinkAndThrowHint);
            }
            drinkHintDone = false;
        }
        else if (isHolding)
        {
            DataHolder.ShowHint(DataHolder.hints.throwHint);
            DataHolder.HideHint(DataHolder.hints.pizzaHint);
            aimHintDone = false;
        }
        else if (canSmoke && !smokingHinted)
        {
            DataHolder.ShowHint(DataHolder.hints.cigHint);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DataHolder.HideHint(DataHolder.hints.cigHint);
                smokingHinted = true;
            }
        }
        else if (!holdingObj)
        {
            DataHolder.HideHint(DataHolder.hints.powderHint);
            DataHolder.HideHint(DataHolder.hints.drinkHint);
            DataHolder.HideHint(DataHolder.hints.throwHint);
            DataHolder.HideHint(DataHolder.hints.smokeHint);
            DataHolder.HideHint(DataHolder.hints.tableDrinkHint);
            DataHolder.HideHint(DataHolder.hints.drinkAndThrowHint);
            DataHolder.HideHint(DataHolder.hints.pizzaHint);
            drinkHintDone = true;
        }
        #endregion
    }

    public void AssignRefs(PickUpObject obj)
    {
        objPickUp = obj;
        objRb = obj.GetComponent<Rigidbody>();
    }

    public void RemoveHandObj()
    {
        isHolding = false;
        holdingObj = null;
        noThrow = true;
        objPickUp = null;
        objRb = null;
    }

    public void HoldingAction()
    {
        switch (objPickUp.objType)
        {
            case HandObjectType.DRINK:
                Drink();
                if (!drinking && !playerHolding.atInterior)
                    BasicThrow();
                break;
            case HandObjectType.CHOPSTICKS:
                currentChop = holdingObj.GetComponent<Chopsticks>();
                UsingChopsticks();
                ChopUIDetect();
                break;
            case HandObjectType.CIGARETTE:
                Smoke();
                if (!playerHolding.atInterior)
                    BasicThrow();
                if (!smokingHinted)
                {
                    DataHolder.HideHint(DataHolder.hints.cigHint);
                    smokingHinted = true;
                }
                break;
            case HandObjectType.DOUBLE:
                DetectDoubleHand();
                break;

            default:
                if (!drinking && !playerHolding.atInterior)
                    BasicThrow();
                break;
        }
    }


    #region Chopsticks Region
    private void UsingChopsticks()
    {
        if (!currentChop.hasFood && !currentChop.chopMoving)
        {
            chopAiming = true;
        }
        else
        {
            chopAiming = false;
        }

        if (Input.GetMouseButtonDown(0) && chopAiming)
        {
            if (selectedFood)
            {
                FoodPickObject food = selectedFood.GetComponent<FoodPickObject>();
                selectedFood.SetParent(holdingObj.parent.transform);
                food.picked = true;
                selectedFood.localPosition = currentChop.foodPickedPos;
                selectedFood.localRotation = Quaternion.Euler(food.inChopRot);
                currentFood = selectedFood;
                selectedFood.GetComponent<FoodPickObject>().selected = false;
                currentChop.hasFood = true;
            }
        }
        if (currentChop.hasFood && !currentChop.chopMoving)
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                Eat();
            }
        }


    }

    public void Eat()
    {
        currentChop.chopMoving = true;
        currentChop.SetEatAnim();
        Invoke(nameof(DestroyFood), 1f);
        Invoke(nameof(ResetChopsticks), 2f);
    }

    private void ChopUIDetect()
    {
        if (chopAiming)
            chopAimUI.SetActive(true);
        else
            chopAimUI.SetActive(false);
    }

    void ResetChopsticks()
    {
        currentChop.chopMoving = false;
        currentChop.hasFood = false;
    }

    public void DestroyFood()
    {
        if (!playerHolding.inDialogue && (foodAte != 2))
            currentFood.GetComponent<FoodPickObject>().CheckFirstAte();
        Transform toEat;
        toEat = currentFood;
        currentFood = null;
        Destroy(toEat.gameObject);
        toEat = null;

        currentChop.PlayEatSound();
        foodAte++;

    }


    public Chopsticks GetCurrentChops()
    {
        if (currentChop && !currentChop.hasFood)
            return currentChop;
        else
            return null;
    }

    public void RemoveCurrentChops()
    {
        currentChop = null;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CigHint" && !smokingHinted)
        {
            canSmoke = true;
            other.gameObject.SetActive(false);
        }
    }



    private void Smoke()
    {
        if (Input.mouseScrollDelta.y < 0 && holdingObj.localPosition.z > -0.1f && !inhaling)
        {
            holdingObj.GetComponent<Cigarette>().PlayInhaleSound();
            Vector3 smokingPos = new Vector3(0.5f, 0f, -0.3f);
            Vector3 smokingRot = new Vector3(0, 180, 0);
            StartCoroutine(LerpPosition(smokingPos, 1f));
            StartCoroutine(LerpRotation(Quaternion.Euler(smokingRot), 1f));
        }
        if (Input.mouseScrollDelta.y > 0 && holdingObj.localPosition.z < 0f && !inhaling)
        {
            holdingObj.GetComponent<Cigarette>().PlayExhaleSound();
            if (!smokeVFX.gameObject.activeSelf)
            {
                smokeVFX.gameObject.SetActive(true);
            }
            else
            {
                smokeVFX.Play();
            }
            Vector3 smokingRot = new Vector3(0, 160, 0);
            StartCoroutine(LerpPosition(Vector3.zero, 1f));
            StartCoroutine(LerpRotation(Quaternion.Euler(smokingRot), 1f));
            holdingObj.GetComponent<Cigarette>().Inhale();
        }
    }

    private void Drink()
    {
        if (Input.GetMouseButtonDown(0) && !noThrow)
        {
            if (playerHolding.atContainer && playerHolding.currentContainer.CheckMatchingObject(holdingObj.gameObject))
            {
                isHolding = false;
                StartCoroutine(playerHolding.currentContainer.MoveAcceptedObject(holdingObj, 1f));
            }
        }

        if (Input.mouseScrollDelta.y < 0 && !drinking)
        {
            drinking = true;
            handAnim.Play("HandDrink");
            BeerCup beer = holdingObj.GetComponent<BeerCup>();
            if (beer)
            {
                beer.ReduceLiquid();
            }
            //drinkHinted = true;
            if (!drinkHintDone)
            {
                DataHolder.HideHint(DataHolder.hints.drinkHint);
                Invoke(nameof(DrinkSound), 1f);
                drinkHintDone = true;
            }
        }
        if (handAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HandDrink")
            drinking = false;
    }

    void DrinkSound()
    {
        RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Swallow", transform.position);
    }

    float rotateVal;
    private void BasicThrow()
    {
        if (!bypassThrow && !playerHolding.inDialogue)
        {
            if (Input.GetMouseButtonDown(0) && holdingObj && !PauseMenu.isPaused)
            {
                readyToThrow = true;
            }

            if(readyToThrow && !noThrow)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    aiming = false;
                    aimUI.SetActive(false);
                    aimUI.transform.localScale = new Vector3(1, 1, 1);

                    if (smoking)
                    {
                        holdingObj.GetComponent<Cigarette>().FinishSmoking();
                        if (!smokingHintDone)
                        {
                            DataHolder.HideHint(DataHolder.hints.smokeHint);
                            smokingHintDone = true;
                        }
                    }

                    isHolding = false;
                    smoking = false;
                    objRb.isKinematic = false;
                    objPickUp.inHand = false;
                    objPickUp.thrown = true;
                    objPickUp.thrownByPlayer = true;
                    holdingObj.SetParent(null);




                    if (playerHolding.atContainer && playerHolding.currentContainer.CheckMatchingObject(holdingObj.gameObject))
                    {
                        isHolding = false;
                        StartCoroutine(playerHolding.currentContainer.MoveAcceptedObject(holdingObj, 1f));
                    }
                    else
                    {
                        RaycastHit hit;
                        Vector3 forceDirection = Camera.main.transform.forward;
                        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f, playerLayer, QueryTriggerInteraction.Ignore))
                        {
                            forceDirection = (hit.point - holdingObj.localPosition).normalized;
                        }

                        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwForceUp;
                        holdingObj.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceMode.Impulse);
                        FMODUnity.RuntimeManager.PlayOneShot(objPickUp.throwEventName, transform.position);
                    }

                    noThrow = true;
                    readyToThrow = false;
                    throwForce = 0.5f;
                    rotateVal = 0;
                    RemoveHandObj();

                    if (!aimHintDone)
                    {
                        DataHolder.HideHint(DataHolder.hints.throwHint);
                        aimHintDone = true;
                    }
                    
                }
                if (Input.GetMouseButton(0))
                {
                    if (readyToThrow && !noThrow)
                    {
                        if (throwForce < maxThrowForce)
                        {

                            throwForce += Time.deltaTime * 10f;
                            rotateVal -= Time.deltaTime;
                            holdingObj.position += Camera.main.transform.up * Time.deltaTime * .05f;
                            holdingObj.rotation *= Quaternion.Euler(rotateVal, 0, 0);

                            if (throwForce > 5f)
                                aimUI.SetActive(true);
                        }
                        aiming = true;
                        float uiScaleFactor = Mathf.Lerp(1, .3f, Mathf.InverseLerp(0, maxThrowForce, throwForce));
                        aimUI.transform.localScale = new Vector3(uiScaleFactor, uiScaleFactor, uiScaleFactor);
                    }

                }
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                readyToThrow = false;
                holdingObj.localPosition = Vector3.zero;
                aimUI.SetActive(false);
                aimUI.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (bypassThrow)
        {
            readyToThrow = false;
            if (objPickUp.objType != HandObjectType.CIGARETTE)
                holdingObj.localPosition = Vector3.zero;
            aimUI.SetActive(false);
            aimUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void DetectDoubleHand()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!noThrow)
            {
                isHolding = false;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                objPickUp.inHand = false;
                objPickUp.thrown = true;
                objPickUp.thrownByPlayer = true;

                GroceryBox box = holdingObj.GetComponent<GroceryBox>();
                if (box != null)
                {
                    box.CheckPlaceBox();
                }

                Vinyl vinyl = holdingObj.GetComponent<Vinyl>();
                if(vinyl != null)
                {
                    vinyl.CheckPlaceVinyl();
                }
                RemoveHandObj();    

            }
        }
    }

    private void DetectSurface()
    {

    }

    private void DetectPizzaHolding()
    {
        readyToThrow = false;
        aimUI.SetActive(false);
        aimUI.transform.localScale = new Vector3(1, 1, 1);
        if (Input.GetMouseButtonUp(0))
        {
            if (!noThrow)
            {
                isHolding = false;
                pizzaBox.AddPizza(holdingObj);
                objPickUp.inHand = false;
                RemoveHandObj();
            }
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        inhaling = true;
        float time = 0;
        Vector3 startPosition = holdingObj.localPosition;
        while (time < duration)
        {
            holdingObj.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        holdingObj.localPosition = targetPosition;
        inhaling = false;

    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = holdingObj.localRotation;
        while (time < duration)
        {
            holdingObj.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        holdingObj.localRotation = endValue;
    }
}
