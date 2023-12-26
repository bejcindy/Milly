using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class OuterPanelController : MonoBehaviour
{
    public static bool mechanicActivated;
    public bool takeOver;
    public bool zoomIn;
    public PannelController currentPanel;
    [Header("Corresponding")]
    public PannelController[] panels;
    public RectTransform[] imgRects;
    public Image panelBG;
    [SerializeField] Material blurMaterial;
    Image[] childImgs;
    bool resetChild;
    public bool exitPanel,enterPanel;
    bool fadingColor;
    bool playerUnpaused;
    float mouseDragSpeed = 70f;
    float fadeSpeed = 1;
    float BGAlpha = .25f;
    bool tookOver;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (PannelController panel in panels)
            panel.parentControl = this;
        childImgs = GetComponentsInChildren<Image>();
        foreach (Image img in childImgs)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        mechanicActivated = false;
        GetComponentInParent<GraphicRaycaster>().enabled = false;
        //exitPanel = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPanel)
        {
            foreach(PannelController pc in panels)
            {
                if (pc != currentPanel && pc.enabled)
                    pc.enabled = false;
            }
        }
        if (takeOver && currentPanel && !zoomIn)
        {
            //tookOver = true;
            playerUnpaused = false;
            //when zoom out child panel & fade in this panel
            //foreach (Image img in childImgs)
            //    img.color = AlphaBasedOnScale(img.color, currentPanel.transform, 1);
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].activatedOnce)
                {
                    imgRects[i].GetComponent<Image>().color = AlphaBasedOnScale(imgRects[i].GetComponent<Image>().color, currentPanel.transform, 1);
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = true;
                }
                else
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = false;
            }
            RectTransform matchingImg = imgRects[System.Array.IndexOf(panels, currentPanel)];
            if (currentPanel.enabled)
                GetComponent<RectTransform>().anchoredPosition = currentPanel.GetComponent<RectTransform>().anchoredPosition - matchingImg.anchoredPosition;
            ReferenceTool.pauseMenu.inTattoo = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponentInParent<GraphicRaycaster>().enabled = true;
            if (currentPanel.transform.localScale.x == .2f)
            {
                currentPanel.enabled = false;
                currentPanel.activated = false;
                currentPanel = null;
                exitPanel = false;
            }
            resetChild = false;
        }
        else if (takeOver && currentPanel && zoomIn)
        {
            tookOver = true;
            playerUnpaused = false;
            if (!resetChild)
            {
                RectTransform matchingImg = imgRects[System.Array.IndexOf(panels, currentPanel)];
                currentPanel.transform.localScale = new Vector2(.2f, .2f);
                currentPanel.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + matchingImg.anchoredPosition;
                currentPanel.enabled = true;
                currentPanel.activated = true;
                currentPanel.noDrag = true;
                resetChild = true;
                ReferenceTool.pauseMenu.inTattoo = false;
                GetComponentInParent<GraphicRaycaster>().enabled = false;
            }
            //foreach (Image img in childImgs)
            //    img.color = AlphaBasedOnScale(img.color, currentPanel.transform, 1);
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].activatedOnce)
                {
                    imgRects[i].GetComponent<Image>().color = AlphaBasedOnScale(imgRects[i].GetComponent<Image>().color, currentPanel.transform, 1);
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = true;
                }
                else
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = false;
            }
            currentPanel.transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + .1f * Time.deltaTime, .2f, 1f), Mathf.Clamp(transform.localScale.y + .1f * Time.deltaTime, .2f, 1f));
            if (currentPanel.transform.localScale.x == 1f)
            {
                currentPanel.noDrag = false;
                zoomIn = false;
                takeOver = false;
                exitPanel = false;
            }
        }
        else if (exitPanel)
        {
            if (!playerUnpaused)
                UnpausePlayer();
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
                if (img.GetComponent<OuterPanelButton>())
                    img.GetComponent<OuterPanelButton>().enabled = false;
            }
            panelBG.color = FadeOutColor(panelBG.color);
            FadeOutBlur();
            ReferenceTool.pauseMenu.inTattoo = false;
            GetComponentInParent<GraphicRaycaster>().enabled = false;
            DataHolder.HideHint(DataHolder.hints.tattooViewHint);
            if (AllImgFadedOut())
                exitPanel = false;
        }
        else if(!takeOver)
        {
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
                if (img.GetComponent<OuterPanelButton>())
                    img.GetComponent<OuterPanelButton>().enabled = false;
            }
            ReferenceTool.pauseMenu.inTattoo = false;
            //GetComponentInParent<GraphicRaycaster>().enabled = false;
        }

        
        //else
        //{
        //    foreach (Image img in childImgs)
        //    {
        //        img.color = FadeOutColor(img.color);
        //        if (img.GetComponent<OuterPanelButton>())
        //            img.GetComponent<OuterPanelButton>().enabled = false;
        //    }
        //    ReferenceTool.pauseMenu.inTattoo = false;
        //    //GetComponentInParent<GraphicRaycaster>().enabled = false;
        //}
        if (takeOver)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(!exitPanel)
                    exitPanel = true;
            }
            
            if (Input.GetMouseButton(0))
            {
                Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * 70f, Input.GetAxis("Mouse Y") * 70f);
                GetComponent<RectTransform>().anchoredPosition += dragAmount;
            }

        }
        //if (mechanicActivated)
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //    {
        //        if (!exitPanel)
        //        {
        //            exitPanel = true;
        //            enterPanel = false;
        //        }
        //        else if (!ReferenceTool.playerHolding.inDialogue)
        //        {
        //            enterPanel = true;
        //            exitPanel = false;
        //        }
        //    }
        //}
    }
    bool AllImgFadedOut()
    {
        foreach(Image img in childImgs)
        {
            if (img.color.a != 0)
                return false;
        }
        return true;
    }

    Color AlphaBasedOnScale(Color c, Transform t, float maxA)
    {
        float alpha = Mathf.Lerp(0, maxA, Mathf.InverseLerp(.5f, .2f, t.localScale.x));
        c = new Color(c.r, c.g, c.b, alpha);
        return c;
    }
    Color FadeInColor(Color c, float targetAlpha)
    {

        if (c.a < targetAlpha)
            c += new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, targetAlpha);
        return c;
    }

    Color FadeOutColor(Color c)
    {

        if (c.a > 0)
            c -= new Color(0, 0, 0, 1f) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, 0);
        return c;
    }
    void FadeOutBlur()
    {
        if (blurMaterial.GetFloat("_Alpha") != 0)
        {
            float timeElapsed = 0;
            while (timeElapsed < 2f)
            {
                float a = Mathf.Lerp(1, 0, timeElapsed / 1.5f);
                blurMaterial.SetFloat("_Alpha", a);
                timeElapsed += Time.deltaTime;
            }
        }
        blurMaterial.SetFloat("_Alpha", 0);
    }
    void FadeInBlur()
    {
        if (blurMaterial.GetFloat("_Alpha") != 1)
        {
            float timeElapsed = 0;
            while (timeElapsed < 2f)
            {
                float a = Mathf.Lerp(0, 1, timeElapsed / 1.5f);
                blurMaterial.SetFloat("_Alpha", a);
                timeElapsed += Time.deltaTime;
            }
        }
        blurMaterial.SetFloat("_Alpha", 1);
    }
    public void PausePlayer()
    {
        playerUnpaused = false;
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


