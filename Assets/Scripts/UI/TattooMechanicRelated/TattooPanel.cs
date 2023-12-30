using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TattooPanel : MonoBehaviour
{
    public bool panelOn;
    public CenterTattoo centerTat;

    public Image panelBG;
    Animator fade;
    CanvasGroup canvasGroup;

    public bool showCenter;

    RectTransform panelTransform;
    public bool lerping;

    bool playerUnpaused;
    float mouseDragSpeed = 70;
    float scrollSizeSpeed = .1f;
    public bool noDrag;

    public OuterPanelController outerPanel;
    public bool activatedOnce;

    // Start is called before the first frame update
    void Start()
    {
        panelTransform = GetComponent<RectTransform>();
        fade = GetComponentInParent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (panelOn)
        {
            OuterPanelController.mechanicActivated = true;
            PausePlayer();
            activatedOnce = true;
            if (outerPanel.currentPanel != this)
            {
                if(outerPanel.currentPanel)
                    outerPanel.currentPanel.enabled = false;
                outerPanel.currentPanel = this;
            }
            if (canvasGroup.alpha == 0)
            {
                fade.Play("TattooPanelOpen");
                Invoke("MakePanelVisible", .25f);
            }

            if (!centerTat.colorOn && !centerTat.activated)
            {
                centerTat.forceActivate = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                panelOn = false;
                fade.Play("TattooPanelClose");
                Invoke("MakePanelInvisible", .25f);
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
                    {
                        outerPanel.gameObject.SetActive(true);
                        canvasGroup.alpha = AlphaBasedOnScale(1);
                    }
                    else
                        canvasGroup.alpha = 1;
                }
            }
        }
        else if(OuterPanelController.mechanicActivated)
        {
            if (!playerUnpaused)
                UnpausePlayer();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                panelOn = true;
                fade.Play("TattooPanelOpen");
                FocusOnTattoo(centerTat.GetComponent<RectTransform>());
            }
        }
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

    float AlphaBasedOnScale(float maxA)
    {
        float alpha = Mathf.Lerp(maxA, 0, Mathf.InverseLerp(.5f, .2f, transform.localScale.x));
        return alpha;
    }

    public void FocusOnTattoo(RectTransform tat)
    {
        panelTransform.anchoredPosition = -tat.anchoredPosition;
        transform.localScale = new Vector2(1, 1);
    }

    void MakePanelVisible()
    {
        canvasGroup.alpha = 1;
    }
    void MakePanelInvisible()
    {
        canvasGroup.alpha = 0;
    }

    public IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        lerping = true;
        float time = 0;
        Vector3 startPosition = GetComponent<RectTransform>().anchoredPosition;
        while (time < duration)
        {
            time += Time.deltaTime;
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, time / duration));
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
        lerping = false;
    }
}
