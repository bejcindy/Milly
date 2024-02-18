using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beautify.Universal;
using FMODUnity;

public class MainCharactersMenu : CharacterTattooMenu
{

    

    void Update()
    {
        if (menuOn)
        {
            MindPalace.tatMenuOn = true;
            mindPalace.currentMenu = this;

            if(!mindPalace.noControl && Input.GetKeyDown(KeyCode.Escape))
            {
                mindPalace.SwitchMindPalaceOnOff();
            }
        }

    }



    protected override IEnumerator MenuOnBlink()
    {
        mindPalace.noControl = true;
        MindPalace.hideHint = true;
        float t = 0;
        while (t < blinkDuration)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        if (t >= blinkDuration)
        {
            menuOn = true;
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
        mindPalace.noControl = false;
        mindPalace.SwitchTatMenuBlend();
        MindPalace.showTatHint = true;
        mindPalace.MenuMouseHintOn();
        yield break;
    }

    protected override IEnumerator MenuOffBlink()
    {
        menuOn = false;
        MindPalace.showTatHint = false;
        mindPalace.SwitchPlayerCamBlend();
        mindPalace.noControl = true;
        RuntimeManager.PlayOneShot(blinkSF);
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
        MindPalace.hideHint = false;
        yield break;
    }

}
