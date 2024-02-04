using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.XR;
using VInspector;

public class CharacterTattooMesh : MonoBehaviour
{
    [Foldout("References")]
    public CharacterTattooMenu myMenu;
    public Transform npcMesh;
    public CharacterTattoo currentTat;

    [Foldout("Drag")]
    public bool draggingTattoo;
    public bool rotatingNPC;
    public float rotSpeed;

    [Foldout("Activation")]
    public int stage;
    public Material[] materials;
    public bool stageChanging;
    public float matColorVal;
    bool matChanged;

    void Start()
    {
        myMenu = transform.parent.GetComponent<CharacterTattooMenu>();
        stage = 0;
        matColorVal = 1;
    }


    void Update()
    {
        if (!myMenu.draggingTat && Input.GetMouseButton(0))
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
            ChangeStage();
        }

    }

    public void ChangeMat()
    {
        foreach (Transform child in npcMesh)
        {
            child.GetComponent<Renderer>().material = materials[stage];
            Material childMat = child.GetComponent<Renderer>().material;
        }
        stageChanging = true;
    }

    public void ChangeStage()
    {
        foreach(Transform child in npcMesh)
        {
            Material childMat = child.GetComponent<Renderer>().material;
            childMat.EnableKeyword("_WhiteDegree");
            if (childMat.GetFloat("_WhiteDegree") >0)
                TurnOnColor(childMat);
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
        if (matColorVal > 0)
        {
            matColorVal -= 0.5f * Time.deltaTime;
            material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
            stageChanging = false;
            matChanged = false;
            currentTat.dragged = true;
            currentTat = null;
        }
    }

    public void ReadyCharacterChange(CharacterTattoo tat)
    {
        currentTat = tat;
        matColorVal = 1;
        ChangeMat();
    }
}
