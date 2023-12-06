using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChopsticksHolder : LivableObject
{
    Vector3 chopHoldingPos;
    Quaternion chopHoldingRot;
    PlayerLeftHand playerLeftHand;
    PlayerHolding playerHolding;
    public Chopsticks myChops;
    public bool hasChop;
    bool chopMoving;
    public EventReference chopPutSound;

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

        if (interactable && !hasChop && playerLeftHand.GetCurrentChops() != null && !chopMoving &&
            !playerLeftHand.chopAiming && !playerLeftHand.GetCurrentChops().hasFood && !playerLeftHand.GetCurrentChops().chopMoving)
        {
            if (Input.GetMouseButtonDown(1))
                PutChops();
        }
        else if (activated && MainQuestState.firstGloriaTalk)
            gameObject.layer = 17;
        else
            gameObject.layer = 0;

        if ((StartSequence.noControl || !playerHolding.atTable) && !hasChop)        
            PutChops();        

        if(myChops != null)
        {
            if (chopMoving)
                myChops.enabled = false;
            else
                myChops.enabled = true;
        }
        else        
            chopMoving = false;
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
        playerLeftHand.RemoveCurrentChops();
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
        if(!chopPutSound.IsNull)
            RuntimeManager.PlayOneShot(chopPutSound, transform.position);
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
