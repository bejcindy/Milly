using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using TMPro;


public class CharacterTattoo : MonoBehaviour
{
    [Foldout("Components")]
    public TattooMesh tatMesh;
    public TextMeshPro tatText;
    public SpriteRenderer tatSprite;
    

    [Foldout("State")]
    public bool triggered;
    public bool dragged;
    public bool activated;

    [Foldout("References")]
    public CharacterTattooMenu myMenu;
    public CharacterTattooMesh myChar;


    void Start()
    {
        
    }


    void Update()
    {
        if (triggered)
        {
            ActivateTatMesh();
        }

    }

    void ActivateTatMesh()
    {
        tatMesh.ColorTattooMesh();
    }

    void DragTatMesh()
    {

    }


    void FadeOutTatText()
    {

    }
}
