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

    void Start()
    {
        groupColorVal = 1;
    }

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
                enabled = false;
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
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
            }
        }

        if (childToChildren)
        {
            foreach(Transform child in transform.parent)
            {
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
                if (child.GetComponent<Renderer>()!=null && child.GetComponent<SpriteRenderer>() == null)
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
            child.gameObject.layer = 17;
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
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") > 0)
                    {
                        TurnOnColor(childMat);
                    }
                }

                if (child.GetComponent<Renderer>().materials.Length > 1)
                {
                    foreach(Material mat in child.GetComponent<Renderer>().materials)
                    {
                        if (mat.HasProperty("_WhiteDegree")){
                            mat.EnableKeyword("_WhiteDegree");
                            TurnOnColor(mat);
                        }
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
                    if (childMat.HasProperty("_WhiteDegree"))
                    {
                        childMat.EnableKeyword("_WhiteDegree");
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
