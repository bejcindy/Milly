using Beautify.Universal;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using FMODUnity;

public class MindPalace : MonoBehaviour
{
    public static bool tatMenuOn;
    public bool noControl;
    public bool mainMenuOn;
    public CharacterTattooMenu currentMenu;
    public CharacterTattooMenu mainTatMenu;
    public CharacterTattooMenu selectedMenu;

    [HideInInspector]
    public bool draggingTat;

    Camera frontCam;
    Camera colorCam;
    Camera focusCam;

    [TextArea]
    public string regularHint;
    [TextArea]
    public string dragHint;
    [TextArea]
    public string mainMenuHint;

    string changeMenuSF= "event:/Sound Effects/Tattoo/ChangeMenu";

    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = false;
        frontCam = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        colorCam = Camera.main.transform.GetChild(1).GetComponent<Camera>();
        focusCam = Camera.main.transform.GetChild(2).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMenu && currentMenu == mainTatMenu)
        {
            mainMenuOn = true;
        }
        else
        {
            mainMenuOn = false;
        }


        if (Input.GetKeyDown(KeyCode.Tab) && !noControl)
        {
            tatMenuOn = !tatMenuOn;

            if (!tatMenuOn)
            {
                MenuMouseHintOff();
                if (currentMenu)
                    currentMenu.TurnOffMenu();
            }
            else
            {
                if (currentMenu)
                {
                    currentMenu.TurnOnMenu();
                }
            }
        }

        if (tatMenuOn)
        {
            DataHolder.MoveHintToBottom();         
            if (mainMenuOn)
            {
                DataHolder.ShowHint(mainMenuHint);
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(dragHint);
            }
            else if (noControl)
            {
                DataHolder.ShowHint(dragHint);
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(mainMenuHint);
            }
            else
            {
                DataHolder.ShowHint(regularHint);
                DataHolder.HideHint(dragHint);
                DataHolder.HideHint(mainMenuHint);
            }
        }
        else
        {
            DataHolder.MoveHintToTop();
            DataHolder.HideHint(regularHint);
            DataHolder.HideHint(dragHint);
            DataHolder.HideHint(mainMenuHint);
        }
    }

    public void SwitchMainMenuOn()
    {
        mainTatMenu.myCam.m_Priority = 20;
        currentMenu = mainTatMenu;
        mainTatMenu.menuOn = true;
        RuntimeManager.PlayOneShot(changeMenuSF, transform.position);
    }

    public void SwitchMainMenuOff()
    {
        mainTatMenu.menuOn = false;
        mainTatMenu.myCam.m_Priority = 0;
        RuntimeManager.PlayOneShot(changeMenuSF, transform.position);
    }

    public void SelectMenu(CharacterTattooMenu menu)
    {
        currentMenu = menu;
        SwitchMainMenuOff();
        currentMenu.myChar.ChangeLayer(17);
        
    }

    public void MenuMouseHintOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //DataHolder.MoveHintToBottom();
        //DataHolder.ShowHint(hint);

    }

    void MenuMouseHintOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //DataHolder.MoveHintToTop();
        //DataHolder.HideHint(hint);

    }


}
