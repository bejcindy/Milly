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
    public float ditherVal;
    public bool ditherIn;
    public bool ditherOut;

    Vector3 screenPoint;
    Vector3 offset;
    Vector3 originalPos;
    bool reachedNPC;
    float lerpBackDuration = .5f;
    CharacterTattooMesh characterTatMesh;
    Collider myCollider;
    string dissolveSF = "event:/Sound Effects/Tattoo/Dissolve";
    string hoverSF= "event:/Sound Effects/Tattoo/HoverObject";
    string pressSF = "event:/Sound Effects/Tattoo/PressDown";

    void Start()
    {
        dissolveVal = 0;
        ditherVal = 0;
        myTat = transform.parent.GetComponent<CharacterTattoo>();
        characterTatMesh = myTat.myChar;
        myMenu = transform.parent.parent.parent.GetComponent<CharacterTattooMenu>();
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
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
            if (!draggable)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 0;
                }

            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 17;
                }
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
        if (draggable && !myMenu.mindPalace.noControl)
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
            RuntimeManager.PlayOneShot(pressSF, transform.position);
            StopCoroutine(LerpBack());
        }
    }

    void OnMouseDrag()
    {
        if (dragging)
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
        if (draggable && !myMenu.mindPalace.noControl)
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
            TatMeshMat childMat = child.GetComponent<TatMeshMat>();
            if (childMat)
            {
                childMat.SwitchDitherMat();
            }

        }
    }

    void SwitchBurnMat()
    {
        foreach (Transform child in transform)
        {
            TatMeshMat childMat = child.GetComponent<TatMeshMat>();
            if (childMat)
            {
                childMat.SwitchBurnMat();
            }

        }
    }





    void SetDitherMeshOut()
    {
        myCollider.enabled = false;
        if(ditherVal > 0)
        {
            ditherVal -= Time.deltaTime;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                {

                    Material childMat = child.GetComponent<Renderer>().material;
                    childMat.EnableKeyword("_DitherThreshold");
                    childMat.SetFloat("_DitherThreshold", ditherVal);
                }

            }
        }
        else
        {
            ditherVal = 0;
            ditherOut = false;
        }

    }

    void SetDitherMeshIn()
    {
        if(!dissolved)
            myCollider.enabled = true;
        if(ditherVal < 1)
        {
            ditherVal += Time.deltaTime;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                {

                    Material childMat = child.GetComponent<Renderer>().material;
                    childMat.EnableKeyword("_DitherThreshold");
                    childMat.SetFloat("_DitherThreshold", ditherVal);

                }

            }
        }
        else
        {
            ditherVal = 1;
            ditherOut = false;
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
            myCollider.enabled = false;
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
            if(other.GetComponent<CharacterTattooMesh>() == myMenu.myChar)
            {
                characterTatMesh.ChangeLayer(17);
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = 16;
                }
                reachedNPC = true;
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CharTatMesh"))
        {
            if (other.GetComponent<CharacterTattooMesh>() == myMenu.myChar)
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
}
