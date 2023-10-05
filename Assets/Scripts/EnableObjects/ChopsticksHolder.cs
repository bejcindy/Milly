using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopsticksHolder : LivableObject
{
    Vector3 chopHoldingPos;
    Quaternion chopHoldingRot;
    PlayerLeftHand playerLeftHand;
    PlayerHolding playerHolding;
    public Chopsticks myChops;
    public bool hasChop;
    bool chopMoving;


    protected override void Start()
    {
        base.Start();
        chopHoldingPos = myChops.transform.parent.localPosition;
        chopHoldingRot = myChops.transform.parent.localRotation;
        playerLeftHand = player.GetComponent<PlayerLeftHand>();
        playerHolding = player.GetComponent<PlayerHolding>();
    }
    protected override void Update()
    {
        base.Update();

        if (transform.childCount < 1)
            hasChop = false;

        if(interactable && !hasChop && playerLeftHand.GetCurrentChops()!=null && !chopMoving &&
            !playerLeftHand.chopAiming && !playerLeftHand.GetCurrentChops().hasFood && !playerLeftHand.GetCurrentChops().chopMoving)
        {
            gameObject.layer = 9;
            if(Input.GetMouseButtonDown(0))
                PutChops();
        }
        else
        {
            gameObject.layer = 0;
        }

        if (StartSequence.noControl && !hasChop)
        {
            PutChops();
        }

        if (chopMoving)
            myChops.enabled = false;
        else
            myChops.enabled = true;
    } 

    public void PutChops()
    {

        Chopsticks targetChops = playerLeftHand.GetCurrentChops();
        myChops = targetChops;
        targetChops.inHand = false;
        targetChops.transform.parent.SetParent(transform);
        StartCoroutine(LerpPosition(targetChops.transform.parent, 1f));
        StartCoroutine(LerpRotation(targetChops.transform.parent, 1f));
        playerHolding.UnoccupyLeft();
        hasChop = true;

    }

    IEnumerator LerpPosition(Transform chops, float duration)
    {
        chopMoving = true;
        float time = 0;
        Vector3 startPosition = chops.localPosition;
        while (time < duration)
        {
            chops.localPosition = Vector3.Lerp(startPosition, chopHoldingPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        chops.localPosition = chopHoldingPos;
        chopMoving = false;
    }

    IEnumerator LerpRotation(Transform chops, float duration)
    {
        float time = 0;
        Quaternion startValue = chops.localRotation;
        while (time < duration)
        {
            chops.localRotation = Quaternion.Lerp(startValue, chopHoldingRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        chops.localRotation = chopHoldingRot;
    }
}
