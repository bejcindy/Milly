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
    public Transform tatSprite;


    [Foldout("State")]
    public bool triggered;
    public bool dragged;
    public bool activated;
    public bool draggingTattoo;
    public bool fadeText;


    [Foldout("References")]
    public CharacterTattooMenu myMenu;
    public CharacterTattooMesh myChar;

    [Foldout("Values")]
    public float tatAlpha;
    float tatAlphaTarget = 0.2f;
    


    void Start()
    {
        tatAlpha = 1;
        myMenu = transform.parent.parent.GetComponent<CharacterTattooMenu>();
    }


    void Update()
    {
        if (triggered)
        {
            ActivateTatMesh();
        }

        if (dragged)
        {
            FadeInTatSprite();
        }

        if (fadeText)
        {
            FadeOutTatText();
        }
        
    }

    void ActivateTatMesh()
    {
        tatMesh.ColorTattooMesh();
    }


    void FadeOutTatText()
    {
        if (tatText.color.a > 0)
        {
            Color temp = tatText.color;
            temp.a -= 1 * Time.deltaTime;
            tatText.color = temp;
        }
        else
        {
            fadeText = false;
        }
        
    }

    void FadeInTatSprite()
    {
        Material spriteMat = tatSprite.GetComponent<Renderer>().material;
        spriteMat.EnableKeyword("_AlphaClipThreshold");
        if(tatAlpha > tatAlphaTarget)
        {
            tatAlpha -= 0.5f * Time.deltaTime;
            spriteMat.SetFloat("_AlphaClipThreshold", tatAlpha);
        }
        else
        {
            dragged = false;
            activated = true;
        }
    }
}
