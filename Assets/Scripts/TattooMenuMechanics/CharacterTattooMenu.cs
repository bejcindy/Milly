using Beautify.Universal;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using FMODUnity;

public class CharacterTattooMenu : MonoBehaviour
{
    public MindPalace mindPalace;
    [Foldout("State")]
    public bool menuOn;
    public bool fadeOutAllTats;
    public bool fadeInFinalTat;
    public bool finalTransition;
    public bool finished;
    public bool discovered;
    public bool draggingTat;
    public bool isMainMenu;

    [Foldout("References")]
    public Transform myTattoos;
    public CharacterTattoo finalTattoo;
    public TattooMesh draggedTat;
    public CharacterTattooMesh myChar;
    public CinemachineVirtualCamera myCam;

    [Foldout("Values")]
    public float blinkDuration = 1f;


    protected Camera frontCam;
    protected Camera colorCam;
    protected Camera focusCam;

    protected Vector3 tatOnPos;
    protected Vector3 tatOffPos;
    protected string blinkSF = "event:/Sound Effects/Tattoo/Blink";

    protected virtual void Start()
    {
        tatOnPos = Vector3.zero;
        tatOffPos = new Vector3(0, 0, -5);
        mindPalace = transform.parent.GetComponent<MindPalace>();
        menuOn = false;
        if (!isMainMenu)
        {
            myCam = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            myChar = transform.GetChild(1).GetComponent<CharacterTattooMesh>();
            myTattoos = transform.GetChild(2);
        }
        frontCam = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        colorCam = Camera.main.transform.GetChild(1).GetComponent<Camera>();
        focusCam = Camera.main.transform.GetChild(2).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(draggedTat != null)
        {
            draggingTat = true;
        }
        else
        {
            draggingTat=false;
        }

        if (menuOn)
        {
            myChar.myNPC.colored = true;
            MindPalace.tatMenuOn = true;
            mindPalace.MenuMouseHintOn();


            if (fadeOutAllTats)
            {
                fadeOutAllTats = false;
                foreach(Transform t in myTattoos)
                {
                    CharacterTattoo childTat = t.GetComponent<CharacterTattoo>();
                    if(!childTat.isFinalTat)
                        childTat.finalFaded = true;
                }

            }

            if (fadeInFinalTat)
            {
                fadeInFinalTat = false;
                finalTattoo.gameObject.SetActive(true);
                finalTattoo.dragged = true;
                myChar.CharacterFinalChange();
            }

            if (finalTransition)
            {
                finished = true;
                finalTransition = false;
                myChar.CharacterFinalTransition();
                finalTattoo.FinalTattooFinalTransition();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !mindPalace.noControl)
            {
                SwitchTattoosOff();
                StopCoroutine(LerpPosition(tatOnPos, 1f));
                SwitchMainTatMenuOn();
            }
        }
    }

    public void TurnOnMenu()
    {
        discovered = true;
        mindPalace.currentMenu = this;
        mindPalace.SelectMenu(this);
        if (!isMainMenu)
        {
            myChar.SetDither(false);
            SwitchTattoosOn();
        }
        StartCoroutine(MenuOnBlink());
    }

    public void TurnOffMenu()
    {
        if(!isMainMenu)
            SwitchTattoosOff();
        StartCoroutine(MenuOffBlink());
    }

    void SwitchTattoosOn()
    {
        foreach (Transform t in myTattoos)
        {
            CharacterTattoo tat = t.GetComponent<CharacterTattoo>();

            if (!tat.isFinalTat)
            {
                tat.DitherMeshIn();
                tat.MenuFadeInText();
                if (tat.activated && !finished)
                {
                    tat.MenuFadeInTatSprite();
                }
            }
            else
            {
                if (finished)
                {
                    tat.MenuFadeInTatSprite();
                }
            }


        }
    }

    void SwitchTattoosOff()
    {
        foreach (Transform t in myTattoos)
        {
            CharacterTattoo tat = t.GetComponent<CharacterTattoo>();

            if (!tat.isFinalTat)
            {
                tat.DitherMeshOut();
                tat.MenuFadeOutText();

            }
            tat.MenuFadeOutTatSprite();
        }

    }

    public void SwitchMainTatMenuOn()
    {
        mindPalace.SwitchMainMenuOn();
        myChar.AfterFinalCharFrontRotate();
        //if(finished)
        //    finalTattoo.FinalTatFrontRotate();
        StartCoroutine(LerpPosition(tatOffPos, 1f));
        myCam.m_Priority = 0;
    }

    public void SelectMyMenu()
    {
        mindPalace.noControl = true;
        myCam.m_Priority = 20;
        SwitchTattoosOn();
        if (finished)
        {
            myChar.AfterFinalCharFinishedRotate();
        }

        StartCoroutine(LerpPosition(tatOnPos, 1f));
        mindPalace.SelectMenu(this);

    }

    public void StartFinalTattoo()
    {
        TurnOnMenu();
        fadeOutAllTats = true;

    }


    protected virtual IEnumerator MenuOnBlink()
    {
        mindPalace.noControl = true;
        float t = 0;
        while (t < blinkDuration)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        if (t >= blinkDuration)
        {
            StartCoroutine(LerpPosition(tatOnPos, 1f));
            if (!finished)
                myChar.transform.localRotation = Quaternion.identity;
            else
                myChar.transform.localRotation = Quaternion.Euler(myChar.finalCharRot);
            myCam.m_Priority = 20;
            frontCam.fieldOfView = 35;
            colorCam.fieldOfView = 35;
            focusCam.fieldOfView = 35;
        }
        RuntimeManager.PlayOneShot(blinkSF);

        while (t >= blinkDuration && t < blinkDuration * 2)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(1, 0, (t - blinkDuration) / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.vignettingBlink.value = 0; 

        yield break;
    }

    protected virtual IEnumerator MenuOffBlink()
    {
        mindPalace.noControl = true;
        StartCoroutine(LerpPosition(tatOffPos, 1f));
        float t = 0;
        RuntimeManager.PlayOneShot(blinkSF);
        while (t < blinkDuration)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        if (t >= blinkDuration)
        {
            myCam.m_Priority = 0;
            frontCam.fieldOfView = 60;
            colorCam.fieldOfView = 60;
            focusCam.fieldOfView = 60;
        }


        while (t >= blinkDuration && t < blinkDuration * 2)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(1, 0, (t - blinkDuration) / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.vignettingBlink.value = 0;
        mindPalace.noControl = false;
        yield break;
    }



    protected virtual IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        mindPalace.noControl = true;
        if (targetPosition == tatOffPos)
        {
            menuOn = false;
        }

        float time = 0;
        Vector3 startPosition = myTattoos.localPosition;
        while (time < duration)
        {
            myTattoos.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        myTattoos.localPosition = targetPosition;

        if(targetPosition == tatOnPos)
        {
            menuOn = true;
            SwitchTattoosOn();
        }

        mindPalace.noControl = false;
    }



}
