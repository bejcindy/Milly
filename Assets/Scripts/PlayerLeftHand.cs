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
    public GameObject aimHint;

    public Animator handAnim;
    public bool drinking;

     Vector2 minThrowForce = new Vector2(100f, 10f);
     Vector2 maxThrowForce = new Vector2(900f, 50f);
    float holdTime = 2f;
    [SerializeField]
    float holdTimer;
    Vector2 throwForce;

    bool readyToThrow;
    bool notHoldingAnyThing;

    bool aimHinted, smokingHinted;
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
        Debug.DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 30f,Color.cyan);
        if (isHolding && !playerHolding.inDialogue)
        {
            if (!inPizzaBox)
            {
                Drink();
                if(!drinking)
                    DetectHolding();
            }

            else
            {
                DetectPizzaHolding();
            }
        }

        if (smoking)
        {
            Smoke();
        }
        if (!isHolding && !GetComponent<PlayerRightHand>().isHolding || playerHolding.inDialogue)
        {
            if (aimHint.activeSelf)
                aimHint.SetActive(false);
        }
        else
        {
            if (!aimHint.activeSelf)
            {
                if (smoking)
                {
                    if (!smokingHinted)
                    {
                        aimHint.transform.GetChild(2).gameObject.SetActive(true);
                        aimHint.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (!aimHinted)
                    {
                        aimHint.transform.GetChild(2).gameObject.SetActive(false);
                        aimHint.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                aimHint.SetActive(true);
            }
                
        }
    }


    private void Smoke()
    {
        if (Input.mouseScrollDelta.y < 0 && holdingObj.localPosition.z > 0.2f && !inhaling)
        {
            Vector3 smokingPos = new Vector3(0, -0.15f, 0.22f);
            StartCoroutine(LerpPosition(smokingPos, 1f));
        }
        if(Input.mouseScrollDelta.y > 0 && holdingObj.localPosition.z < 0.75f && !inhaling)
        {
            StartCoroutine(LerpPosition(holdingPosition, 1f));
            holdingObj.GetComponent<Cigarette>().Inhale();
            //smokingHinted = true;
        }
    }

    private void Drink()
    {
        Debug.Log(handAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if(Input.mouseScrollDelta.y<0 && !drinking)
        {
            drinking = true;
            handAnim.Play("HandDrink");
        }
        if (handAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "HandDrink")
            drinking = false;
    }

    private void DetectHolding()
    {
        if (Input.GetMouseButtonDown(0) && holdingObj)
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
                        smokingHinted = true;
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
                    aimHinted = true;
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
            holdingObj.localPosition = holdingPosition;
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


}
