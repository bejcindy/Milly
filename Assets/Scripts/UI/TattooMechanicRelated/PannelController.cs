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
//using UnityEditor.VersionControl;

public class PannelController : MonoBehaviour
{
    public static bool mechanicActivated;
    public bool activated;
    public float fadeSpeed;
    public RectTransform CanvasRect;
    public Image pannelBG;
    float BGAlpha;
    public Renderer targetObj;
    public RectTransform currentTattoo;
    public float mouseDragSpeed, scrollSizeSpeed;

    Image[] childImgs;
    public UILineRendererList blackLine, greyLine;

    bool gotPos, noDrag, firstActivated;
    bool lerping;
    float timer;
    bool playerUnpaused;
    Renderer previousObj;
    RectTransform previousTattoo;

    [SerializeField] PlayerHolding playerHolding;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera catCamera;
    bool catActivated;


    bool clearedCurrent;
    bool fadingColor;
    [SerializeField] Material blurMaterial;

    private void Awake()
    {
        childImgs = GetComponentsInChildren<Image>();
        foreach (Image img in childImgs)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }
        blackLine.color = new Color(blackLine.color.r, blackLine.color.g, blackLine.color.b, 0);
        greyLine.color = new Color(greyLine.color.r, greyLine.color.g, greyLine.color.b, 0);
        BGAlpha = pannelBG.color.a;
        pannelBG.color = new Color(pannelBG.color.r, pannelBG.color.g, pannelBG.color.b, 0);
        mechanicActivated = false;
        clearedCurrent = true;
        blurMaterial.SetFloat("_Alpha", 0);
    }

    void Update()
    {
        if (activated)
        {
            PausePlayer();

            //PixelCrushers.UIPanel.monitorSelection = false; // Don't allow dialogue UI to steal back input focus.
            //PixelCrushers.DialogueSystem.DialogueManager.Pause(); // Stop DS timers (e.g., sequencer commands).

            clearedCurrent = false;
            DataHolder.ShowHint(DataHolder.hints.tattooViewHint);
            if (!mechanicActivated)
                mechanicActivated = true;
            if (targetObj && currentTattoo && !gotPos)
            {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(targetObj.bounds.center);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
                if (targetObj.isVisible)
                {
                    if (pannelBG.color.a == 0)
                        GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition - currentTattoo.anchoredPosition;
                    else
                        StartCoroutine(LerpPosition(WorldObject_ScreenPosition - currentTattoo.anchoredPosition, 1f));
                }
                else
                {
                    GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                }

                currentTattoo.GetComponent<TattooConnection>().activated = true;
                foreach (RectTransform relate in currentTattoo.GetComponent<TattooConnection>().relatedTattoos)
                    relate.GetComponent<TattooConnection>().related = true;
                GetComponent<ConnectionManager>().ActivateLines(currentTattoo.GetComponent<TattooConnection>());
                transform.localScale = new Vector2(1, 1);
                previousObj = targetObj;
                previousTattoo = currentTattoo;
                gotPos = true;
                noDrag = true;
            }
            else if (!targetObj || gotPos)
            {
                if (currentTattoo)
                {
                    if (targetObj != previousObj || currentTattoo != previousTattoo)
                        gotPos = false;
                    currentTattoo.GetComponent<Image>().color = FadeInColor(currentTattoo.GetComponent<Image>().color, 1);
                    if (currentTattoo.GetComponent<Image>().color.a < 1)
                        fadingColor = true;
                    if (currentTattoo.GetComponent<Image>().color.a == 1)
                    {
                        if (greyLine.color.a != .5f)
                            timer += Time.deltaTime;
                        else
                            fadingColor = false;
                        //Debug.Log(timer);
                        if (timer > .5f && timer < 2f)
                        {
                            pannelBG.color = FadeInColor(pannelBG.color, BGAlpha);
                            FadeInBlur();
                        }
                        if (timer > 2f && timer < 3.5f)
                        {
                            if (firstActivated)
                            {
                                foreach (Image img in childImgs)
                                {
                                    if (img.GetComponent<TattooConnection>())
                                    {
                                        if (img.GetComponent<TattooConnection>().activated)
                                            img.color = FadeInColor(img.color, 1);
                                    }
                                }
                                blackLine.color = FadeInColor(blackLine.color, 1);
                            }
                            else
                            {
                                Debug.Log("here");
                                foreach (Image img in childImgs)
                                {
                                    if (img.GetComponent<TattooConnection>())
                                    {
                                        if (!img.GetComponent<TattooConnection>().activated && img.GetComponent<TattooConnection>().related)
                                            img.color = FadeInColor(img.color, .5f);
                                    }
                                }
                                greyLine.color = FadeInColor(greyLine.color, .5f);
                            }
                        }
                        if (firstActivated && timer > 3.5f)
                        {
                            foreach (Image img in childImgs)
                            {
                                if (img.GetComponent<TattooConnection>())
                                {
                                    if (!img.GetComponent<TattooConnection>().activated && img.GetComponent<TattooConnection>().related)
                                        img.color = FadeInColor(img.color, .5f);
                                }
                            }
                            greyLine.color = FadeInColor(greyLine.color, .5f);
                        }

                    }
                }
                if (!currentTattoo)
                {
                    foreach (Image img in childImgs)
                    {
                        if (img.GetComponent<TattooConnection>())
                        {
                            if (img.GetComponent<TattooConnection>().activated)
                                img.color = FadeInColor(img.color, 1);
                            else if (img.GetComponent<TattooConnection>().related)
                                img.color = FadeInColor(img.color, .5f);
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
            if (greyLine.color.a == .5f && !lerping)
            {
                noDrag = false;
                firstActivated = true;
                fadingColor = false;
            }

        }
        else
        {
            if(!playerUnpaused)
                UnpausePlayer();
            gotPos = false;
            noDrag = true;
            timer = 0;
            DataHolder.HideHint(DataHolder.hints.tattooViewHint);
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
            }
            blackLine.color = FadeOutColor(blackLine.color);
            greyLine.color = FadeOutColor(greyLine.color);
            pannelBG.color = FadeOutColor(pannelBG.color);
            FadeOutBlur();
            if (greyLine.color.a > 0)
                fadingColor = true;
            else
                fadingColor = false;
            if (!clearedCurrent)
            {
                currentTattoo = null;
                targetObj = null;
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
                transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + scrollAmount * scrollSizeSpeed, .5f, 2f), Mathf.Clamp(transform.localScale.y + scrollAmount * scrollSizeSpeed, .5f, 2f));
            }
        }

        if (mechanicActivated)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !fadingColor)
            {
                if (activated)
                    activated = false;
                else
                {
                    if (!playerHolding.inDialogue)
                        activated = true;
                }

            }
        }
    }

    public void PausePlayer()
    {
        playerUnpaused = false;
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = false;
        }
        playerMovement.enabled = false;
        if (Input.GetMouseButton(0))
        {
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseDragSpeed * .1f;
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseDragSpeed * .1f;

            if (catActivated)
            {
                catCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseDragSpeed * .1f;
                catCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseDragSpeed * .1f;
            }
        }
        else
        {
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;

            if (catActivated)
            {
                catCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
                catCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
            }
        }
    }

    public void UnpausePlayer()
    {
        playerUnpaused = true;
        if (catActivated)
        {
            catActivated = false;
            catCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
            catCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        }


        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = true;
        }
        playerMovement.enabled = true;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
        playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;

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
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            lerping = true;
            transform.localPosition = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
        lerping = false;

    }
}
