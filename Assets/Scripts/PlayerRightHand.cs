using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRightHand : MonoBehaviour
{
    float xOffset = 50;
    public bool isHolding;
    public bool noThrow;
    public bool inPizzaBox;

    public bool smoking;
    public bool inhaling;

    public PizzaBox pizzaBox;
    public Transform holdingObj;
    public Vector3 holdingPosition;
    public PlayerHolding playerHolding;
    public GameObject aimUI;

     Vector2 minThrowForce = new Vector2(100f, 10f);
     Vector2 maxThrowForce = new Vector2(500f, 100f);
    float holdTime = 2f;
    [SerializeField]
    float holdTimer;
    Vector2 throwForce;

    bool readyToThrow;
    // Start is called before the first frame update
    void Start()
    {
        noThrow = true;
        playerHolding = GetComponent<PlayerHolding>();
        pizzaBox = GameObject.Find("PizzaHolder").GetComponent<PizzaBox>();
        aimUI = GetComponent<PlayerLeftHand>().aimUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            if (!inPizzaBox)
                DetectHolding();
            else
            {
                DetectPizzaHolding();
            }
        }
        if (smoking)
        {
            Smoke();
        }
    }

    private void Smoke()
    {
        if (Input.mouseScrollDelta.y < 0 && holdingObj.localPosition.z > 0.2f && !inhaling)
        {
            Vector3 smokingPos = new Vector3(0, -0.15f, 0.22f);
            StartCoroutine(LerpPosition(smokingPos, 1f));
        }
        if (Input.mouseScrollDelta.y > 0 && !inhaling)
        {
            StartCoroutine(LerpPosition(holdingPosition, 1f));
            holdingObj.GetComponent<Cigarette>().Inhale();
        }
    }

    private void DetectHolding()
    {
        if (Input.GetMouseButtonDown(1) && holdingObj)
        {
            readyToThrow = true;
        }
        if (Input.GetMouseButtonUp(1))
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
                    }
                    isHolding = false;
                    smoking = false;
                    holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                    holdingObj.GetComponent<PickUpObject>().inHand = false;
                    holdingObj.GetComponent<PickUpObject>().thrown = true;
                    holdingObj.SetParent(null);
                    //holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * Camera.main.transform.forward + new Vector3(xOffset, 0, 0) + new Vector3(0, throwForce.y, 0));
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (hit.point - transform.position).normalized + new Vector3(0, throwForce.y, 0));
                        //Debug.Log("hit " + hit.transform.gameObject.name);
                    }
                    else
                    {
                        holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * (Camera.main.transform.forward * 30f - transform.position).normalized + new Vector3(0, throwForce.y, 0));
                        //Debug.Log("didn't hit");
                    }
                    noThrow = true;
                    playerHolding.UnoccupyRight();
                    readyToThrow = false;
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (readyToThrow)
            {
                if (holdTimer < holdTime)
                {
                    holdTimer += Time.deltaTime;
                    holdingObj.position -= Camera.main.transform.forward * Time.deltaTime * .1f;
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
        if (Input.GetMouseButtonUp(1))
        {
            if (!noThrow)
            {
                isHolding = false;
                pizzaBox.AddPizza(holdingObj);
                holdingObj.GetComponent<PickUpObject>().inHand = false;
                playerHolding.UnoccupyRight();
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
