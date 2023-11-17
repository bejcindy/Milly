using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lid : LivableObject
{
    public bool lidMoving;
    public bool lidOpen;
    public Vector3 targetRot;
    Quaternion openRotation;
    Quaternion closeRotation;

    public LivableObject controlledContainer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (lidOpen)
        {
            openRotation = transform.localRotation;
            closeRotation = Quaternion.Euler(targetRot);
        }

        else
        {
            openRotation = Quaternion.Euler(targetRot);
            closeRotation = transform.localRotation;
        }

    }

    // Update is called once per frame
    protected override void Update()
    { 
        base.Update();
        if (interactable && !lidMoving)
        {
            LidControl();
        }
        else
        {
            gameObject.layer = 0;
        }

        if (!lidOpen)
        {
            if(controlledContainer != null)
            {
                controlledContainer.enabled = false;
            }
        }
        else
        {
            if (controlledContainer != null)
            {
                controlledContainer.enabled = true;
            }
        }
    }

    void LidControl()
    {
        gameObject.layer = 9;

        if (Input.GetMouseButton(0))
        {
            float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime;
            if (lidOpen)
            {
                if (verticalInput < 0)
                {
                    StartCoroutine(LerpRotation(closeRotation, 1f));
                }
            }
            else
            {
                if (verticalInput > 0)
                {
                    StartCoroutine(LerpRotation(openRotation, 1f));
                }
            }
        }

    }


    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        lidMoving = true;
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endValue;
        lidMoving = false;

        if(endValue == openRotation)
        {
            lidOpen = true;
        }
        else
        {
            lidOpen = false;
        }
    }
}
