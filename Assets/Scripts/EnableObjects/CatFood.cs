using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : PickUpObject
{
    public Xixi cat;
    public bool opened;
    Transform openCan;
    Transform closeCan;

    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.TRASH;
        openCan = transform.GetChild(0);
        closeCan = transform.GetChild(1);
    }

    protected override void Update()
    {
        if(!cat.inConversation)
            base.Update();
        if (inHand)
        {
            if (cat.inConversation && !DataHolder.camBlended && !opened)
            {
                transform.SetParent(cat.transform);
                objType = HandObjectType.CATFOOD;
                playerHolding.UnoccupyLeft();
                PlaceCan();
            }
        }

        if (opened)
        {
            if (!openCan.gameObject.activeSelf)
            {
                openCan.gameObject.SetActive(true);
                closeCan.gameObject.SetActive(false);
            }
        }

        if (!cat.inConversation && !inHand)
        {
            transform.SetParent(null);
            objType = HandObjectType.TRASH;
        }
    }

    public void PlaceCan()
    {
        inHand = false;
        StartCoroutine(LerpPosition(cat.catFoodPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(cat.catFoodRot), 1f));
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
        Quaternion startValue = transform.rotation;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
        opened = true;
    }
}
