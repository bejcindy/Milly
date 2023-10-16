using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliPowder : PickUpObject
{
    Animator anim;
    Vector3 startingPos;
    PlayerLeftHand leftHand;
    bool powderMoving;
    public ParticleSystem powderParticle;
    public TableController myTable;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        startingPos = transform.position;
        leftHand = player.GetComponent<PlayerLeftHand>();
    }

    protected override void Update()
    {
        if (playerHolding.atTable && !powderMoving && !StartSequence.noControl && myTable.tableControlOn)
        {
            base.Update();

            if(selected && !powderMoving)
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

            if (inHand && !leftHand.noThrow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Use");
                }

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

    public void PlayChiliParticle()
    {
        powderParticle.Play();
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        powderMoving = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        powderMoving = false;

    }
}
