using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PlayerLeftHand : MonoBehaviour
{
    float xOffset = -50;
    public bool isHolding;
    public bool noThrow;
    public bool inPizzaBox;

    public bool smoking;
    public bool inhaling;

    public float pullForce;
    public PizzaBox pizzaBox;
    public Transform holdingObj;
    public Vector3 holdingPosition;
    public Vector3 smokingPosition;
    public PlayerHolding playerHolding;
    public GameObject aimUI;
    public GameObject chopAimUI;
    public GameObject aimHint;

    public Animator handAnim;
    public bool drinking;
    public ParticleSystem smokeVFX;
    Vector2 minThrowForce = new Vector2(100f, 50f);
    Vector2 maxThrowForce = new Vector2(800f, 200f);
    float holdTime = 2f;

    Chopsticks currentChop;
    bool chopOut;
    float chopHoldTimeMax = 1f;
    public float chopHoldVal = 0;
    public bool chopAiming;
    public static int foodAte;
    bool foodEatingDialogueDone;

    public Transform selectedFood;
    Transform currentFood;
    [SerializeField]
    float holdTimer;

    Vector2 throwForce;

    public bool readyToThrow;
    bool notHoldingAnyThing;

    public Transform groundDetector;
    public Transform surfaceDetector;
    public LayerMask groundLayer;

    public GameObject eatingDialogue;


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
        noThrow = true;
        playerHolding = GetComponent<PlayerHolding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding && !MainTattooMenu.tatMenuOn)
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
                holdTimer = 0;
                throwForce = Vector2.zero;
                aimUI.SetActive(false);
                aimUI.transform.localScale = new Vector3(1, 1, 1);
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
        else if (currentChop && !currentChop.hasFood && !chopAiming)
        {
            DataHolder.HideHint(DataHolder.hints.pickFoodHint);
            DataHolder.HideHint(DataHolder.hints.eatHint);
            DataHolder.ShowHint(DataHolder.hints.chopHint);
            chophinted = true;
        }
        else if (currentChop && !currentChop.hasFood && chopAiming)
        {
            //关其他的
            DataHolder.HideHint(DataHolder.hints.chopHint);
            DataHolder.HideHint(DataHolder.hints.eatHint);
            DataHolder.ShowHint(DataHolder.hints.pickFoodHint);
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

    public void HoldingAction()
    {
        PickUpObject pickUp = holdingObj.GetComponent<PickUpObject>();
        switch (pickUp.objType)
        {
            case HandObjectType.DRINK:
                Drink();
                if (!drinking && !playerHolding.atInterior)
                    DetectHolding();
                break;
            case HandObjectType.CHOPSTICKS:
                currentChop = holdingObj.GetComponent<Chopsticks>();
                UsingChopsticks();
                ChopUIDetect();
                break;
            case HandObjectType.CIGARETTE:
                Smoke();
                if (!playerHolding.atInterior)
                    DetectHolding();
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
                    DetectHolding();
                break;
        }
    }


    #region Chopsticks Region
    private void UsingChopsticks()
    {
        if (Input.GetMouseButton(0) && !holdingObj.GetComponent<Chopsticks>().hasFood)
        {
            chopOut = false;
            if (chopHoldVal < chopHoldTimeMax)
            {
                chopHoldVal += Time.deltaTime;
                if (chopHoldVal > 0.2f)
                    chopAiming = true;
                currentChop.transform.parent.localPosition += Camera.main.transform.forward * Time.deltaTime * 0.1f;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            chopOut = true;
            chopHoldVal = 0;

            if (chopAiming && selectedFood)
            {
                if (!currentChop.hasFood)
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
            chopAiming = false;
        }

        if (chopOut)
            DrawBackChop(currentChop);

        if (currentChop.hasFood)
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
        currentChop.hasFood = false;
        Invoke(nameof(DestroyFood), 1f);
    }

    private void ChopUIDetect()
    {
        if (chopAiming)
            chopAimUI.SetActive(true);
        else
            chopAimUI.SetActive(false);
    }

    public void DestroyFood()
    {
        if (!playerHolding.inDialogue && (foodAte != 2))
            currentFood.GetComponent<FoodPickObject>().CheckFirstAte();
        Destroy(currentFood.gameObject);
        currentChop.chopMoving = false;
        currentChop.PlayEatSound();
        foodAte++;
    }

    private void DrawBackChop(Chopsticks chop)
    {
        if (chop.transform.parent.localPosition != Vector3.zero)
        {
            chop.transform.parent.localPosition = Vector3.Lerp(chop.transform.parent.localPosition, Vector3.zero, 0.5f);
        }
        else
            chopOut = false;
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

    public bool bypassThrow;
    private void DetectHolding()
    {
        if (!bypassThrow && !playerHolding.inDialogue)
        {
            if (Input.GetMouseButtonDown(0) && holdingObj && !PauseMenu.isPaused)
            {
                readyToThrow = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (readyToThrow)
                {
                    aiming = false;
                    aimUI.SetActive(false);
                    aimUI.transform.localScale = new Vector3(1, 1, 1);
                    holdTimer = 0;
                    if (!noThrow)
                    {
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
                        holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                        holdingObj.GetComponent<PickUpObject>().inHand = false;
                        holdingObj.GetComponent<PickUpObject>().thrown = true;
                        holdingObj.GetComponent<PickUpObject>().thrownByPlayer = true;
                        holdingObj.SetParent(null);

                        if (playerHolding.atContainer && playerHolding.currentContainer.CheckMatchingObject(holdingObj.gameObject))
                        {
                            isHolding = false;
                            StartCoroutine(playerHolding.currentContainer.MoveAcceptedObject(holdingObj, 1f));
                        }
                        else
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
                            {
                                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                                //Debug.Log("hit " + hit.transform.gameObject.name);
                            }
                            else
                            {
                                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (Camera.main.transform.position + Camera.main.transform.forward * 300f - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                            }
                            FMODUnity.RuntimeManager.PlayOneShot(holdingObj.GetComponent<PickUpObject>().throwEventName, transform.position);
                        }

                        noThrow = true;
                        playerHolding.UnoccupyLeft();
                        if (!aimHintDone)
                        {
                            DataHolder.HideHint(DataHolder.hints.throwHint);
                            aimHintDone = true;
                        }
                        readyToThrow = false;
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (readyToThrow)
                {
                    if (holdTimer < holdTime)
                    {
                        holdTimer += Time.deltaTime;
                        holdingObj.position -= Camera.main.transform.forward * Time.deltaTime * .1f;
                        if (holdTimer > 0.2f)
                            aimUI.SetActive(true);
                    }
                    aiming = true;
                    float throwForceX = Mathf.Lerp(minThrowForce.x, maxThrowForce.x, Mathf.InverseLerp(0, holdTime, holdTimer));
                    float throwForceY = Mathf.Lerp(minThrowForce.y, maxThrowForce.y, Mathf.InverseLerp(0, holdTime, holdTimer));

                    throwForce = new Vector2(throwForceX, throwForceY);
                    float uiScaleFactor = Mathf.Lerp(1, .3f, Mathf.InverseLerp(0, holdTime, holdTimer));
                    aimUI.transform.localScale = new Vector3(uiScaleFactor, uiScaleFactor, uiScaleFactor);
                }

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                readyToThrow = false;
                holdingObj.localPosition = Vector3.zero;
                holdTimer = 0;
                throwForce = Vector2.zero;
                aimUI.SetActive(false);
                aimUI.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (bypassThrow)
        {
            readyToThrow = false;
            if (holdingObj.GetComponent<PickUpObject>().objType != HandObjectType.CIGARETTE)
                holdingObj.localPosition = Vector3.zero;
            holdTimer = 0;
            throwForce = Vector2.zero;
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
                holdingObj.GetComponent<PickUpObject>().inHand = false;
                holdingObj.GetComponent<PickUpObject>().thrown = true;
                holdingObj.GetComponent<PickUpObject>().thrownByPlayer = true;

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
                playerHolding.UnoccupyLeft();

            }
        }
    }

    private void DetectSurface()
    {

    }

    private void DetectPizzaHolding()
    {
        readyToThrow = false;
        holdTimer = 0;
        throwForce = Vector2.zero;
        aimUI.SetActive(false);
        aimUI.transform.localScale = new Vector3(1, 1, 1);
        if (Input.GetMouseButtonUp(0))
        {
            if (!noThrow)
            {
                isHolding = false;
                pizzaBox.AddPizza(holdingObj);
                holdingObj.GetComponent<PickUpObject>().inHand = false;
                playerHolding.UnoccupyLeft();
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
