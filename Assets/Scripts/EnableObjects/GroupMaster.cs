using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMaster : MonoBehaviour
{
    public List<Transform> chainTriggers;
    public bool activateAll;
    public float groupColorVal;
    public float fadeInterval;
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
            ActivateAll();
        }
    }

    void ActivateAll()
    {
        foreach(Transform child in chainTriggers)
        {
            if(child.GetComponent<LivableObject>() == null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_WhiteDegree");
                TurnOnColor(childMat);
            }
            else
            {
                if (!child.GetComponent<LivableObject>().activated){
                    Material childMat = child.GetComponent<Renderer>().material;
                    childMat.EnableKeyword("_WhiteDegree");
                    TurnOnColor(childMat);
                }
            }

        }
    }

    void TurnOnColor(Material material)
    {
        if (groupColorVal > 0)
        {
            groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            material.SetFloat("_WhiteDegree", groupColorVal);
        }
        else
        {
            groupColorVal = 0;
        }

    }
}
