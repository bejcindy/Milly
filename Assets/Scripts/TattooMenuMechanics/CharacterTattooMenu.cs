using Beautify.Universal;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class CharacterTattooMenu : MonoBehaviour
{
    [Foldout("State")]
    public bool menuOn;
    public bool fadeOutAllTats;
    public bool fadeInFinalTat;
    public bool finalTransition;
    public bool finished;

    [Foldout("References")]
    public Transform myTattoos;
    public CharacterTattoo finalTattoo;
    public TattooMesh draggedTat;
    public CharacterTattooMesh myChar;
    public CinemachineVirtualCamera myCam;

    [Foldout("Values")]
    public float blinkDuration = 1f;
    public MindPalace mindPalace;
    public bool draggingTat;

    protected Camera frontCam;
    protected Camera colorCam;
    protected Camera focusCam;

    protected Vector3 tatOnPos;
    protected Vector3 tatOffPos;

    protected virtual void Start()
    {
        tatOnPos = Vector3.zero;
        tatOffPos = new Vector3(0, 0, -100);
        mindPalace = transform.parent.GetComponent<MindPalace>();
        menuOn = false;
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
                SwitchMainTatMenuOn();
            }
        }
    }

    public void TurnOnMenu()
    {
        mindPalace.currentMenu = this;
        StartCoroutine(MenuOnBlink());
    }

    public void TurnOffMenu()
    {
        StartCoroutine(MenuOffBlink());
    }

    public void SwitchMainTatMenuOn()
    {
        mindPalace.SwitchMainMenuOn();
        myChar.AfterFinalCharFrontRotate();
        if(finished)
            finalTattoo.FinalTatFrontRotate();
        StartCoroutine(LerpPosition(tatOffPos, 1f));
        myCam.m_Priority = 0;
    }

    public void SelectMyMenu()
    {
        myCam.m_Priority = 20;
        if (finished)
        {
            myChar.AfterFinalCharFinishedRotate();
            finalTattoo.FinalTatInMenuRotate();
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
        if(targetPosition == tatOffPos)
        {
            menuOn = false;
            foreach(Transform t in myTattoos)
            {
                CharacterTattoo tat = t.GetComponent<CharacterTattoo>();
                if (!tat.isFinalTat)
                {
                    tat.MenuFadeOutText();
                }
            }
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
            foreach (Transform t in myTattoos)
            {
                CharacterTattoo tat = t.GetComponent<CharacterTattoo>();
                if (!tat.isFinalTat)
                {
                    tat.MenuFadeInText();
                }
            }
            
            mindPalace.noControl = false;
        }
    }

}
