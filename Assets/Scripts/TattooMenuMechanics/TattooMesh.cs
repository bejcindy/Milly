using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
    string dissolveSF = "event:/Sound Effects/Tattoo/Dissolve";
    string hoverSF= "event:/Sound Effects/Tattoo/HoverObject";

    void Start()
    {
        dissolveVal = 0;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        characterTatMesh = myTat.myChar;
        myMenu = transform.parent.parent.parent.GetComponent<CharacterTattooMenu>();
        originalPos = transform.localPosition;
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

        if (!myMenu.menuOn)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 0;
            }
        }
    }

    #region Drag Related
    void OnMouseDown()
    {
        if (draggable)
        {
            myMenu.mindPalace.noControl = true;
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
            myTat.fadeOutText = true;
            dissolving = true;
            characterTatMesh.ReadyCharacterChange(myTat);
            GetComponent<Collider>().enabled = false;
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
                RuntimeManager.PlayOneShot(hoverSF, transform.position);
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
        Vector3 currentPos = transform.localPosition;
        while (t < lerpBackDuration)
        {
            transform.localPosition = Vector3.Lerp(currentPos, originalPos, t / lerpBackDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
        myMenu.mindPalace.noControl = false;
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

                if (child.GetComponent<Renderer>())
                {
                    Material mat = child.GetComponent<Renderer>().material;
                    mat.EnableKeyword("_WhiteDegree");
                    mat.SetFloat("_WhiteDegree", 0);
                }

            }
            myTat.triggered = false;
            draggable = true;
        }
    }

    public void Dissolve()
    {
        
        if (dissolveVal < 1)
        {
            myMenu.mindPalace.noControl = true;
            dissolveVal += 0.5f * Time.deltaTime;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    Material mat = child.GetComponent<Renderer>().material;
                    mat.EnableKeyword("_BurnAmount");
                    mat.SetFloat("_BurnAmount", dissolveVal);
                }

            }
        }
        else
        {
            myMenu.mindPalace.noControl = false;
            characterTatMesh.draggingTattoo = false;
            dissolving = false;
            dissolved = true;
            RuntimeManager.PlayOneShot(dissolveSF, transform.position);
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
