using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using System;


public class CharacterTattoo : MonoBehaviour, ISaveSystem
{
    [Foldout("Components")]
    public TattooMesh tatMesh;
    public TextMeshPro tatText;
    public SpriteRenderer tatSprite;


    [Foldout("State")]
    public bool triggered;
    public bool dragged;
    public bool finalFaded;
    public bool activated;
    public bool draggingTattoo;
    public bool fadeOutText;
    public bool fadeInText;
    public bool fadeInSprite;
    public bool fadeOutSprite;
    public bool isFinalTat;
    public bool hiddenTat;
    public bool discovered;


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
        if (!isFinalTat)
        {
            tatMesh = transform.GetChild(0).GetComponent<TattooMesh>();
            tatText = transform.GetChild(1).GetComponent<TextMeshPro>();
            tatSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        }

        myMenu = transform.parent.parent.GetComponent<CharacterTattooMenu>();

    }


    void Update()
    {
        if (triggered && !discovered && !ReferenceTool.playerHolding.inDialogue)
        {
            discovered = true;
            MindPalace.tattoosActivated++;
            myChar.gameObject.SetActive(true);
            tatMesh.gameObject.SetActive(true);
            tatSprite.gameObject.SetActive(true);
            ActivateTatMesh();
        }

        if (dragged)
        {
            FirstFadeInSprite();
        }

        if (fadeOutText)
        {
            FadeOutTatText();
        }

        if (fadeInText)
        {
            FadeInText();
        }

        if (fadeInSprite)
        {
            fadeOutSprite = false;
            FadeInSprite();
        }

        if (fadeOutSprite)
        {
            fadeInSprite = false;
            FadeOutSprite();
        }


        if (finalFaded)
        {
            if(activated)
                FinalFadeOutSprite();
            else
            {
                fadeOutText = true;
                tatMesh.SetMeshDither(true);
                activated = true;
            }
        }

        
    }

    public void TriggerTattoo()
    {
        Invoke(nameof(DelayTrigger), 1f);
    }

    void DelayTrigger()
    {
        triggered = true;
    }

    void ActivateTatMesh()
    {
        tatMesh.ColorTattooMesh();
        if(!hiddenTat)
            myMenu.BlinkMenuOn();
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

    public void DitherMeshIn()
    {
        if(!tatMesh.dissolved && !tatMesh.ditherIn)
            tatMesh.SetMeshDither(false);
    }

    public void DitherMeshOut()
    {
        if(!tatMesh.dissolved && !tatMesh.ditherOut)
            tatMesh.SetMeshDither(true);
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

    public void FirstFadeInSprite()
    {
        if (tatSprite.color.a < 1)
        {
            Color temp = tatSprite.color;
            temp.a += Time.deltaTime;
            tatSprite.color = temp;
        }
        else
        {
            dragged = false;
            activated = true;
            if(!isFinalTat)
                myMenu.mindPalace.noControl = false;
        }

    }

    public void FinalFadeOutSprite()
    {
        if (tatSprite.color.a > 0)
        {
            Color temp = tatSprite.color;
            temp.a -= 0.5f * Time.deltaTime;
            tatSprite.color = temp;
        }
        else
        {
            finalFaded = false;
            if (!myMenu.finalTattoo.dragged)
                myMenu.fadeInFinalTat = true;
        }

    }

    public void MenuFadeOutTatSprite()
    {
        fadeOutSprite = true;
    }

    public void MenuFadeInTatSprite()
    {
        fadeInSprite = true;
    }

    void FadeOutSprite()
    {
        if (tatSprite.color.a > 0)
        {
            Color temp = tatSprite.color;
            temp.a -= Time.deltaTime;
            tatSprite.color = temp;
        }
        else
        {
            Color temp = tatSprite.color;
            temp.a = 0;
            tatSprite.color = temp;
            fadeOutSprite = false;
        }
    }

    void FadeInSprite()
    {
        if (tatSprite.color.a < 1)
        {
            Color temp = tatSprite.color;
            temp.a += Time.deltaTime;
            tatSprite.color = temp;
        }
        else
        {
            Color temp = tatSprite.color;
            temp.a = 1;
            tatSprite.color = temp;
            fadeInSprite = false;
        }
    }

    public void FinalTattooFinalTransition()
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


    public virtual void SaveData(ref GameData data)
    {
        string id = GetComponent<ObjectID>().id;
        if (String.IsNullOrEmpty(id))
            Debug.LogError(gameObject.name + " ID is null.");

        if (data.tattooDict.ContainsKey(id))
            data.tattooDict.Remove(id);
        data.tattooDict.Add(id, new TattooData(discovered, activated));
    }

    public virtual void LoadData(GameData data)
    {
        string id = GetComponent<ObjectID>().id;
        if (String.IsNullOrEmpty(id))
            Debug.LogError(gameObject.name + " ID is null.");

        if(data.tattooDict.TryGetValue(id, out TattooData save))
        {
            discovered = save.isDiscovered;
            activated = save.isActivated;

            if(discovered && !activated)
            {
                tatMesh.LoadColorTattooMesh();
            }
        }
    }
}
