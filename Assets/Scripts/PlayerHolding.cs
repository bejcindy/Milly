using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHolding : MonoBehaviour
{
    public bool fullHand;
    PlayerLeftHand leftHand;
    PlayerRightHand rightHand;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<PlayerLeftHand>();
        rightHand = GetComponent<PlayerRightHand>();
    }

    // Update is called once per frame
    void Update()
    {
        GetFullHand();
    }



    public bool GetLeftHand()
    {
        if (leftHand.isHolding)
        {
            return false;
        }
        leftHand.noThrow = true;
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

    public void GetFullHand()
    {
        if (leftHand.isHolding && rightHand.isHolding)
            fullHand = true;
        else
            fullHand = false;
    }

    public void OccupyLeft(Transform obj)
    {
        leftHand.isHolding = true;
        leftHand.holdingObj = obj;
        obj.SetParent(Camera.main.transform);
        obj.localPosition = leftHand.holdingPosition;
        Invoke(nameof(EnableThrowLeft), 0.5f);
    }

    public void OccupyRight(Transform obj)
    {
        rightHand.isHolding = true;
        rightHand.holdingObj = obj;
        obj.SetParent(Camera.main.transform);
        obj.localPosition = rightHand.holdingPosition;
        Invoke(nameof(EnableThrowRight), 0.5f);
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
}
