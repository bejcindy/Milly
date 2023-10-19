using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialActivation : MonoBehaviour
{
    public List<Transform> iniLightObj;
    bool initialOn;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform obj in iniLightObj)
        {
            ActivateAll(obj);
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

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", 0);

    }
}
