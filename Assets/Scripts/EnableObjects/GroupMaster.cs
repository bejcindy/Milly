using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMaster : MonoBehaviour
{
    public List<Transform> chainTriggers;
    public bool activateAll;
    public float groupColorVal;
    public float fadeInterval;

    public bool designatedGroup;
    public bool childToChildren;
    public bool buildingControl;
    public bool delayedIgnition;

    public Transform delayedGroup;
    public BuildingGroupController bgc;
    // Start is called before the first frame update
    void Start()
    {
        groupColorVal = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateAll)
        {
            if(groupColorVal > 0)
            {
                groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            }
            else
            {
                groupColorVal = 0;
            }
            ActivateAll();
        }
    }

    void ActivateAll()
    {
        if (designatedGroup)
        {
            foreach (Transform child in chainTriggers)
            {
                ActivateGroup(child);

            }
        }

        if (childToChildren)
        {
            foreach(Transform child in transform.parent)
            {
                if(child.GetComponent<Renderer>()!=null)
                    ActivateGroup(child);
            }
        }

        if (buildingControl)
        {
            bgc.activateAll = true; 
        }

        if (delayedIgnition)
        {
            StartCoroutine(DelayedActivate());
        }
    }

    IEnumerator DelayedActivate()
    {
        foreach(Transform child in chainTriggers)
        {
            child.GetComponent<LivableObject>().activated = true;
            yield return new WaitForSeconds(1.0f);
        }
    }

    void ActivateGroup(Transform child)
    {

        if (child.GetComponent<LivableObject>() == null)
        {
            if(child.GetComponent<Renderer>()!= null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.IsKeywordEnabled("_WhiteDegree"))
                {
                    if (childMat.GetFloat("_WhiteDegree") > 0)
                    {
                        TurnOnColor(childMat);
                    }

                }

            }
        }
        else
        {
            if (!child.GetComponent<LivableObject>().activated)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Material childMat = child.GetComponent<Renderer>().material;
                    if (childMat.IsKeywordEnabled("_WhiteDegree"))
                    {
                        if (childMat.GetFloat("_WhiteDegree") > 0)
                        {
                            TurnOnColor(childMat);
                        }
                    }
                    child.GetComponent<LivableObject>().activated = true;
                }

            }
        }
    }



    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", groupColorVal);

    }
}
