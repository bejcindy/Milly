using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftHand : MonoBehaviour
{
    public bool isHolding;
    public bool noThrow;
    public bool inPizzaBox;
    public PizzaBox pizzaBox;
    public Transform holdingObj;
    public Vector3 holdingPosition;
    public PlayerHolding playerHolding;

    public Vector2 minThrowForce = new Vector2(100f, 50f);
    public Vector2 maxThrowForce = new Vector2(500f, 200f);
    float holdTime = 2f;
    [SerializeField]
    float holdTimer;
    Vector2 throwForce;

    // Start is called before the first frame update
    void Start()
    {
        noThrow = true;
        playerHolding = GetComponent<PlayerHolding>();

        pizzaBox = GameObject.Find("PizzaHolder").GetComponent<PizzaBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            if(!inPizzaBox)
                DetectHolding();
            else
            {
                DetectPizzaHolding();
            }
        }
    }

    private void DetectHolding()
    {

        if (Input.GetMouseButtonUp(0))
        {
            holdTimer = 0;
            if (!noThrow)
            {
                isHolding = false;
                holdingObj.GetComponent<Rigidbody>().isKinematic = false;
                holdingObj.GetComponent<PickUpObject>().inHand = false;
                holdingObj.SetParent(null);
                holdingObj.GetComponent<Rigidbody>().AddForce(throwForce.x * Camera.main.transform.forward + new Vector3(0, throwForce.y, 0));
                noThrow = true;
                playerHolding.UnoccupyLeft();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (holdTimer < holdTime)
            {
                holdTimer += Time.deltaTime;
                holdingObj.position -= Camera.main.transform.forward * Time.deltaTime * .1f;
            }
            float throwForceX = Mathf.Lerp(minThrowForce.x, maxThrowForce.x, Mathf.InverseLerp(0, holdTime, holdTimer));
            float throwForceY = Mathf.Lerp(minThrowForce.y, maxThrowForce.y, Mathf.InverseLerp(0, holdTime, holdTimer));
            throwForce = new Vector2(throwForceX, throwForceY);
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
}
