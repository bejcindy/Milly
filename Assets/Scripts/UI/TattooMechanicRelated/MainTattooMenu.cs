using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
public class MainTattooMenu : MonoBehaviour
{
    public TattooPanel activePanel;
    public CharacterPanel characterPanel;
    public bool showPanel;
    public bool playerUnpaused;
    Animator menuFade;
    float mouseDragSpeed = 70;


    bool playAnim;
    bool firstActivated;
    // Start is called before the first frame update
    void Start()
    {
        menuFade = GetComponent<Animator>();
        playAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (showPanel)
        {
            firstActivated = true;
            if (playAnim)
            {
                menuFade.SetTrigger("FadeIn");
                playAnim = false;
            }
            PausePlayer();

        }


        if (Input.GetKeyDown(KeyCode.Tab) && firstActivated)
        {
            if (showPanel)
            {
                menuFade.SetTrigger("FadeOut");
                UnpausePlayer();
                showPanel = false;
                playAnim = true;
            }
            else
            {
                showPanel = true;

            }
        }
    }

    public void TurnOffActivePanel()
    {
        activePanel.panelOn = false;
        activePanel.gameObject.SetActive(false);
    }

    public void TurnOnActivePanel()
    {
        activePanel.panelOn = true;
        activePanel.transform.localScale = new Vector2(1, 1);
        activePanel.gameObject.SetActive(true);
        activePanel.ResetPosition();
        activePanel.MakePanelVisible();
    }

    public void ChangePanel()
    {
        activePanel.panelOn = false;
        activePanel.gameObject.SetActive(false);
        activePanel = characterPanel;
    }

    public void ChoosePanel(TattooPanel panel)
    {

    }

    public void PausePlayer()
    {
        playerUnpaused = false;
        DataHolder.ShowHint(DataHolder.hints.tattooViewHint);
        ReferenceTool.playerLeftHand.bypassThrow = true;
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = false;
        }
        ReferenceTool.playerMovement.enabled = false;
        CinemachineVirtualCamera currentCam = ReferenceTool.playerBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (currentCam.GetCinemachineComponent<CinemachinePOV>())
        {
            if (Input.GetMouseButton(0))
            {
                currentCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseDragSpeed * .1f;
                currentCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseDragSpeed * .1f;
            }
            else
            {
                currentCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
                currentCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
            }
        }
        GetComponentInParent<GraphicRaycaster>().enabled = true;
    }

    public void UnpausePlayer()
    {
        playerUnpaused = true;
        DataHolder.HideHint(DataHolder.hints.tattooViewHint);
        ReferenceTool.playerLeftHand.bypassThrow = false;
        CinemachineVirtualCamera currentCam = ReferenceTool.playerBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = true;
        }
        ReferenceTool.playerMovement.enabled = true;
        if (currentCam.GetCinemachineComponent<CinemachinePOV>())
        {
            currentCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
            currentCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        }
        GetComponentInParent<GraphicRaycaster>().enabled = false;
    }
}
