using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class OuterPanelController : MonoBehaviour
{
    public static bool mechanicActivated;
    public bool takeOver;
    public bool zoomIn;
    public TattooPanel currentPanel;
    [Header("Corresponding")]
    //public PannelController[] panels;
    public TattooPanel[] panels;
    public RectTransform[] imgRects;

    public bool exitPanel, enterPanel;

    bool playerUnpaused;
    float mouseDragSpeed = 70f;

    CanvasGroup canvasGroup;
    Animator fade;
    public GameObject blurCanvas;
    public bool noDrag;

    // Start is called before the first frame update
    void Awake()
    {
        //foreach (TattooPanel panel in panels)
        //    panel.characterPanel = this;

        mechanicActivated = false;
        GetComponentInParent<GraphicRaycaster>().enabled = false;
        canvasGroup = GetComponent<CanvasGroup>();
        //exitPanel = true;
        fade = GetComponentInParent<Animator>();
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!exitPanel)
        {
            playerUnpaused = false;

            ReferenceTool.pauseMenu.inTattoo = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponentInParent<GraphicRaycaster>().enabled = true;
            if (!zoomIn)
            {
                //when zoom out child panel & fade in this panel
                canvasGroup.alpha = AlphaBasedOnScale(currentPanel.transform, 1);
                //Debug.Log(AlphaBasedOnScale(currentPanel.transform, 1));
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].activatedOnce)
                        imgRects[i].gameObject.SetActive(true);
                    else
                        imgRects[i].gameObject.SetActive(false);
                }
                RectTransform matchingImg = imgRects[System.Array.IndexOf(panels, currentPanel)];
                if (currentPanel.enabled)
                    GetComponent<RectTransform>().anchoredPosition = currentPanel.GetComponent<RectTransform>().anchoredPosition - matchingImg.anchoredPosition;
                if (currentPanel.transform.localScale.x == .2f)
                {
                    currentPanel.enabled = false;
                    currentPanel.GetComponent<CanvasGroup>().alpha = 0;
                }
                else if (currentPanel.transform.localScale.x > .5f)
                {
                    exitPanel = true;
                }
            }
            else
            {
                //clicked on outer panel icon
                canvasGroup.alpha = FadeOut(canvasGroup.alpha);
                currentPanel.FocusOnTattoo(currentPanel.centerTat.GetComponent<RectTransform>());
                currentPanel.enabled = true;
                currentPanel.panelOn = true;
                if (canvasGroup.alpha == 0)
                    exitPanel = true;
            }
        }
        else
        {
            ReferenceTool.pauseMenu.inTattoo = false;
            GetComponentInParent<GraphicRaycaster>().enabled = false;
            zoomIn = false;
            
            exitPanel = false;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            exitPanel = true;
            currentPanel.panelOn = false;
            currentPanel.enabled = true;
            fade.Play("TattooPanelClose");
            if (!playerUnpaused)
                UnpausePlayer();
        }

        if (Input.GetMouseButton(0) && !noDrag)
        {
            Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * 70f, Input.GetAxis("Mouse Y") * 70f);
            GetComponent<RectTransform>().anchoredPosition += dragAmount;
        }

    }

    float AlphaBasedOnScale(Transform t, float maxA)
    {
        float alpha = Mathf.Lerp(0, maxA, Mathf.InverseLerp(.5f, .2f, t.localScale.x));
        return alpha;
    }


    float FadeOut(float x)
    {
        if (x > 0)
            x -= 1f * Time.deltaTime;
        else
            x = 0;
        return x;
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


