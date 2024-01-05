using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BeerCup : PickUpObject
{    
    Transform liquid;
    float liquidLevel;
    Material liquidMat;
    Vector3 startingPos;
    Quaternion startRotation;

    public bool moving;
    public TableController myTable;
    public EventReference powderSound;
    public bool playedSF;

    protected override void Start()
    {
        base.Start();
        startingPos = transform.position;
        liquid = transform.GetChild(0);
        startRotation = transform.rotation;
        liquidMat = liquid.GetComponent<Renderer>().material;
        liquidLevel = liquidMat.GetFloat("_FillAmount");
    }

    protected override void Update()
    {
        Debug.Log("Beer fill amount is" + liquidLevel);
        if (!moving && myTable.tableControlOn)
        {
            base.Update();
            if (selected)
            {
                gameObject.layer = 9;

            }
            else if (inHand)
            {
                gameObject.layer = 7;
                liquid.gameObject.layer = 7;
            }
            //else if (activated)
            //{
            //    gameObject.layer = 17;
            //    liquid.gameObject.layer = 17;
            //}
            else
            {
                gameObject.layer = 0;
                liquid.gameObject.layer = 0;
            }

            if (inHand && !ReferenceTool.playerLeftHand.noThrow)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    liquid.gameObject.layer = 17;
                    PutDownCup();
                }
            }
        }
        else if (!myTable.tableControlOn)
        {
            selected = false;
            playerHolding.RemoveInteractable(gameObject);
            if (inHand)
                PutDownCup();

            gameObject.layer = 0;
            liquid.gameObject.layer = 0;
            //if (!MainQuestState.firstGloriaTalk)
            //    gameObject.layer = 0;
            //else
            //    gameObject.layer = 17;
        }
    }

    public void PutDownCup()
    {
        playerHolding.UnoccupyLeft();
        inHand = false;
        StartCoroutine(LerpPosition(startingPos, 1f));
        StartCoroutine(LerpRotation(startRotation, 1f));
        transform.SetParent(null);
    }

    public void ReduceLiquid()
    {
        if(liquidLevel < 0.6f)
        {
            liquidLevel += 0.02f;
            liquidMat.SetFloat("_FillAmount", liquidLevel);
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

    public IEnumerator LerpRotation(Quaternion targetRot, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
    }
}
