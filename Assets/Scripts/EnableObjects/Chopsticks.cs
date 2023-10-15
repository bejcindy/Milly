using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopsticks : PickUpObject
{
    Transform otherChopstick;
    public Vector3 foodPickedPos;
    public Vector3 chopPickingPos;
    public Vector3 chopPickingRot;
    Quaternion chopOriginalRot;
    public bool hasFood;
    public bool chopMoving;
    Animator anim;

    protected override void Start() 
    {
        base.Start();
        otherChopstick = transform.parent.GetChild(1).transform;
        anim = transform.parent.GetComponent<Animator>();
        chopOriginalRot = transform.localRotation;

    }

    protected override void Update()
    {
        if (playerHolding.atTable && !StartSequence.noControl)
        {
            base.Update();
            if (selected && !thrown)
                otherChopstick.gameObject.layer = 9;
            else if(inHand)
                otherChopstick.gameObject.layer = 7;
            else
                otherChopstick.gameObject.layer = 0;
        }
        else
        {
            selected = false;
            gameObject.layer = 0;
            otherChopstick.gameObject.layer = 0;
        }





    }

    public void SetChopAnimPick()
    {
        anim.ResetTrigger("Unpick");
        anim.SetTrigger("Pick");
    }

    public void SetChopAnimUnpick()
    {
        anim.ResetTrigger("Pick");
        anim.SetTrigger("Unpick");
    }

    public void SetEatAnim()
    {
        anim.SetTrigger("Eat");
    }

    public IEnumerator LerpChopPicking(float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.parent.localPosition;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.parent.localPosition = Vector3.Lerp(startPosition, chopPickingPos, time / duration);
            transform.localRotation = Quaternion.Lerp(startValue, Quaternion.Euler(chopPickingRot), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = chopPickingPos;
        transform.localRotation = Quaternion.Euler(chopPickingRot);

    }

    public IEnumerator LerpChopUnpicking(float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.parent.localPosition;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.parent.localPosition = Vector3.Lerp(startPosition, Vector3.zero, time / duration);
            transform.localRotation = Quaternion.Lerp(startValue, chopOriginalRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = Vector3.zero;
        transform.localRotation = chopOriginalRot;
    }

    public IEnumerator LerpChopRotation(Quaternion targetRot, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.parent.localRotation;
        while (time < duration)
        {
            transform.parent.localRotation = Quaternion.Lerp(startValue, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.localRotation = targetRot;
    }
}
