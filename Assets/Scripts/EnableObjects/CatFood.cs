using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : PickUpObject
{
    public Xixi cat;
    public bool opened;
    public bool finishedEating;

    public bool finishDiaDone;
    string openCanSound = "event:/Sound Effects/ObjectInteraction/CatCan/CanOpen";
    Transform openCan;
    Transform closeCan;
    Transform wetFood;

    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.TRASH;
        openCan = transform.GetChild(1);
        closeCan = transform.GetChild(0);
        wetFood = transform.GetChild(2);
    }

    protected override void Update()
    {
        if(!StartSequence.noControl || overrideStartSequence)
        {
            if (!cat.inConversation)
                base.Update();

            if (inHand)
            {
                if (cat.inConversation && !DataHolder.camBlended && !opened)
                {
                    transform.SetParent(cat.transform);
                    objType = HandObjectType.CATFOOD;
                    playerLeftHand.RemoveHandObj();
                    PlaceCan();
                }

            }

            if (opened)
            {

                if (!openCan.gameObject.activeSelf)
                {
                    cat.EatCan();
                    rend = openCan.GetComponent<Renderer>();
                    openCan.gameObject.SetActive(true);
                    closeCan.gameObject.SetActive(false);
                }

                if(MainQuestState.firstGloriaTalk && selected)
                {
                    rb.isKinematic = false;
                    DataHolder.ShowHint(DataHolder.hints.soccerHint);
                }

                if(kicked || inHand || !selected)
                {
                    DataHolder.HideHint(DataHolder.hints.soccerHint);
                }
            }

            if (!cat.inConversation && !inHand)
            {
                transform.SetParent(null);
                objType = HandObjectType.TRASH;
            }

            if (selected && !thrown)
                rend.gameObject.layer = 9;
            else if (inHand)
                rend.gameObject.layer = 7;
            else if (transformed)
                rend.gameObject.layer = 17;
            else
                rend.gameObject.layer = 0;
        }

    }

    public void PlaceCan()
    {
        FMODUnity.RuntimeManager.PlayOneShot(openCanSound, transform.position);
        inHand = false;
        wetFood.gameObject.layer = 17;
        StartCoroutine(LerpPosition(cat.catFoodPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(cat.catFoodRot), 1f));
    }


    public void FinishEating()
    {
        transform.SetParent(null);
        finishedEating = true;
        wetFood.gameObject.SetActive(false);
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
        opened = true;
    }

    IEnumerator LerpRotation(Quaternion targetRot, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = targetRot;
        opened = true;
    }
}
