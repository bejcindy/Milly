using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveActivation : MonoBehaviour
{
    float matColorVal;
    float fadeInterval;
    public bool activated;
    bool hasGroupControl;
    GroupMaster groupControl;

    Material mat;
    void Start()
    {
        matColorVal = 1;
        fadeInterval = 10;
        mat = GetComponent<Renderer>().material;
        if (GetComponent<GroupMaster>() != null)
        {
            hasGroupControl = true;
            groupControl = GetComponent<GroupMaster>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            TurnOnColor(mat);
            if (hasGroupControl)
            {
                groupControl.activateAll = true;
            }
        }
    }

    protected virtual void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
            activated = false;
            this.enabled = false;
        }
    }
}
