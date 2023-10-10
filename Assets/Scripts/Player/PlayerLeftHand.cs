using System.Collections;
using System.Collections.Generic;
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
     Vector2 minThrowForce = new Vector2(100f, 10f);
     Vector2 maxThrowForce = new Vector2(900f, 50f);
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

    bool aimHinted, smokingHinted,drinkHinted;
    bool aimHintDone, smokingHintDone,drinkHintDone;
    bool chophinted;
    bool canSmoke;
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
                if(holdingObj.GetComponent<PickUpObject>().objType == HandObjectType.DRINK)
                    Drink();
                if(!drinking && !playerHolding.atInterior)
                    DetectHolding();
            }

            else
            {
                DetectPizzaHolding();
            }


            if (holdingObj != null && holdingObj.TryGetComponent<Chopsticks>(out Chopsticks chop))
            {
                currentChop = holdingObj.GetComponent<Chopsticks>();
                UsingChopsticks();
                ChopUIDetect();
            }
            else
            {
                currentChop = null;
            }

            if (smoking)
            {
                Smoke();
                if (!smokingHinted)
                {
                    DataHolder.HideHint();
                    smokingHinted = true;
                }
            }
        }






        #region Hint Region
        if (smoking)
        {
            DataHolder.ShowHint(DataHolder.hints.smokeHint);
            smokingHintDone = false;
        }
        else if (drinking)
        {
            DataHolder.ShowHint(DataHolder.hints.drinkHint);
            drinkHintDone = false;
        }
        else if (currentChop && !currentChop.hasFood)
        {
            DataHolder.ShowHint(DataHolder.hints.chopHint);
            chophinted = true;
        }
        else if (currentChop && currentChop.hasFood)
        {
            DataHolder.ShowHint(DataHolder.hints.eatHint);
            chophinted = true;
        }
        else if (!currentChop && chophinted)
        {
            DataHolder.HideHint();
            chophinted = false;
        }
        else if (isHolding)
        {
            DataHolder.ShowHint(DataHolder.hints.throwHint);
            aimHintDone = false;
        }
        else if (canSmoke && !smokingHinted)
        {
            DataHolder.ShowHint(DataHolder.hints.cigHint);
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
                if(chopHoldVal>0.2f)
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

        if(currentChop.hasFood)
        {
            if(Input.mouseScrollDelta.y < 0)
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

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "StartConvo" && !smokingHinted)
        {
            canSmoke = true;
        }
    }



    private void Smoke()
    {
        if (Input.mouseScrollDelta.y < 0 && holdingObj.localPosition.z > -0.1f && !inhaling)
        {
            Vector3 smokingPos = new Vector3(0.5f, 0f, -0.3f);
            Vector3 smokingRot = new Vector3(0, 180, 0);
            StartCoroutine(LerpPosition(smokingPos, 1f));
            StartCoroutine(LerpRotation(Quaternion.Euler(smokingRot), 1f));
        }
        if(Input.mouseScrollDelta.y > 0 && holdingObj.localPosition.z < 0f && !inhaling)
        {
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

        if (Input.mouseScrollDelta.y<0 && !drinking)
        {
            drinking = true;
            handAnim.Play("HandDrink");
            //drinkHinted = true;
            if (!drinkHintDone)
            {
                DataHolder.HideHint();
                drinkHintDone = true;
            }
        }
        if (handAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HandDrink")
            drinking = false;
    }

    private void DetectHolding()
    {
        if (Input.GetMouseButtonDown(0) && holdingObj && !PauseMenu.isPaused)
        {
            readyToThrow = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (readyToThrow)
            {
                aimUI.SetActive(false);
                aimUI.transform.localScale = new Vector3(1, 1, 1);
                playerHolding.throwing = false;
                holdTimer = 0;
                if (!noThrow)
                {
                    if (smoking)
                    {
                        holdingObj.GetComponent<Cigarette>().FinishSmoking();
                        if (!smokingHintDone)
                        {
                            DataHolder.HideHint();
                            smokingHintDone = true;
                        }
                    }
                    isHolding = false;
                    smoking = false;
                    holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                    holdingObj.GetComponent<PickUpObject>().inHand = false;
                    holdingObj.GetComponent<PickUpObject>().thrown = true;
                    holdingObj.SetParent(null);
                    //holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * Camera.main.transform.forward+new Vector3(xOffset,0,0) + new Vector3(0, throwForce.y, 0));
                    if (playerHolding.atContainer && playerHolding.currentContainer.CheckMatchingObject(holdingObj.gameObject))
                    {
                        isHolding = false;
                        StartCoroutine(playerHolding.currentContainer.MoveAcceptedObject(holdingObj, 1f));
                    }
                    else
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                            //holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - transform.position).normalized);
                            //Debug.Log("hit " + hit.transform.gameObject.name);
                        }
                        else
                        {
                            holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (Camera.main.transform.position+ ray.direction * 30f - holdingObj.transform.position).normalized + new Vector3(0, throwForce.y, 0));
                            //Debug.Log("didn't hit");
                        }
                        FMODUnity.RuntimeManager.PlayOneShot(holdingObj.GetComponent<PickUpObject>().throwEventName, transform.position);
                    }
                    
                    noThrow = true;
                    playerHolding.UnoccupyLeft();
                    if (!aimHintDone)
                    {
                        Debug.Log("hinted");
                        DataHolder.HideHint();
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
                    playerHolding.throwing = true;
                    holdTimer += Time.deltaTime;
                    holdingObj.position -= Camera.main.transform.forward * Time.deltaTime * .1f;
                    if (holdTimer > 0.2f)
                        aimUI.SetActive(true);
                }
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
