using Beautify.Universal;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using FMODUnity;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using VInspector;
using UnityEngine.UI;

public class MindPalace : MonoBehaviour
{
    [Foldout("States")]
    public static int tattoosActivated;
    public int tattoosCount;
    public static bool tatMenuOn;
    public bool tattooMenuOn;
    public static bool hideHint;
    public static bool showTatHint;
    public static bool showedCursorAnimation;
    public bool noControl;
    public bool mainMenuOn;
    public bool firstTriggered;
    public bool iconOn;

    [Foldout("References")]
    public CharacterTattooMenu currentMenu;
    public CharacterTattooMenu mainTatMenu;
    public CharacterTattooMenu selectedMenu;
    public Transform playerHand;

    public List<CharacterTattooMenu> tattooMenuList = new List<CharacterTattooMenu>();
    public GameObject leftChooseUI;
    public GameObject rightChooseUI;
    public GameObject menuIcon;
    float iconAlphaVal;
    bool iconFirstFade;
    [HideInInspector]
    public bool draggingTat;

    public CinemachineBlenderSettings playerBlend;
    public CinemachineBlenderSettings tatMenuBlend;
    CinemachineBrain camBrain;
    PlayerHolding playerHolding;
    bool firstThought;

    [Foldout("Hints")]

    [TextArea]
    public string regularHint;
    [TextArea]
    public string hasLeftHint;
    [TextArea]
    public string hasRightHint;
    [TextArea]
    public string hasBothHint;
    [TextArea]
    public string dragHint;
    [TextArea]
    public string mainMenuHint;
    [TextArea]
    public string mainMenuHoverHint;

    readonly string changeMenuSF = "event:/Sound Effects/Tattoo/ChangeMenu";
    PlayerLeftHand playerLeftHand;

    public GameObject cursorAnimation;
    bool movedToTop;
    bool zeroCamSpeed;

    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = false;
        camBrain = ReferenceTool.playerBrain;
        GetComponentsInChildren<CharacterTattooMenu>(tattooMenuList);
        playerLeftHand = ReferenceTool.playerLeftHand;
        playerHolding = ReferenceTool.playerHolding;
    }

    // Update is called once per frame
    void Update()
    {
        tattooMenuOn = tatMenuOn;
        tattoosCount = tattoosActivated;
        if (currentMenu && currentMenu == mainTatMenu && tatMenuOn)
        {
            mainMenuOn = true;
        }
        else
        {
            mainMenuOn = false;
        }

        if (tatMenuOn && currentMenu.menuOn)
        {
            MenuMouseHintOn();
        }

        if (!playerHolding.inDialogue&&!hideHint)
        {
            ShowIcon();
        }
        else
        {
            HideIcon();
        }

        if (hideHint && !zeroCamSpeed)
        {
            ReferenceTool.playerPOV.m_HorizontalAxis.m_MaxSpeed = 0;
            ReferenceTool.playerPOV.m_VerticalAxis.m_MaxSpeed = 0;
            zeroCamSpeed = true;
        }
        else if (!hideHint && zeroCamSpeed)
        {
            ReferenceTool.playerPOV.m_HorizontalAxis.m_MaxSpeed = 200;
            ReferenceTool.playerPOV.m_VerticalAxis.m_MaxSpeed = 200;
            zeroCamSpeed = false;
        }


        if (Input.GetKeyDown(KeyCode.Tab) && !noControl && firstTriggered && !playerHolding.inDialogue)
        {
            SwitchMindPalaceOnOff();
        }

        if (showTatHint)
        {
            if (!movedToTop)
            {
                DataHolder.MoveHintToTop();
                movedToTop = true;
            }
            if (mainMenuOn)
            {
                if (selectedMenu)
                {
                    DataHolder.ShowHintHorizontal(mainMenuHoverHint);
                    DataHolder.HideHint(mainMenuHint);
                }
                else
                {
                    DataHolder.ShowHintHorizontal(mainMenuHint);
                    DataHolder.HideHint(mainMenuHoverHint);
                }

                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(hasLeftHint);
                DataHolder.HideHint(hasRightHint);
                DataHolder.HideHint(hasBothHint);
                DataHolder.HideHint(dragHint);
            }
            else if (draggingTat)
            {
                DataHolder.ShowHintHorizontal(dragHint);
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(hasLeftHint);
                DataHolder.HideHint(hasRightHint);
                DataHolder.HideHint(hasBothHint);
                DataHolder.HideHint(mainMenuHint);
            }
            else if (!noControl)
            {
                if (currentMenu.leftable && currentMenu.rightable)
                {
                    DataHolder.ShowHintHorizontal(hasBothHint);
                }
                else if (currentMenu.leftable)
                {
                    DataHolder.ShowHintHorizontal(hasLeftHint);
                }
                else if (currentMenu.rightable)
                {
                    DataHolder.ShowHintHorizontal(hasRightHint);
                }
                else
                {
                    DataHolder.ShowHintHorizontal(regularHint);
                }
                DataHolder.HideHint(dragHint);
                DataHolder.HideHint(mainMenuHint);
            }
            else
            {
                DataHolder.HideHint(regularHint);
                DataHolder.HideHint(hasLeftHint);
                DataHolder.HideHint(hasRightHint);
                DataHolder.HideHint(hasBothHint);
                DataHolder.HideHint(dragHint);
                DataHolder.HideHint(mainMenuHint);
                DataHolder.HideHint(mainMenuHoverHint);
            }
        }
        else
        {
            if (movedToTop)
            {
                DataHolder.MoveHintToLower();
                movedToTop = false;
            }
            DataHolder.HideHint(regularHint);
            DataHolder.HideHint(hasLeftHint);
            DataHolder.HideHint(hasRightHint);
            DataHolder.HideHint(hasBothHint);
            DataHolder.HideHint(dragHint);
            DataHolder.HideHint(mainMenuHint);
            DataHolder.HideHint(mainMenuHoverHint);
            DataHolder.HideHint(DataHolder.hints.cigHint);
        }

        if (firstTriggered)
        {
            if (!iconFirstFade)
            {
                if (!playerHolding.inDialogue)
                {
                    Image iconImage = menuIcon.GetComponent<Image>();
                    Color iconColor = iconImage.color;
                    if (iconAlphaVal < 1)
                    {
                        iconAlphaVal += Time.deltaTime;
                        iconColor.a = iconAlphaVal;
                        iconImage.color = iconColor;
                    }
                    else
                    {
                        iconAlphaVal = 1;
                        iconFirstFade = true;
                    }

                }
            }

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
                currentMenu.BlinkMenuOff();
        }
        else
        {
            PausePlayer();
            if (currentMenu)
            {
                currentMenu.BlinkMenuOn();
            }
        }
    }

    public void SwitchMainMenuOn()
    {
        mainTatMenu.myCam.m_Priority = 20;
        foreach (CharacterTattooMenu charMenu in tattooMenuList)
        {
            if (charMenu != mainTatMenu)
            {
                charMenu.myChar.SetDither(false);
                charMenu.myChar.CharacterFrontRotate();
            }

        }
        currentMenu = mainTatMenu;
        mainTatMenu.menuOn = true;
    }

    public void SwitchMainMenuOff()
    {
        mainTatMenu.menuOn = false;
        mainTatMenu.myCam.m_Priority = 0;
    }

    public void SelectMenu(CharacterTattooMenu menu)
    {
        currentMenu = menu;
        if (!menu.isMainMenu)
        {
            foreach (CharacterTattooMenu charMenu in tattooMenuList)
            {
                if (charMenu != mainTatMenu && charMenu != currentMenu)
                    charMenu.DeselectMyMenu();
            }
            SwitchMainMenuOff();
            currentMenu.myChar.ChangeLayer(17);
        }
        RuntimeManager.PlayOneShot(changeMenuSF, transform.position);

    }

    #region UI Region

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

    public void LeftChooseUIOn()
    {
        leftChooseUI.SetActive(true);
    }

    public void RightChooseUIOn()
    {
        rightChooseUI.SetActive(true);
    }

    public void BothChooseUIOn()
    {
        leftChooseUI.SetActive(true);
        rightChooseUI.SetActive(true);
    }

    public void LeftChooseUIOff()
    {
        leftChooseUI.SetActive(false);
    }

    public void RightChooseUIOff()
    {
        rightChooseUI.SetActive(false);
    }

    public void BothChooseUIOff()
    {
        leftChooseUI.SetActive(false);
        rightChooseUI.SetActive(false);
    }

    public void ChooseCurrentMenuLeft()
    {
        currentMenu.ChooseMyLeft();
    }

    public void ChooseCurrentMenuRight()
    {
        currentMenu.ChooseMyRight();
    }

    #endregion



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

    public void ShowIcon()
    {
        menuIcon.SetActive(true);
    }

    public void HideIcon()
    {
        menuIcon.SetActive(false);
    }
}
