using Beautify.Universal;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using TMPro;

public class CharacterTattooMenu : MonoBehaviour
{
    public MindPalace mindPalace;
    [Foldout("State")]
    public bool menuOn;
    public bool inFinalStage;
    public bool fadeOutAllTats;
    public bool fadeInFinalTat;
    public bool finalTransition;
    public bool finished;
    public bool discovered;
    public bool draggingTat;
    public bool isMainMenu;
    bool finalCalledOnce;

    [Foldout("References")]
    public Transform myTattoos;
    public Transform myInfo;
    public CharacterTattoo finalTattoo;
    public TattooMesh draggedTat;
    public CharacterTattooMesh myChar;
    public CinemachineVirtualCamera myCam;
    public CharacterTattooMenu previousChar;
    public CharacterTattooMenu nextChar;
    public bool leftable;
    public bool rightable;
    public GameObject cursorAnim;

    [Foldout("Values")]
    public float blinkDuration = 1f;
    public float infoFadeVal;
    public string finishedDialogue;
    bool finishedDialogueDone;
    bool fadeinInfo;
    bool fadeOutInfo;

    protected Camera frontCam;
    protected Camera colorCam;
    protected Camera focusCam;

    protected Vector3 tatOnPos;
    protected Vector3 tatOffPos;
    protected string blinkSF = "event:/Sound Effects/Tattoo/Blink";
    protected string charcterFinalSF = "event:/Sound Effects/Tattoo/CharacterFinalChange";
    protected string fadeOutTatSF = "event:/Sound Effects/Tattoo/FadeOutTattoo";
    protected string changeMenuSF = "event:/Sound Effects/Tattoo/ChangeMenu";

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
            myInfo = transform.GetChild(3);
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

        if(previousChar && previousChar.discovered)
        {
            leftable = true;
        }
        if(nextChar && nextChar.discovered)
        {
            rightable = true;
        }

        if (menuOn)
        {
            if(myChar.myNPC)
                myChar.myNPC.colored = true;
            MindPalace.tatMenuOn = true;

            if (!mindPalace.noControl)
            {
                if(leftable && Input.GetKeyDown(KeyCode.A))
                {
                    ChooseMyLeft();

                }

                if(rightable && Input.GetKeyDown(KeyCode.D))
                {
                    ChooseMyRight();

                }
            }


            if (fadeOutAllTats)
            {
                fadeOutAllTats = false;
                mindPalace.noControl = true;
                foreach(Transform t in myTattoos)
                {
                    CharacterTattoo childTat = t.GetComponent<CharacterTattoo>();
                    if(!childTat.isFinalTat)
                        childTat.finalFaded = true;
                }
                RuntimeManager.PlayOneShot(fadeOutTatSF);
            }

            if (fadeInFinalTat && !finalCalledOnce)
            {
                finalCalledOnce = true;
                fadeInFinalTat = false;
                finalTattoo.dragged = true;
                myChar.CharacterFinalChange();
                RuntimeManager.PlayOneShot(charcterFinalSF);
            }

            if (finalTransition)
            {
                finished = true;
                finalTransition = false;
                inFinalStage = false;
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

        if (fadeinInfo)
        {
            TurnOnCharInfo();
        }

        if (fadeOutInfo)
        {
            TurnOffCharInfo();
        }
    }

    public void BlinkMenuOn()
    {
        mindPalace.PausePlayer();
        discovered = true;
        mindPalace.SelectMenu(this);
        StartCoroutine(MenuOnBlink());
    }

    public void BlinkMenuOff()
    {
        if(!isMainMenu)
            SwitchTattoosOff();
        StartCoroutine(MenuOffBlink());
    }

    void SwitchTattoosOn()
    {
        fadeinInfo = true;
        myChar.SetDither(false);
        StartCoroutine(LerpPosition(tatOnPos, 1f));
        foreach (Transform t in myTattoos)
        {
            CharacterTattoo tat = t.GetComponent<CharacterTattoo>();

            if (!tat.isFinalTat)
            {
                if(!tat.activated && !finished)
                {
                    tat.DitherMeshIn();
                    tat.MenuFadeInText();
                }

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
        if (cursorAnim)
            cursorAnim.SetActive(true);
    }

    void SwitchTattoosOff()
    {
        fadeOutInfo = true;
        StartCoroutine(LerpPosition(tatOffPos, 1f));
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
        if (cursorAnim)
            cursorAnim.SetActive(false);
    }

    public void SwitchMainTatMenuOn()
    {
        RuntimeManager.PlayOneShot(changeMenuSF, transform.position);
        mindPalace.SwitchMainMenuOn();
        myChar.CharacterFrontRotate();
        myCam.m_Priority = 0;
    }

    public void SelectMyMenu()
    {
        mindPalace.noControl = true;
        myCam.m_Priority = 20;
        SwitchTattoosOn();
        if (finished)
        {
            myChar.CharacterFinishedRotate();
        }
        else
        {
            myChar.CharacterFrontRotate();
        }

        mindPalace.SelectMenu(this);
        if (cursorAnim)
            cursorAnim.SetActive(true);
    }

    public void DeselectMyMenu()
    {
        mindPalace.noControl = true;
        myCam.m_Priority = 0;
        SwitchTattoosOff();
        myChar.SetDither(true);
        if (cursorAnim)
            cursorAnim.SetActive(false);
    }


    public void ChooseMyLeft()
    {
        DeselectMyMenu();
        previousChar.SelectMyMenu();
    }

    public void ChooseMyRight()
    {
        DeselectMyMenu();
        nextChar.SelectMyMenu();
    }

    public void StartFinalTattoo()
    {
        BlinkMenuOn();
        inFinalStage = true;

    }


    void ArrowButtonsOn()
    {

        if(leftable && rightable)
        {
            mindPalace.LeftChooseUIOn();
            mindPalace.RightChooseUIOn();
        }
        else if (leftable)
        {
            mindPalace.LeftChooseUIOn();

        }
        else if (rightable)
        {
            mindPalace.RightChooseUIOn();
        }
    }


    protected virtual IEnumerator MenuOnBlink()
    {
        
        mindPalace.noControl = true;
        MindPalace.hideHint = true;
        ReferenceTool.playerCinemachine.m_Transitions.m_InheritPosition = false;
        float t = 0;
        while (t < blinkDuration)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        if (t >= blinkDuration)
        {
            if (!finished)
                myChar.transform.localRotation = Quaternion.identity;
            else
                myChar.transform.localRotation = Quaternion.Euler(myChar.finalCharRot);
            if (!isMainMenu)
            {
                myChar.SetDither(false);
                SwitchTattoosOn();
            }

            myCam.m_Priority = 20;
            frontCam.fieldOfView = 35;
            colorCam.fieldOfView = 35;
            focusCam.fieldOfView = 35;
            mindPalace.HandOff();

            RuntimeManager.PlayOneShot(blinkSF);
        }


        while (t >= blinkDuration && t < blinkDuration * 2)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(1, 0, (t - blinkDuration) / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.vignettingBlink.value = 0;
        mindPalace.SwitchTatMenuBlend();
        MindPalace.showTatHint = true;
        mindPalace.firstTriggered = true;
        yield break;
    }

    protected virtual IEnumerator MenuOffBlink()
    {
        myChar.SetDither(true);
        MindPalace.showTatHint = false;
        mindPalace.SwitchPlayerCamBlend();
        mindPalace.noControl = true;
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
            mindPalace.HandOn();
        }


        while (t >= blinkDuration && t < blinkDuration * 2)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(1, 0, (t - blinkDuration) / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.vignettingBlink.value = 0;

        mindPalace.noControl = false;
        MindPalace.hideHint = false;
        ReferenceTool.playerCinemachine.m_Transitions.m_InheritPosition = true;

        mindPalace.FirstMindPalaceDialogue();
        if (finished && !finishedDialogueDone)
        {
            finishedDialogueDone = true;
            StartCompleteDialogue();
        }
        yield break;
    }



    protected virtual IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        mindPalace.noControl = true;
        if (targetPosition == tatOffPos)
        {
            menuOn = false;
            mindPalace.BothChooseUIOff();
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
            ArrowButtonsOn();
        }

        mindPalace.noControl = false;
        if (inFinalStage)
        {
            fadeOutAllTats = true;
        }
    }

    void StartCompleteDialogue()
    {
        DialogueManager.StartConversation(finishedDialogue);
    }

    void TurnOnCharInfo()
    {
        if(infoFadeVal < 1)
        {
            infoFadeVal += Time.deltaTime;
            foreach (Transform t in myInfo)
            {
                SpriteRenderer infoSprite = t.GetComponent<SpriteRenderer>();
                TextMeshPro infoText = t.GetComponent<TextMeshPro>();
                if (infoSprite != null)
                {
                    Color temp = infoSprite.color;
                    temp.a = infoFadeVal;
                    infoSprite.color = temp;
                }

                if(infoText != null)
                {
                    Color temp = infoText.color;
                    temp.a = infoFadeVal;
                    infoText.color = temp;

                }
            }

        }
        else
        {
            fadeinInfo = false;
            infoFadeVal = 1;
        }

    }

    void TurnOffCharInfo()
    {
        if (infoFadeVal > 0)
        {
            infoFadeVal -= Time.deltaTime;
            foreach (Transform t in myInfo)
            {
                SpriteRenderer infoSprite = t.GetComponent<SpriteRenderer>();
                TextMeshPro infoText = t.GetComponent<TextMeshPro>();
                if (infoSprite != null)
                {
                    Color temp = infoSprite.color;
                    temp.a = infoFadeVal;
                    infoSprite.color = temp;
                }

                if (infoText != null)
                {
                    Color temp = infoText.color;
                    temp.a = infoFadeVal;
                    infoText.color = temp;

                }
            }

        }
        else
        {
            fadeOutInfo = false;
            infoFadeVal = 0;
        }

    }



}
