using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using VInspector;
using static Beautify.Universal.Beautify;
using UnityEngine.AI;

public class TattooMesh : MonoBehaviour
{
    CharacterTattoo myTat;
    CharacterTattooMenu myMenu;
    [Foldout("State")]
    public bool dissolving;
    public bool dissolved;
    public float dissolveVal;
    public bool draggable;
    bool dragging;

    [Foldout("Materials")]
    public Material ditherMat;
    public Material burnMat;
    public float ditherVal;
    public bool ditherIn;
    public bool ditherOut;

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
        ditherVal = 1;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        characterTatMesh = myTat.myChar;
        myMenu = transform.parent.parent.parent.GetComponent<CharacterTattooMenu>();
        originalPos = transform.localPosition;
        ditherIn = false;
        ditherOut = false;
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

        if (ditherIn)
        {
            SetDitherMeshIn();
        }

        if (ditherOut)
        {
            SetDitherMeshOut();
        }
    }

    #region Drag Related
    void OnMouseDown()
    {
        if (draggable)
        {
            SwitchBurnMat();
            myMenu.mindPalace.noControl = true;
            myMenu.mindPalace.draggingTat = true;
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
            SwitchDitherMat();
            StartCoroutine(LerpBack());
            myMenu.mindPalace.draggingTat = false;
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
            RuntimeManager.PlayOneShot(dissolveSF, transform.position);
            GetComponent<BoxCollider>().enabled = false;
            myMenu.mindPalace.draggingTat = false;
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

    void SwitchDitherMat()
    {
        foreach (Transform child in transform)
        {

            if (child.GetComponent<Renderer>())
            {
                child.GetComponent<Renderer>().material = ditherMat;
            }

        }
    }

    void SwitchBurnMat()
    {
        Debug.Log("See if we are setting material a lot");
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>())
            {
                child.GetComponent<Renderer>().material = burnMat;
            }

        }
    }

    void DitherOut(Material mat)
    {
        ditherVal -= Time.deltaTime;
        mat.SetFloat("_DitherThreshold", ditherVal);
    }

    void DitherIn(Material mat)
    {
        ditherVal += Time.deltaTime;
        mat.SetFloat("_DitherThreshold", ditherVal);
    }


    void SetDitherMeshOut()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<Renderer>()) {

                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_DitherThreshold");
                ditherVal = childMat.GetFloat("_DitherThreshold");
                if (ditherVal > 0)
                    DitherOut(childMat);
                else
                {
                    ditherVal = 0;
                    childMat.SetFloat("_DitherThreshold", 0);
                    ditherOut = false;
                }
            }

        }
    }

    void SetDitherMeshIn()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>())
            {

                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_DitherThreshold");
                ditherVal = childMat.GetFloat("_DitherThreshold");
                if (ditherVal < 1)
                    DitherIn(childMat);
                else
                {
                    ditherVal = 1;
                    childMat.SetFloat("_DitherThreshold", 1);
                    ditherOut = false;
                }
            }

        }
    }

    public void SetMeshDither(bool dither)
    {
        if (dither)
        {
            ditherIn = false;
            ditherOut = true;
        }
        else
        {
            ditherOut = false;
            ditherIn = true;
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
                    
                    Debug.Log(mat.name);
                    mat.EnableKeyword("_Surface_BurnAmount");
                    mat.SetFloat("_Surface_BurnAmount", dissolveVal);
                }

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
