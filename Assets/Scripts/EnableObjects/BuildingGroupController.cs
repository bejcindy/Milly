using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroupController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activateAll;
    public bool groupControl;
    public float groupColorVal;
    public float fadeInterval;

    public BuildingGroupController bgc;
    public BuildingGroupController targetBgc;
    void Start()
    {
        groupColorVal = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateAll)
        {
            if (groupColorVal > 0)
                groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            else
            {
                groupColorVal = 0;
                activateAll = false;
            }

            ActivateAll(this.transform);

            if (groupControl)
            {
                if (bgc.activateAll)
                {
                    targetBgc.activateAll = true;
                }
            }
        }
    }

    void ActivateAll(Transform obj)
    {
        if(obj.GetComponent<Renderer>()!= null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            mat.EnableKeyword("_WhiteDegree");
            if(mat.GetFloat("_WhiteDegree") >=0)
                TurnOnColor(mat);
        }
        foreach(Transform child in obj)
        {
            if(child.childCount <= 0 && child.GetComponent<Renderer>()!=null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_WhiteDegree");
                if (childMat.GetFloat("_WhiteDegree") >= 0)
                    TurnOnColor(childMat);
            }
            else
            {
                ActivateAll(child);
            }
        }
    }

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", groupColorVal);

    }
}
