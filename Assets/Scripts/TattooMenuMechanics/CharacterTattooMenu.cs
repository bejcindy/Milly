using Beautify.Universal;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTattooMenu : MonoBehaviour
{
    public bool menuOn;
    public Transform myTattoos;
    public TattooMesh draggedTat;
    public CharacterTattooMesh myChar;
    public CinemachineVirtualCamera myCam;
    public float blinkDuration = 1f;
    MindPalace mindPalace;
    public bool draggingTat;

    Camera frontCam;
    Camera colorCam;
    Camera focusCam;

    Vector3 tatOnPos;
    Vector3 tatOffPos;

    void Start()
    {
        tatOnPos = Vector3.zero;
        tatOffPos = new Vector3(0, 10, 0);
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
            MindPalace.tatMenuOn = true;
            mindPalace.MenuMouseHintOn();
            mindPalace.currentMenu = this;
        }



    }

    public void TurnOnMenu()
    {
        StartCoroutine(MenuOnBlink());
    }

    public void TurnOffMenu()
    {
        StartCoroutine(MenuOffBlink());
    }


    IEnumerator MenuOnBlink()
    {
        mindPalace.menuMoving = true;
        float t = 0;
        while (t < blinkDuration)
        {
            Debug.Log("t val is " + t);
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        if (t >= blinkDuration)
        {
            myChar.transform.localRotation = Quaternion.identity;
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
        StartCoroutine(LerpPosition(tatOnPos, 0.5f));
        yield break;
    }

    IEnumerator MenuOffBlink()
    {
        mindPalace.menuMoving = true;
        StartCoroutine(LerpPosition(tatOffPos, 0.5f));
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
        mindPalace.menuMoving = false;
        yield break;
    }



    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        if(targetPosition == tatOffPos)
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
            mindPalace.menuMoving = false;
        }
    }

}
