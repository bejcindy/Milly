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
    public bool finalFaded;
    public bool activated;
    public bool draggingTattoo;
    public bool fadeOutText;
    public bool fadeInText;
    public bool isFinalTat;
    bool triggeredOnce;


    [Foldout("References")]

    public CharacterTattooMesh myChar;

    [Foldout("Values")]
    public float tatAlpha;
    public Vector3 finalTatPos;
    public Vector3 finalTatRot;
    public Vector3 finalTatStartPos;
    public Vector3 finalTatStartRot;
    float tatAlphaTarget = 0.2f;
    CharacterTattooMenu myMenu;


    void Start()
    {
        tatAlpha = 1;
        tatMesh = transform.GetChild(0).GetComponent<TattooMesh>();
        tatText = transform.GetChild(1).GetComponent<TextMeshPro>();
        tatSprite = transform.GetChild(2);
        myMenu = transform.parent.parent.GetComponent<CharacterTattooMenu>();

    }


    void Update()
    {
        if (triggered && !triggeredOnce)
        {
            triggeredOnce = true;
            myChar.gameObject.SetActive(true);
            tatMesh.gameObject.SetActive(true);
            tatSprite.gameObject.SetActive(true);
            ActivateTatMesh();
        }

        if (dragged)
        {
            FadeInTatSprite();
        }

        if (fadeOutText)
        {
            FadeOutTatText();
        }

        if (fadeInText)
        {
            FadeInText();
        }


        if (finalFaded)
            FadeOutTatSprite();
        
    }

    void ActivateTatMesh()
    {
        tatMesh.ColorTattooMesh();
        myMenu.TurnOnMenu();
    }

    public void MenuFadeOutText()
    {
        fadeOutText = true;
    }
    public void MenuFadeInText()
    {
        if(!activated)
            fadeInText = true;  
    }


    void FadeOutTatText()
    {
        if (tatText.color.a > 0)
        {
            Color temp = tatText.color;
            temp.a -= 3 * Time.deltaTime;
            tatText.color = temp;
        }
        else
        {
            Color temp = tatText.color;
            temp.a = 0;
            tatText.color = temp;
            fadeOutText = false;
        }
        
    }

    void FadeInText()
    {
        if (tatText.color.a < 0.7f)
        {
            Color temp = tatText.color;
            temp.a += 2 * Time.deltaTime;
            tatText.color = temp;
        }
        else
        {
            Color temp = tatText.color;
            temp.a = 0.7f;
            tatText.color = temp;
            fadeInText = false;
        }
    }

    void FadeInTatSprite()
    {
        Material spriteMat = tatSprite.GetComponent<Renderer>().material;
        spriteMat.EnableKeyword("_AlphaClipThreshold");
        if(tatAlpha > tatAlphaTarget)
        {
            tatAlpha -= 0.2f * Time.deltaTime;
            spriteMat.SetFloat("_AlphaClipThreshold", tatAlpha);
        }
        else
        {
            tatAlpha = 0.2f;
            spriteMat.SetFloat("_AlphaClipThreshold", tatAlpha);
            dragged = false;
            activated = true;
            myMenu.mindPalace.noControl = false;
        }
    }

    void FadeOutTatSprite()
    {
        Material spriteMat = tatSprite.GetComponent<Renderer>().material;
        spriteMat.EnableKeyword("_AlphaClipThreshold");
        if (tatAlpha < 1)
        {
            tatAlpha += 0.5f * Time.deltaTime;
            spriteMat.SetFloat("_AlphaClipThreshold", tatAlpha);
        }
        else
        {
            if (!myMenu.fadeInFinalTat)
                myMenu.fadeInFinalTat = true;
            gameObject.SetActive(false);
        }
    }

    public void FinalTattooFinalTransition()
    {
        StartCoroutine(LerpPosition(finalTatPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(finalTatRot), 1f));    
    }

    public void FinalTatFrontRotate()
    {
        StartCoroutine(LerpPosition(finalTatStartPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(finalTatStartRot), 1f));
    }

    public void FinalTatInMenuRotate()
    {
        StartCoroutine(LerpPosition(finalTatPos, 1f));
        StartCoroutine(LerpRotation(Quaternion.Euler(finalTatRot), 1f));
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
