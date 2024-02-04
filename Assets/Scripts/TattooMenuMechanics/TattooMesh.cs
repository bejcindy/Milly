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
    public bool draggable;

    Vector3 screenPoint;
    Vector3 offset;
    Vector3 originalPos;
    bool reachedNPC;
    float lerpBackDuration = .5f;

    public GameObject NPCMesh;
    Renderer[] npcMeshChildren;
    CharacterTattooMesh characterTatMesh;

    void Start()
    {
        dissolveVal = 0;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        tatCharMesh = myTat.myChar;
        originalPos = transform.position;
        NPCMesh = GetComponentInParent<CharacterTattoo>().myChar.gameObject;
        npcMeshChildren = NPCMesh.GetComponentsInChildren<Renderer>();
        characterTatMesh = NPCMesh.GetComponent<CharacterTattooMesh>();
    }


    void Update()
    {
        if (dissolving)
        {
            Dissolve();
        }
    }

    #region Drag Related
    void OnMouseDown()
    {
        if (draggable)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            gameObject.layer = 17;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
            StopCoroutine(LerpBack());
        }
    }

    void OnMouseDrag()
    {
        if (draggable)
        {
            characterTatMesh.draggingTattoo = true;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
            if (!reachedNPC)
            {
                foreach (Renderer child in npcMeshChildren)
                {
                    child.gameObject.layer = 9;
                }
            }
            
        }
    }

    private void OnMouseUp()
    {
        if (draggable && !reachedNPC)
        {
            
            foreach (Renderer child in npcMeshChildren)
            {
                child.gameObject.layer = 0;
            }
            StartCoroutine(LerpBack());
        }
    }
    private void OnMouseEnter()
    {
        if (draggable)
        {
            if (!characterTatMesh.rotatingNPC)
            {
                characterTatMesh.draggingTattoo = true;
                gameObject.layer = 9;
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 9;
                }
            }
        }
    }
    private void OnMouseExit()
    {
        if (draggable)
        {
            characterTatMesh.draggingTattoo = false;
            gameObject.layer = 17;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
        }
    }
    

    IEnumerator LerpBack()
    {
        float t = 0;
        Vector3 currentPos = transform.position;
        while (t < lerpBackDuration)
        {
            transform.position = Vector3.Lerp(currentPos, originalPos, t / lerpBackDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
        yield break;
    }
    #endregion

    public void ColorTattooMesh()
    {
        if (!draggable)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
                Material mat = child.GetComponent<Renderer>().material;
                mat.EnableKeyword("_WhiteDegree");
                mat.SetFloat("_WhiteDegree", 0);
            }
            draggable = true;
        }
    }

    public void Dissolve()
    {
        
        if (dissolveVal < 1)
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
            foreach (Renderer child in npcMeshChildren)
            {
                child.gameObject.layer = 17;
            }
            reachedNPC = true;
        }
    }
}
