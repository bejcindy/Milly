using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BeerCup : PickUpObject
{
    public TableController myTable;
    public bool moving;
    Vector3 startingPos;

    public EventReference powderSound;
    public bool playedSF;

    protected override void Start()
    {
        base.Start();
        startingPos = transform.position;
    }

    protected override void Update()
    {
        if (playerHolding.atTable &&!moving && !StartSequence.noControl && myTable.tableControlOn)
        {
            base.Update();

            if (selected)
            {
                gameObject.layer = 9;
            }
            else if (inHand)
            {
                gameObject.layer = 7;
            }
            else
            {
                gameObject.layer = 0;
            }

            if (inHand)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    playerHolding.UnoccupyLeft();
                    inHand = false;
                    StartCoroutine(LerpPosition(startingPos, 1f));
                    transform.SetParent(null);
                }
            }
        }
        else
        {
            selected = false;
            gameObject.layer = 0;
        }


    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        moving = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        playedSF = false;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        moving = false;
        if (targetPosition == startingPos)
        {
            if (!pickUpSound.IsNull && !playedSF)
            {
                playedSF = true;
                RuntimeManager.PlayOneShot(pickUpSound, transform.position);
            }
        }
    }
}
