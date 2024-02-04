using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TattooMesh : MonoBehaviour
{
    CharacterTattoo myTat;
    CharacterTattooMenu myMenu;
    public bool dissolving;
    public bool dissolved;
    public float dissolveVal;
    public bool draggable;
    bool dragging;

    Vector3 screenPoint;
    Vector3 offset;
    Vector3 originalPos;
    bool reachedNPC;
    float lerpBackDuration = .5f;
    CharacterTattooMesh characterTatMesh;

    void Start()
    {
        dissolveVal = 0;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        characterTatMesh = myTat.myChar;
        myMenu = transform.parent.parent.parent.GetComponent<CharacterTattooMenu>();
        originalPos = transform.position;
    }


    void Update()
    {
        if (dissolving)
        {
            Dissolve();
        }

        if (reachedNPC)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 16;
            }
        }
    }

    #region Drag Related
    void OnMouseDown()
    {
        if (draggable)
        {
            dragging = true;
            myMenu.draggedTat = this;
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
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
            if (!reachedNPC)
            {
                characterTatMesh.ChangeLayer(9);
            }
            
        }
    }

    private void OnMouseUp()
    {
        dragging = false;
        if (draggable && !reachedNPC)
        {
            if(myMenu.draggedTat!=null && myMenu.draggedTat == this)
            {
                myMenu.draggedTat = null;
            }
            characterTatMesh.ChangeLayer(0);
            StartCoroutine(LerpBack());
        }
        else if(draggable && reachedNPC)
        {
            if (myMenu.draggedTat != null && myMenu.draggedTat == this)
            {
                myMenu.draggedTat = null;
            }
            myTat.fadeText = true;
            dissolving = true;
            characterTatMesh.ReadyCharacterChange(myTat);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void OnMouseEnter()
    {
        if (draggable)
        {
            if (!characterTatMesh.rotatingNPC && !dragging)
            {
                //characterTatMesh.draggingTattoo = true;
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
            if (!dragging)
            {
                gameObject.layer = 17;
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 17;
                }
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
            myTat.triggered = false;
            draggable = true;
        }
    }

    public void Dissolve()
    {
        
        if (dissolveVal < 1)
        {
            dissolveVal += 0.5f * Time.deltaTime;
            foreach (Transform child in transform)
            {
                Material mat = child.GetComponent<Renderer>().material;
                mat.EnableKeyword("_BurnAmount");
                mat.SetFloat("_BurnAmount", dissolveVal);
            }
        }
        else
        {
            characterTatMesh.draggingTattoo = false;
            dissolving = false;
            dissolved = true;
            characterTatMesh.stage++;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CharTatMesh"))
        {
            characterTatMesh.ChangeLayer(17);
            foreach(Transform child in transform)
            {
                child.gameObject.layer = 16;
            }
            reachedNPC = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CharTatMesh"))
        {
            characterTatMesh.ChangeLayer(9);
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
            reachedNPC = false;
        }
    }
}
