using Beautify.Universal;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using FMODUnity;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class MindPalace : MonoBehaviour
{
    public static bool tatMenuOn;
    public static bool hideHint;
    public static bool showTatHint;
    public bool noControl;
    public bool mainMenuOn;
    public bool firstTriggered;
    public CharacterTattooMenu currentMenu;
    public CharacterTattooMenu mainTatMenu;
    public CharacterTattooMenu selectedMenu;
    public Transform playerHand;

    public List<CharacterTattooMenu> tattooMenuList = new List<CharacterTattooMenu>();

    [HideInInspector]
    public bool draggingTat;

    public CinemachineBlenderSettings playerBlend;
    public CinemachineBlenderSettings tatMenuBlend;
    CinemachineBrain camBrain;
    Camera frontCam;
    Camera colorCam;
    Camera focusCam;

    bool firstThought;

    [TextArea]
    public string regularHint;
    [TextArea]
    public string dragHint;
    [TextArea]
    public string mainMenuHint;

    string changeMenuSF = "event:/Sound Effects/Tattoo/ChangeMenu";
    PlayerLeftHand playerLeftHand;

    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = false;
        frontCam = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        colorCam = Camera.main.transform.GetChild(1).GetComponent<Camera>();
        focusCam = Camera.main.transform.GetChild(2).GetComponent<Camera>();
        camBrain = ReferenceTool.playerBrain;
        GetComponentsInChildren<CharacterTattooMenu>(tattooMenuList);
        playerLeftHand = ReferenceTool.playerLeftHand;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Mind Palace is " + tatMenuOn);
        if(currentMenu && currentMenu == mainTatMenu)
        {
            mainMenuOn = true;
        }
        else
        {
            mainMenuOn = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !noControl && firstTriggered)
        {
            SwitchMindPalaceOnOff();
        }

        if (showTatHint)
        {
            DataHolder.MoveHintToBottom();         
            if (mainMenuOn)
            {
                DataHolder.ShowHint(mainMenuHint);
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(dragHint);
            }
            else if (draggingTat)
            {
                DataHolder.ShowHint(dragHint);
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(mainMenuHint);
            }
            else if(!noControl)
            {
                DataHolder.ShowHint(regularHint);
                DataHolder.HideHint(dragHint);
                DataHolder.HideHint(mainMenuHint);
            }
            else
            {
                DataHolder.HideHint(regularHint);
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

    public void SwitchPlayerCamBlend()
    {
        camBrain.m_CustomBlends = playerBlend;
    }

    public void SwitchTatMenuBlend()
    {
        camBrain.m_CustomBlends = tatMenuBlend;
    }

    public void SwitchMindPalaceOnOff()
    {
        tatMenuOn = !tatMenuOn;

        if (!tatMenuOn)
        {
            UnpausePlayer();
            MenuMouseHintOff();
            if (currentMenu)
                currentMenu.TurnOffMenu();
        }
        else
        {
            PausePlayer();
            if (currentMenu)
            {
                currentMenu.TurnOnMenu();
            }
        }
    }

    public void SwitchMainMenuOn()
    {
        mainTatMenu.myCam.m_Priority = 20;
        foreach (CharacterTattooMenu charMenu in tattooMenuList)
        {
            if (charMenu != mainTatMenu && charMenu != currentMenu)
                charMenu.myChar.SetDither(false);
        }
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
        if (!menu.isMainMenu)
        {
            foreach (CharacterTattooMenu charMenu in tattooMenuList)
            {
                if (charMenu != mainTatMenu && charMenu != currentMenu)
                    charMenu.myChar.SetDither(true);
            }
            SwitchMainMenuOff();
            currentMenu.myChar.ChangeLayer(17);
        }

        
    }

    public void MenuMouseHintOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }

    void MenuMouseHintOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    public void HandOn()
    {
        playerHand.transform.SetParent(Camera.main.transform);
        playerHand.transform.localPosition = Vector3.zero;
        playerHand.transform.localRotation = Quaternion.identity;
    }

    public void HandOff()
    {        
        playerHand.transform.SetParent(null);
        playerLeftHand.HideHandRelatedHints();
        DataHolder.HideHint(DataHolder.hints.soccerHint);
    }

    public void PausePlayer()
    {
        ReferenceTool.playerMovement.enabled = false;
        ReferenceTool.playerLeftHand.bypassThrow = true;
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = false;
        }
    }

    public void UnpausePlayer()
    {
        ReferenceTool.playerMovement.enabled = true;
        ReferenceTool.playerLeftHand.bypassThrow = false;
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = true;
        }
    }

    public void FirstMindPalaceDialogue()
    {
        if (!firstThought)
        {
            firstThought = true;
            DialogueManager.StartConversation("Thoughts/FirstTattooMenu");
        }
    }
}
