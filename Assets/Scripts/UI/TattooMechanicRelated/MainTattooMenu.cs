using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using Beautify.Universal;

public class MainTattooMenu : MonoBehaviour
{
    public TattooPanel activePanel;
    public CharacterPanel characterPanel;
    public bool showPanel;
    public bool playerUnpaused;
    public static bool tatMenuOn;
    Animator menuFade;
    public Animator vfxMenuFade;
    float mouseDragSpeed = 70;
    public EventReference panelOpenSound;
    public GameObject blurCanvas;
    public bool lerping;
    public bool mainQuestBegun;

    public TextMeshProUGUI[] incompleteTexts;

    float blinkDuration = 1f;

    bool incompleteShowed;

    bool playAnim;
    bool firstActivated;

    FMOD.Studio.EventInstance snapshot;
    float I = 0;
    float fadeInterval = 10f;

    CursorLockMode modeBeforePanel;
    bool cursorVisibility;
    bool gotCursor;
    string blinkSF = "event:/Sound Effects/Tattoo/Blink";
    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = false;
        menuFade = GetComponent<Animator>();
        playAnim = true;
        blurCanvas.SetActive(false);
        mainQuestBegun = false;
        foreach (TextMeshProUGUI tmp in incompleteTexts)
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //    StartCoroutine(Blink());
        if (showPanel)
        {
            if (!gotCursor)
            {
                modeBeforePanel = Cursor.lockState;
                cursorVisibility = Cursor.visible;
                gotCursor = true;
            }
            else
            {
                tatMenuOn = true;
                firstActivated = true;

                if (playAnim)
                {
                    menuFade.SetTrigger("FadeIn");
                    vfxMenuFade.SetTrigger("FadeIn");
                    RuntimeManager.PlayOneShot(panelOpenSound);
                    snapshot = RuntimeManager.CreateInstance("snapshot:/EnableObject");
                    snapshot.start();
                    StartCoroutine(FadeInFilter());
                    playAnim = false;
                }
                PausePlayer();
                if (activePanel == characterPanel)
                {
                    DataHolder.ShowHint(DataHolder.hints.outerPanelHint);
                    DataHolder.HideHintExceptThis(DataHolder.hints.outerPanelHint);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    DataHolder.ShowHint(DataHolder.hints.tattooViewHint);
                    DataHolder.HideHintExceptThis(DataHolder.hints.tattooViewHint);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    //Debug.Log("here3");
                }
            }
        }
        //Debug.Log(Cursor.lockState + "; " + Cursor.visible);

        if (Input.GetKeyDown(KeyCode.Tab) && firstActivated)
        {
            if (showPanel && !lerping)
            {
                tatMenuOn = false;
                menuFade.SetTrigger("FadeOut");
                vfxMenuFade.SetTrigger("FadeOut");
                UnpausePlayer();
                showPanel = false;
                StartCoroutine(FadeOutFilter());
                playAnim = true;
                if (gotCursor)
                {
                    Cursor.lockState = modeBeforePanel;
                    Cursor.visible = cursorVisibility;
                    gotCursor = false;
                }
            }
            else if (mainQuestBegun)
            {
                showPanel = true;

            }
        }

        if (mainQuestBegun && !incompleteShowed)
        {
            foreach (TextMeshProUGUI tmp in incompleteTexts)
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);
            incompleteShowed = true;
        }
    }

    IEnumerator Blink()
    {
        float t = 0;
        while (t < blinkDuration)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(0, 1, t / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        RuntimeManager.PlayOneShot(blinkSF);
        while (t >= blinkDuration && t < blinkDuration * 2)
        {
            BeautifySettings.settings.vignettingBlink.value = Mathf.Lerp(1, 0, (t-blinkDuration) / blinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.vignettingBlink.value = 0;
        yield break;
    }

    public void StartMainTattooQuest()
    {
        mainQuestBegun = true;
    }

    public void TurnOffActivePanel()
    {
        activePanel.panelOn = false;
        activePanel.gameObject.SetActive(false);
        blurCanvas.SetActive(false);
    }

    public void TurnOnActivePanel()
    {
        activePanel.panelOn = true;
        activePanel.transform.localScale = new Vector2(1, 1);
        activePanel.gameObject.SetActive(true);
        activePanel.ResetPosition();
        activePanel.MakePanelVisible();
        blurCanvas.SetActive(true);
    }

    public void ChangePanel()
    {
        activePanel.panelOn = false;
        activePanel.gameObject.SetActive(false);
        activePanel = characterPanel;
    }


    public void PausePlayer()
    {
        playerUnpaused = false;

        ReferenceTool.playerLeftHand.bypassThrow = true;
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = false;
        }
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
        GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void UnpausePlayer()
    {
        playerUnpaused = true;
        DataHolder.HideHint(DataHolder.hints.tattooViewHint);
        DataHolder.HideHint(DataHolder.hints.outerPanelHint);
        ReferenceTool.playerLeftHand.bypassThrow = false;
        CinemachineVirtualCamera currentCam = ReferenceTool.playerBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = true;
        }

        if (currentCam.GetCinemachineComponent<CinemachinePOV>())
        {
            currentCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 200;
            currentCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 200;
        }
        GetComponent<GraphicRaycaster>().enabled = false;
    }

    IEnumerator FadeOutFilter()
    {
        while (I > 0)
        {
            I -= 0.1f * fadeInterval * Time.deltaTime;
            snapshot.setParameterByName("EnableFilterIntensity", I);
            yield return null;
        }

        I = 0;
        snapshot.setParameterByName("EnableFilterIntensity", I);
        snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        snapshot.release();
        yield break;
    }

    IEnumerator FadeInFilter()
    {
        while (I < 1)
        {
            I += 0.1f * fadeInterval * Time.deltaTime;
            snapshot.setParameterByName("EnableFilterIntensity", I);
            yield return null;
        }
        I = 1;
        yield break;
    }
}
