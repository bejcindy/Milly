using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChopsticksHolder : LivableObject
{
    Vector3 chopHoldingPos;
    Quaternion chopHoldingRot;
    public Chopsticks myChops;
    public GameObject chopAimingUI;
    public bool hasChop;
    bool chopMoving;
    public EventReference chopPutSound;
    public GameObject eatingDialogue;

    protected override void Start()
    {
        base.Start();
        chopHoldingPos = myChops.transform.parent.localPosition;
        chopHoldingRot = myChops.transform.parent.localRotation;
    }

    protected override void Update()
    {
        base.Update();

        if (transform.childCount < 1)
            hasChop = false;

        if (!hasChop && playerLeftHand.GetCurrentChops() != null && !chopMoving &&
            !playerLeftHand.GetCurrentChops().hasFood && !playerLeftHand.GetCurrentChops().chopMoving)
        {
            if (Input.GetMouseButtonDown(1))
                PutChops();
        }
        else if (activated && MainQuestState.firstGloriaTalk)
            gameObject.layer = 17;
        else
            gameObject.layer = 0;

        if ((StartSequence.noControl || !playerHolding.atTable) && !hasChop)
        {
            if (myChops.hasFood)
            {
                myChops.hasFood = false;
                playerLeftHand.DestroyFood();
            }
            PutChops();
        }        
     

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
        playerLeftHand.chopAiming = false;
        chopAimingUI.SetActive(false);
        if(PlayerLeftHand.foodAte > 1)
        {
            eatingDialogue.SetActive(true);
        }
        Chopsticks targetChops = playerLeftHand.GetCurrentChops();
        myChops = targetChops;
        targetChops.inHand = false;
        targetChops.transform.parent.SetParent(transform);
        StartCoroutine(LerpPosition(targetChops.transform.parent, 1f));
        StartCoroutine(LerpRotation(targetChops.transform.parent, 1f));
        playerLeftHand.RemoveHandObj();
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
