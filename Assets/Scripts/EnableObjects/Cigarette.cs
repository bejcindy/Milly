using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using VInspector;

public class Cigarette : PickUpObject
{
    [Foldout("Cigarette")]
    public int cigStage;
    public bool activateAll;
    public Transform activeCigObj;
    public EventReference inhaleEvent;
    public EventReference exhaleEvent;



    protected override void Start()
    {
        base.Start();
        rend = transform.GetChild(0).GetComponent<Renderer>();
    }
    protected override void Update()
    {
        base.Update();
        SwitchCig();

        if(cigStage > 3)
            objType = HandObjectType.TRASH;


        if (selected && !thrown)
            activeCigObj.gameObject.layer = 9;
        else if (inHand)
            activeCigObj.gameObject.layer = 7;
        else
            activeCigObj.gameObject.layer = 17;

        if (activated && !activateAll)
            ActivateAllCig();
    }


    void SwitchCig()
    {
        activeCigObj = transform.GetChild(cigStage);
        rend = activeCigObj.GetComponent<Renderer>();
        mat = rend.material;
        visibleDetector = activeCigObj.GetComponent<RiggedVisibleDetector>();
        activeCigObj.gameObject.SetActive(true);
        foreach(Transform child in transform)
        {
            if(child != activeCigObj)
                child.gameObject.SetActive(false);            
        }
    }

    void ActivateAllCig()
    {
        foreach (Transform child in transform)
        {
            Material childMat = child.GetComponent<Renderer>().material;
            childMat.EnableKeyword("_WhiteDegree");
            childMat.SetFloat("_WhiteDegree", 0);
        }
        activateAll = true;
    }

    public void PlayInhaleSound()
    {
        RuntimeManager.PlayOneShot(inhaleEvent, transform.position);
    }

    public void PlayExhaleSound()
    {
        RuntimeManager.PlayOneShot(exhaleEvent, transform.position);
    }

    public void Inhale()
    {
        if (cigStage < 3)
            cigStage++;
        else
            FinishSmoking();
    }

    public void FinishSmoking()
    {
        objType = HandObjectType.TRASH;
        cigStage = Random.Range(4, 7);
        activeCigObj.tag = "CigButt";
        gameObject.tag = "CigButt";
    }
}
