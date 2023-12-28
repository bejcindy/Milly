using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using PixelCrushers.DialogueSystem;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using UnityEngine.Rendering;
using UnityEditor;
using TMPro;
//using UnityEditor.VersionControl;

public class PannelController : MonoBehaviour
{
    public bool activatedOnce;
    public bool activated;
    public float fadeSpeed;
    public RectTransform CanvasRect;
    public Image pannelBG;
    float BGAlpha = .25f;
    public RectTransform currentTattoo;
    public float mouseDragSpeed, scrollSizeSpeed;

    Image[] childImgs;
    public UILineRendererList blackLine, greyLine;
    public bool noDrag;
    bool gotPos, firstActivated;
    bool lerping;
    float timer;
    bool playerUnpaused;
    RectTransform previousTattoo;
    public Image centerTattoo;
    Image simpleCenterTattoo;
    //[SerializeField] CinemachineVirtualCamera playerCamera;
    //[SerializeField] CinemachineVirtualCamera catCamera;
    bool catActivated;

    bool clearedCurrent;
    bool fadingColor;
    [SerializeField] Material blurMaterial;
    TextMeshProUGUI[] childTexts;
    public OuterPanelController parentControl;
    [SerializeField] string transitionStage;
    Color referenceColor;

    private void Awake()
    {
        childImgs = GetComponentsInChildren<Image>();
        foreach (Image img in childImgs)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

        childTexts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI tmp in childTexts)
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);

        blackLine.color = new Color(blackLine.color.r, blackLine.color.g, blackLine.color.b, 0);
        greyLine.color = new Color(greyLine.color.r, greyLine.color.g, greyLine.color.b, 0);
        pannelBG.color = new Color(pannelBG.color.r, pannelBG.color.g, pannelBG.color.b, 0);
        clearedCurrent = true;
        blurMaterial.SetFloat("_Alpha", 0);
        referenceColor = new Color(0, 0, 0, 0);
        transitionStage = "SetCenterImgPos";
        //    Debug.Log(centerTattoo.transform.GetChild(0).GetComponent<Image>().color);
        simpleCenterTattoo = centerTattoo.transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (activated)
        {
            PausePlayer();
            //PixelCrushers.UIPanel.monitorSelection = false; // Don't allow dialogue UI to steal back input focus.
            //PixelCrushers.DialogueSystem.DialogueManager.Pause(); // Stop DS timers (e.g., sequencer commands).
            parentControl.currentPanel = this;
            clearedCurrent = false;
            DataHolder.ShowHint(DataHolder.hints.tattooViewHint);
            if (!OuterPanelController.mechanicActivated)
                OuterPanelController.mechanicActivated = true;
            if (!activatedOnce)
                activatedOnce = true;
            if (!parentControl.takeOver)
            {
                if (currentTattoo)
                {
                    switch (transitionStage)
                    {
                        case "SetCenterImgPos":
                            GetComponent<RectTransform>().anchoredPosition = -centerTattoo.GetComponent<RectTransform>().anchoredPosition;
                            currentTattoo.GetComponent<TattooConnection>().activated = true;
                            foreach (RectTransform relate in currentTattoo.GetComponent<TattooConnection>().relatedTattoos)
                            {
                                if (!relate.GetComponent<TattooConnection>().hidden)
                                    relate.GetComponent<TattooConnection>().related = true;
                            }
                            foreach (RectTransform relate in centerTattoo.GetComponent<TattooConnection>().relatedTattoos)
                            {
                                if (!relate.GetComponent<TattooConnection>().hidden)
                                    relate.GetComponent<TattooConnection>().related = true;
                            }
                            //Calculate CenterImg Lines
                            GetComponent<ConnectionManager>().ActivateLines(centerTattoo.GetComponent<TattooConnection>());
                            GetComponent<ConnectionManager>().ActivateLines(currentTattoo.GetComponent<TattooConnection>());
                            transform.localScale = new Vector2(1, 1);
                            previousTattoo = currentTattoo;
                            gotPos = true;
                            noDrag = true;

                            transitionStage = "FadeInBG";
                            break;

                        case "FadeInBG":
                            pannelBG.color = FadeInColor(pannelBG.color, BGAlpha);
                            FadeInBlur();
                            if (pannelBG.color.a == BGAlpha && blurMaterial.GetFloat("_Alpha") == 1)
                                transitionStage = "FadeInCenterImg";
                            break;

                        case "FadeInCenterImg":
                            //考虑给tattooconnection上单写一个function管centertattoo
                            if (!centerTattoo.GetComponent<TattooConnection>().activated)
                            {
                                simpleCenterTattoo.color = FadeInColor(simpleCenterTattoo.color, 1);
                                centerTattoo.GetComponent<TattooConnection>().related = true;

                                if (simpleCenterTattoo.color.a == 1)
                                    transitionStage = "ShowBlackLines";
                            }
                            else
                            {
                                centerTattoo.GetComponent<Image>().color = FadeInColor(centerTattoo.GetComponent<Image>().color, 1);

                                if (centerTattoo.GetComponent<Image>().color.a == 1)
                                    transitionStage = "ShowBlackLines";
                            }
                            break;

                        case "ShowBlackLines":
                            blackLine.color = FadeInColor(blackLine.color, 1);

                            if (blackLine.color.a == 1)
                            {
                                if (currentTattoo.transform != centerTattoo.transform)
                                    transitionStage = "LerpToCurrentImgPos";
                                else
                                    transitionStage = "ShowGreyLines";
                            }
                            break;

                        case "LerpToCurrentImgPos":
                            if (!lerping)
                            {
                                if (GetComponent<RectTransform>().anchoredPosition != -currentTattoo.anchoredPosition)
                                    StartCoroutine(LerpPosition(-currentTattoo.anchoredPosition, 1f));
                                else
                                    transitionStage = "FadeInCurrentImg";
                            }
                            break;

                        case "FadeInCurrentImg":
                            currentTattoo.GetComponent<Image>().color = FadeInColor(currentTattoo.GetComponent<Image>().color, 1);
                            if (currentTattoo.GetComponent<Image>().color.a == 1)
                                transitionStage = "ShowGreyLines";
                            break;

                        case "ShowGreyLines":
                            greyLine.color = FadeInColor(greyLine.color, .5f);
                            if (greyLine.color.a == .5f)
                            {
                                transitionStage = "ShowRelatedImgs";
                            }
                            break;

                        case "ShowRelatedImgs":
                            foreach (Image img in childImgs)
                            {
                                if (img.GetComponent<TattooConnection>())
                                {
                                    if (!img.GetComponent<TattooConnection>().activated && img.GetComponent<TattooConnection>().related)
                                    {
                                        if (!img.GetComponentInChildren<TextMeshProUGUI>())
                                        {
                                            Image imgChild = img.transform.GetChild(0).GetComponent<Image>();
                                            imgChild.color = FadeInColor(imgChild.color, 1f);
                                        }
                                        else
                                        {
                                            TextMeshProUGUI tmp = img.GetComponentInChildren<TextMeshProUGUI>();
                                            tmp.color = FadeInColor(tmp.color, 1f);
                                        }
                                    }
                                }
                            }
                            referenceColor = FadeInColor(referenceColor, 1f);
                            if (referenceColor.a == 1)
                                transitionStage = "ResetValues";
                            break;

                        case "ResetValues":
                            noDrag = false;
                            currentTattoo = null;
                            referenceColor = new Color(0, 0, 0, 0);
                            //targetObj = null;
                            firstActivated = true;
                            fadingColor = false;
                            break;
                    }
                }
                else
                {
                    foreach (Image img in childImgs)
                    {
                        if (img.GetComponent<TattooConnection>())
                        {
                            if (img.GetComponent<TattooConnection>().activated)
                                img.color = FadeInColor(img.color, 1);
                            else if (img.GetComponent<TattooConnection>().related)
                            {
                                if (!img.GetComponentInChildren<TextMeshProUGUI>())
                                {
                                    Image imgChild = img.transform.GetChild(0).GetComponent<Image>();
                                    imgChild.color = FadeInColor(imgChild.color, 1f);
                                }
                                else
                                {
                                    TextMeshProUGUI tmp = img.GetComponentInChildren<TextMeshProUGUI>();
                                    tmp.color = FadeInColor(tmp.color, 1f);
                                }
                            }
                        }
                    }

                    blackLine.color = FadeInColor(blackLine.color, 1);
                    greyLine.color = FadeInColor(greyLine.color, .5f);
                    pannelBG.color = FadeInColor(pannelBG.color, BGAlpha);
                    FadeInBlur();

                    if (greyLine.color.a < .5f)
                        fadingColor = true;
                    else
                        fadingColor = false;

                }
            }
            else
            {
                foreach (Image img in childImgs)
                {
                    if (img.GetComponent<TattooConnection>())
                    {
                        if (img.GetComponent<TattooConnection>().activated)
                            img.color = AlphaBasedOnScale(img.color, 1);
                        else if (img.GetComponent<TattooConnection>().related)
                        {
                            if (!img.GetComponentInChildren<TextMeshProUGUI>())
                            {
                                Image imgChild = img.transform.GetChild(0).GetComponent<Image>();
                                imgChild.color = AlphaBasedOnScale(imgChild.color, 1f);
                                //Debug.Log(img.name);
                            }
                            else
                            {
                                TextMeshProUGUI tmp = img.GetComponentInChildren<TextMeshProUGUI>();
                                tmp.color = AlphaBasedOnScale(tmp.color, 1f);
                            }
                        }
                    }
                }
                blackLine.color = AlphaBasedOnScale(blackLine.color, 1);
                greyLine.color = AlphaBasedOnScale(greyLine.color, .5f);
            }
        }
        else
        {
            if (!playerUnpaused)
                UnpausePlayer();
            gotPos = false;
            noDrag = true;
            transitionStage = "SetCenterImgPos";
            timer = 0;
            DataHolder.HideHint(DataHolder.hints.tattooViewHint);
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
            }
            foreach (TextMeshProUGUI tmp in childTexts)
            {
                tmp.color = FadeOutColor(tmp.color);
            }
            blackLine.color = FadeOutColor(blackLine.color);
            greyLine.color = FadeOutColor(greyLine.color);
            if (!parentControl.takeOver)
            {
                pannelBG.color = FadeOutColor(pannelBG.color);
                FadeOutBlur();
            }
            if (greyLine.color.a > 0)
                fadingColor = true;
            else
                fadingColor = false;
            if (!clearedCurrent)
            {
                currentTattoo = null;
                //targetObj = null;
                parentControl.currentPanel = null;
                clearedCurrent = true;
            }
        }
        if (!noDrag)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * mouseDragSpeed, Input.GetAxis("Mouse Y") * mouseDragSpeed);
                GetComponent<RectTransform>().anchoredPosition += dragAmount;
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                float scrollAmount = Input.mouseScrollDelta.y;
                //1 to 0.5: resize
                //0.5 to 0.2: fade to outer menu
                transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + scrollAmount * scrollSizeSpeed, .2f, 2f), Mathf.Clamp(transform.localScale.y + scrollAmount * scrollSizeSpeed, .2f, 2f));
                if (transform.localScale.x < .5f)
                    parentControl.takeOver = true;
                else
                    parentControl.takeOver = false;
            }
        }
        if (OuterPanelController.mechanicActivated)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !fadingColor)
            {
                if (activated)
                    activated = false;
                else
                {
                    if (!ReferenceTool.playerHolding.inDialogue)
                        activated = true;
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //    DemoCatActivation();
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

    public void DemoCatActivation()
    {
        activated = true;
        catActivated = true;
        Invoke("ActivateCat", 5f);
    }

    void ActivateCat()
    {
        currentTattoo = transform.GetChild(3).GetComponent<RectTransform>();
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
            c -= new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, 0);
        return c;
    }

    Color AlphaBasedOnScale(Color c, float maxA)
    {
        float alpha = Mathf.Lerp(maxA, 0, Mathf.InverseLerp(.5f, .2f, transform.localScale.x));
        c = new Color(c.r, c.g, c.b, alpha);
        return c;
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

    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = GetComponent<RectTransform>().anchoredPosition;
        while (time < duration)
        {
            lerping = true;
            time += Time.deltaTime;
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, time / duration));
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
        lerping = false;
    }
}
