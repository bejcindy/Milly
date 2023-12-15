using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChiliPowder : PickUpObject
{
    Animator anim;
    Vector3 startingPos;
    bool powderMoving;
    public ParticleSystem powderParticle;
    public TableController myTable;

    public EventReference powderSound;
    public bool playedSF;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        startingPos = transform.position;
    }

    protected override void Update()
    {
        if (!powderMoving && myTable.tableControlOn)
        {
            base.Update();

            if (selected && !powderMoving)
            {
                gameObject.layer = 9;
            }
            else if (inHand)
            {
                gameObject.layer = 7;
            }
            //else if (activated)
            //{
            //    gameObject.layer = 17;
            //}
            else
            {
                gameObject.layer = 0;
            }

            if (inHand && !playerLeftHand.noThrow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Use");
                    if (!powderSound.IsNull)
                        RuntimeManager.PlayOneShot(powderSound, transform.position);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    PutDownPowder();
                }
            }
        }
        else if (!myTable.tableControlOn)
        {
            selected = false;
            playerHolding.RemoveInteractable(gameObject);
            if (inHand)
                PutDownPowder();

            gameObject.layer = 0;
            //if (!MainQuestState.firstGloriaTalk)
            //    gameObject.layer = 0;
            //else
            //    gameObject.layer = 17;
        }
    }

    public void PutDownPowder()
    {
        playerHolding.UnoccupyLeft();
        inHand = false;
        StartCoroutine(LerpPosition(startingPos, 1f));
        transform.SetParent(null);
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
        playedSF = false;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        powderMoving = false;
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
