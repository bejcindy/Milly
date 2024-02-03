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
    bool draggable;
    Vector3 screenPoint;
    Vector3 offset;

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

    void OnMouseDown()
    {
        if (draggable)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    void OnMouseDrag()
    {
        if (draggable)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
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
        draggable = true;
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
            myTat.dragged = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CharTatMesh"))
        {
            dissolving = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
