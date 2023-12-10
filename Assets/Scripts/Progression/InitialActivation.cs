using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialActivation : MonoBehaviour
{
    public List<Transform> iniLightObj;
    public List<Transform> iniOffObj;
    bool initialOn;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform obj in iniLightObj)
        {
            ActivateAll(obj);
        }

        foreach(Transform obj in iniOffObj)
        {
            DeactivateAll(obj);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    void ActivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") >= 0)
                    TurnOnColor(mat);
            }

        }

        foreach (Transform child in obj)
        {
            if (child.childCount <= 0 && child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") >= 0)
                        TurnOnColor(childMat);
                }

            }
            else
            {
                ActivateAll(child);
            }


        }

    }

    void DeactivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") < 1)
                    TurnOffColor(mat);
            }

        }

        foreach (Transform child in obj)
        {
            if (child.childCount <= 0 && child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") < 1)
                        TurnOffColor(childMat);
                }

            }
            else
            {
                DeactivateAll(child);
            }


        }

    }

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", 0);

    }

    void TurnOffColor(Material material)
    {
        material.SetFloat("_WhiteDegree", 1);
    }
}
