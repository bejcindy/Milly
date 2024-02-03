using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TattooMesh : MonoBehaviour
{
    CharacterTattoo myTat;
    CharacterTattooMesh tatCharMesh;
    public bool dissolving;
    public bool dissolved;
    public float dissolveVal;

    void Start()
    {
        dissolveVal = 0;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        tatCharMesh = myTat.myChar;
    }


    void Update()
    {
        if (dissolving)
        {
            Dissolve();
        }
    }

    public void ColorTattooMesh()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.layer = 17;
            Material mat = child.GetComponent<Renderer>().material;
            mat.EnableKeyword("_WhiteDegree");
            mat.SetFloat("_WhiteDegree", 0);
        }
    }

    public void Dissolve()
    {
        if(dissolveVal < 1)
        {
            dissolveVal += Time.deltaTime;
            foreach (Transform child in transform)
            {
                Material mat = child.GetComponent<Renderer>().material;
                mat.EnableKeyword("_BurnAmount");
                mat.SetFloat("_BurnAmount", dissolveVal);
            }
        }
        else
        {
            dissolving = false;
            dissolved = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CharTatMesh"))
        {
            dissolving = true;
        }
    }
}
