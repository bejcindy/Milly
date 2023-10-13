using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliPowder : PickUpObject
{
    Animator anim;
    Vector3 startingPos;
    public ParticleSystem powderParticle;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        startingPos = transform.position;
    }

    protected override void Update()
    {
        if (playerHolding.atTable)
        {
            base.Update();
            if (inHand)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Use");
                    powderParticle.Play();
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

    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

    }
}
