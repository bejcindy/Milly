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

    Collider myCollider;


    string hoverSF = "event:/Sound Effects/Tattoo/HoverObject";

    void Start()
    {
        myMenu = transform.parent.GetComponent<CharacterTattooMenu>();
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
        stage = 0;
        matColorVal = 1;
        ditherVal = 1;
        rotSpeed = 500;
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

        if (ditherOut)
        {
            CharacterDitherOut();
        }

        if (ditherIn)
        {
            CharacterDitherIn();
        }

    }

    public void OnMouseEnter()
    {
        if (selectable && !myMenu.mindPalace.noControl)
        {
            ChangeLayer(9);
            myMenu.mindPalace.selectedMenu = myMenu;
            RuntimeManager.PlayOneShot(hoverSF, transform.position);
        }
    }

    public void OnMouseExit()
    {
        if (selectable)
        {
            if(myMenu.mindPalace.selectedMenu == myMenu)
            {
                myMenu.mindPalace.selectedMenu = null;
            }
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
        if (selectable && !myMenu.mindPalace.noControl)
        {
            ChangeLayer(17);
            DataHolder.HideHint(myMenu.mindPalace.mainMenuHoverHint);
            DataHolder.HideHint(myMenu.mindPalace.mainMenuHint);
            myMenu.SelectMyMenu();
        }
    }

    public void EnableCollider()
    {
        myCollider.enabled = true;
    }

    public void ChangeCharMeshMat()
    {
        if (myNPC)
        {
            myNPC.ChangeCharMeshMat(materials[stage]);
            myNPC.matColorVal = 1;
        }

        foreach (Transform child in npcMesh)
        {
            child.GetComponent<Renderer>().material = materials[stage];
        }
        stageChanging = true;
        if (myNPC)
        {
            myNPC.ColorActualNPC();
        }
    }

    public void ChangeCharMatColor()
    {


        if(matColorVal > 0)
        {
            matColorVal -= 0.5f * Time.deltaTime;
            foreach (Transform child in npcMesh)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_WhiteDegree");
                if (childMat.GetFloat("_WhiteDegree") > 0)
                    SetMatColor(childMat);

            }
        }
        else
        {
            stageChanging = false;
            matColorVal = 0;
            stage++;
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

    public void ChangeLayer(int layerNumber)
    {
        gameObject.layer = layerNumber;
        var children = npcMesh.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.layer = layerNumber;
        }
    }

    void SetMatColor(Material material)
    {

        material.SetFloat("_WhiteDegree", matColorVal);
    }


    public void CharacterFinalChange()
    {
        matColorVal = 1;
        stage = materials.Length - 1;
        if (myNPC)
        {
            myNPC.ChangeCharMeshMat(materials[stage]);
            myNPC.matColorVal = 1;
        }

        foreach (Transform child in npcMesh)
        {
            child.GetComponent<Renderer>().material = materials[stage];
        }
        finalChange = true;
        if(!stageChanging)
            stageChanging = true;

        if (myNPC)
        {
            myNPC.ColorActualNPC();
        }
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

    public void CharacterFrontRotate()
    {
        StartCoroutine(LerpPosition(Vector3.zero, 1f));
        StartCoroutine(LerpRotation(Quaternion.identity, 1f));
    }

    public void CharacterFinishedRotate()
    {
        StartCoroutine(LerpPosition(finalCharPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(finalCharRot), 1f));
    }

    public void SetDither(bool dither)
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

    public void CharacterDitherOut()
    {
        myCollider.enabled = false;
        if(ditherVal > 0)
        {
            ditherVal -= Time.deltaTime;
            foreach (Transform child in npcMesh)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_DitherThreshold");
                childMat.SetFloat("_DitherThreshold", ditherVal);
            }
        }
        else
        {
            ditherVal = 0;
            ditherOut = false;
        }

    }

    public void CharacterDitherIn()
    {
        myCollider.enabled = true;
        if (ditherVal < 1)
        {
            ditherVal += Time.deltaTime;
            foreach (Transform child in npcMesh)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                childMat.EnableKeyword("_DitherThreshold");
                childMat.SetFloat("_DitherThreshold", ditherVal);

            }
        }
        else
        {
            ditherVal = 1;
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
        myMenu.mindPalace.noControl = false;
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
