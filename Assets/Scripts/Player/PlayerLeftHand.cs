using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    Vector2 maxThrowForce = new Vector2(900f, 250f);
    float holdTime = 2f;

    Chopsticks currentChop;
    bool chopOut;
    float chopHoldTimeMax = 1f;
    public float chopHoldVal = 0;
    public bool chopAiming;

    public Transform selectedFood;
    Transform currentFood;
    [SerializeField]
    float holdTimer;

    Vector2 throwForce;

    public bool readyToThrow;
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
        noThrow = true;
        playerHolding = GetComponent<PlayerHolding>();

        pizzaBox = GameObject.Find("PizzaHolder").GetComponent<PizzaBox>();
        //aimHint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            if (!inPizzaBox)
            {
                if (holdingObj.GetComponent<PickUpObject>().objType == HandObjectType.DRINK)
                    Drink();
                if (!drinking && !playerHolding.atInterior)
                    DetectHolding();
            }

            else
            {
                DetectPizzaHolding();
            }


            if (holdingObj != null && holdingObj.TryGetComponent<Chopsticks>(out Chopsticks chop))
            {
                if (holdingObj.GetComponent<Chopsticks>())
                {
                    currentChop = holdingObj.GetComponent<Chopsticks>();
                    UsingChopsticks();
                    ChopUIDetect();
                }
                else
                {
                    currentChop = null;
                }
            }
            if (smoking)
            {
                Smoke();
                if (!smokingHinted)
                {
                    DataHolder.HideHint(DataHolder.hints.cigHint);
                    smokingHinted = true;
                }
            }
            //if (holdingObj)
            //{
            //    holdingObj.gameObject.layer = 16;
            //}
        }
        else
        {
            inhaling = false;
            drinking = false;
            smoking = false;
        }






        #region Hint Region
        if (smoking)
        {
            DataHolder.ShowHint(DataHolder.hints.smokeHint);
            smokingHintDone = false;
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
            drinkHintDone = true;
        }
        #endregion
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
                currentChop.chopMoving = true;
                currentChop.SetEatAnim();
                currentChop.hasFood = false;
                Invoke(nameof(DestroyFood), 1f);
            }
        }


    }

    private void ChopUIDetect()
    {
        if (chopAiming)
            chopAimUI.SetActive(true);
        else
            chopAimUI.SetActive(false);
    }

    private void DestroyFood()
    {
        Destroy(currentFood.gameObject);
        currentChop.chopMoving = false;
        currentChop.PlayEatSound();
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
        if (other.name == "StartConvo" && !smokingHinted)
        {
            Debug.Log("should hint smoke");
            canSmoke = true;
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
            //drinkHinted = true;
            if (!drinkHintDone)
            {
                DataHolder.HideHint(DataHolder.hints.drinkHint);
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Swallow", transform.position);
                drinkHintDone = true;
            }
        }
        if (handAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HandDrink")
            drinking = false;
    }
    public bool bypassThrow;
    private void DetectHolding()
    {
        if (!bypassThrow)
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
                        //holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * Camera.main.transform.forward+new Vector3(xOffset,0,0) + new Vector3(0, throwForce.y, 0));
                        if (playerHolding.atContainer && playerHolding.currentContainer.CheckMatchingObject(holdingObj.gameObject))
                        {
                            isHolding = false;
                            StartCoroutine(playerHolding.currentContainer.MoveAcceptedObject(holdingObj, 1f));
                        }
                        else
                        {
                            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            //Ray ray = Camera.main.transform.forward;
                            RaycastHit hit;
                            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                            if (Physics.Raycast(transform.position,Camera.main.transform.forward, out hit, Mathf.Infinity,~0, QueryTriggerInteraction.Ignore))
                            {
                                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                                //holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - transform.position).normalized);
                                Debug.Log("hit " + hit.transform.gameObject.name);
                            }
                            else
                            {
                                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (Camera.main.transform.position + Camera.main.transform.forward * 300f - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                                //Debug.Log("didn't hit");
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
        else
        {
            readyToThrow = false;
            holdingObj.localPosition = Vector3.zero;
            holdTimer = 0;
            throwForce = Vector2.zero;
            aimUI.SetActive(false);
            aimUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void DetectPizzaHolding()
    {
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
