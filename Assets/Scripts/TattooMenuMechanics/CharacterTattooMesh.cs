using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.XR;
using VInspector;
using FMODUnity;

public class CharacterTattooMesh : MonoBehaviour
{
    CharacterTattooMenu myMenu;

    [Foldout("References")]
    public Transform npcMesh;
    public CharacterTattoo currentTat;
    public NPCControl myNPC;

    [Foldout("Drag")]
    public bool draggingTattoo;
    public bool rotatingNPC;
    public bool selectable;
    public float rotSpeed;

    [Foldout("Activation")]
    public int stage;
    public Material[] materials;
    public bool stageChanging;
    public bool finalChange;
    public bool ditherOut;
    public bool ditherIn;

    [Foldout("Values")]
    public float matColorVal;
    public float ditherVal;
    public Vector3 finalCharPos;
    public Vector3 finalCharRot;
    bool matChanged;

    string hoverSF = "event:/Sound Effects/Tattoo/HoverObject";

    void Start()
    {
        myMenu = transform.parent.GetComponent<CharacterTattooMenu>();
        stage = 0;
        matColorVal = 1;
        ditherVal = 1;
    }


    void Update()
    {
        if (!myMenu.draggingTat && myMenu.menuOn && Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            transform.Rotate(Vector3.up, -rotX);
            rotatingNPC = true;
        }
        else
        {
            rotatingNPC = false;
        }

        if (stageChanging)
        {
            ChangeCharMatColor();
        }

        if (myMenu.mindPalace.mainMenuOn && myMenu.discovered)
        {
            selectable = true;
        }
        else
        {
            selectable = false;
        }

        if (ditherOut || ditherIn)
        {
            CharacterDither();
        }

    }

    public void OnMouseEnter()
    {
        if (selectable)
        {
            ChangeLayer(9);
            RuntimeManager.PlayOneShot(hoverSF, transform.position);
        }
    }

    public void OnMouseExit()
    {
        if (selectable)
        {
            ChangeLayer(17);
        }
    }

    public void OnMouseDown()
    {
        if (selectable)
        {
            ChangeLayer(17);
        }
    }

    public void OnMouseUp()
    {
        if (selectable)
        {
            ChangeLayer(17);
            myMenu.SelectMyMenu();
        }
    }

    public void ChangeCharMeshMat()
    {
        myNPC.ChangeCharMeshMat(materials[stage]);
        myNPC.matColorVal = 1;
        foreach (Transform child in npcMesh)
        {
            child.GetComponent<Renderer>().material = materials[stage];
            Material childMat = child.GetComponent<Renderer>().material;
        }
        stageChanging = true;
    }

    public void ChangeCharMatColor()
    {
        myNPC.ActivateAll(myNPC.npcMesh);
        foreach(Transform child in npcMesh)
        {
            Material childMat = child.GetComponent<Renderer>().material;
            childMat.EnableKeyword("_WhiteDegree");
            if (childMat.GetFloat("_WhiteDegree") >0)
                TurnOnColor(childMat);
            else
            {
                matColorVal = 0;
                stageChanging = false;
                matChanged = false;
                if (finalChange)
                {
                    myMenu.finalTransition = true;
                }
                else
                {
                    currentTat.dragged = true;
                    currentTat = null;
                }
            }
        }

    }

    public void ChangeLayer(int layerNumber)
    {

        gameObject.layer = layerNumber;
        var children = npcMesh.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.layer = layerNumber;
        }
    }

    void TurnOnColor(Material material)
    {
        myMenu.mindPalace.noControl = true;
        if (matColorVal > 0)
        {
            matColorVal -= 0.2f * Time.deltaTime;
            material.SetFloat("_WhiteDegree", matColorVal);
        }

    }


    public void CharacterFinalChange()
    {
        matColorVal = 1;
        stage = materials.Length - 1;
        myNPC.ChangeCharMeshMat(materials[stage]);
        myNPC.matColorVal = 1;
        foreach (Transform child in npcMesh)
        {
            child.GetComponent<Renderer>().material = materials[stage];
            Material childMat = child.GetComponent<Renderer>().material;
        }
        finalChange = true;
        stageChanging = true;
    }

    public void ReadyCharacterChange(CharacterTattoo tat)
    {
        currentTat = tat;
        matColorVal = 1;
        ChangeCharMeshMat();
    }

    public void CharacterFinalTransition()
    {
        StartCoroutine(LerpPosition(finalCharPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(finalCharRot), 1f));
    }

    public void AfterFinalCharFrontRotate()
    {
        StartCoroutine(LerpRotation(Quaternion.identity, 1f));
    }

    public void AfterFinalCharFinishedRotate()
    {
        StartCoroutine(LerpRotation(Quaternion.Euler(finalCharRot), 1f));
    }

    public void SetDither(bool dither)
    {
        if (dither)
        {
            ditherOut = true;
        }
        else
        {
            ditherIn = true;
        }
    }

    public void CharacterDither()
    {
        foreach(Transform child in npcMesh)
        {
            Material childMat = child.GetComponent<Renderer>().material;
            childMat.EnableKeyword("_DitherThreshold");
            if (ditherOut)
                DitherOut(childMat);
            else
                DitherIn(childMat);
        }
    }



    void DitherOut(Material mat)
    {
        if (ditherVal > 0)
        {
            ditherVal -= Time.deltaTime;
            mat.SetFloat("_DitherThreshold", ditherVal);
        }
        else
        {
            ditherOut = false;
            ditherIn = false;
        }
    }

    void DitherIn(Material mat)
    {
        if (ditherVal < 1)
        {
            ditherVal += Time.deltaTime;
            mat.SetFloat("_DitherThreshold", ditherVal);
        }
        else
        {
            ditherOut = false;
            ditherIn = false;
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;

    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endValue;
    }
}
