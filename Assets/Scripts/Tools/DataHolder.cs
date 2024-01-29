using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using VInspector;
using Beautify.Universal;

[Serializable]
public class HintTexts
{
    [TextArea]
    public string throwHint, inhaleHint, exhaleHint, lookHint, drinkHint, kickHint, scrollHint, sitHint, standHint, cigHint, chopHint, pickFoodHint, eatHint, powderHint, tabHint, tableDrinkHint, drinkAndThrowHint, tattooViewHint,outerPanelHint, pizzaHint;
}

public class DataHolder : MonoBehaviour
{
    #region Blur Related
    public static bool focusing;
    public static bool focused;
    public static bool camBlended, camBlendDone;
    public static GameObject currentFocus;
    public CinemachineVirtualCamera focusVcam;
    public static CinemachineVirtualCamera focusCinemachine;
    public static CinemachineBrain playerBrain;
    public static CinemachinePOV pov;

    static float focusDist = .75f;
    static float originalVerticalSpeed, originalHorizontalSpeed;
    static DepthOfField dof;
    static DepthOfField colorDof;
    public Volume postProcessingVolume, chromaticVolume;
    static Volume v,cv;
    static CinemachineVirtualCamera playerCinemachine;
    #endregion

    #region Hint Related
    [SerializeField]
    [InspectorName("Hints")]
    HintTexts hintsReference;
    public GameObject hintPanelPrefab;
    public GameObject hintPrefab;
    public Transform canvasRef;
    
    public static HintTexts hints;

    static Transform canvas;
    static GameObject hintPanel;
    static GameObject hintPref;
    static List<string> currentHints;
    static List<GameObject> hintPanels;

    bool hintOff;
    #endregion

    public static bool canMakeSound;
    float beginningAudioCoolDownTimer;

    bool tatPanelOn;
    public Animator tatCanvasAnim;

    void Start()
    {
        //reset public static variables
        focusing = false;
        focused = false;
        currentFocus = null;
        camBlended = false;
        camBlendDone = false;
        canMakeSound = false;

        focusCinemachine = focusVcam;
        playerCinemachine = ReferenceTool.playerCinemachine;
        playerBrain = ReferenceTool.playerBrain;
        v = postProcessingVolume;
        cv = chromaticVolume;

        hintPanel = hintPanelPrefab;
        hintPref = hintPrefab;
        hints = hintsReference;
        currentHints = new List<string>();
        hintPanels = new List<GameObject>();
        canvas = canvasRef;
        pov = ReferenceTool.playerPOV;
        originalHorizontalSpeed = pov.m_HorizontalAxis.m_MaxSpeed;
        originalVerticalSpeed = pov.m_VerticalAxis.m_MaxSpeed;
    }

    void Update()
    {
        if (beginningAudioCoolDownTimer < 2)
            beginningAudioCoolDownTimer += Time.deltaTime;
        else
        {
            canMakeSound = true;
        }

        if (focused)
        {
            Unfocus();
        }

        if (Input.GetKeyDown(KeyCode.Y))
            hintOff = !hintOff;
        if (hintOff && hintPanels.Count != 0)
        {
            for(int i=0;i< hintPanels.Count; i++)
            {
                hintPanels[i].SetActive(false);
            }
        }
        else if (!hintOff && hintPanels.Count != 0)
        {
            for (int i = 0; i < hintPanels.Count; i++)
            {
                hintPanels[i].SetActive(true);
            }
        }
        
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (!tatPanelOn)
        //        TurnOnTattooPanel();
        //    else
        //        TurnOffTattooPanel();
        //}
    }

    #region Focusing and Unfocusing
    public static void FocusOnThis(float matColorVal)
    {
        ReferenceTool.playerLeftHand.bypassThrow = true;

        if (playerBrain.IsBlending)
            camBlended = true;
        if (camBlended && !playerBrain.IsBlending)
            camBlendDone = true;

        if (camBlendDone)
        {
            if (focusDist < 1.8f)
            {
                float speed = Mathf.Lerp(0f, 2f, Mathf.InverseLerp(1, 0, matColorVal));
                focusDist = speed;
                Debug.Log("here");
            }
            else
            {
                focusDist = 2f;
                focused = true;
                Debug.Log("no here");
            }

            BeautifySettings.settings.blurIntensity.Override(focusDist);
        }       
    }

    public static void Unfocus()
    {
        playerCinemachine.LookAt = null;
        focusCinemachine.Priority = 1;
        focusCinemachine.LookAt = null;
        playerCinemachine.ForceCameraPosition(playerCinemachine.transform.position, focusCinemachine.transform.rotation);

        if (focusDist >0f)
        {
            //currentFocus.layer = 13;
            focusDist -= .5f * Time.deltaTime;
        }
        else
        {
            pov.m_HorizontalAxis.m_MaxSpeed = originalHorizontalSpeed;
            pov.m_VerticalAxis.m_MaxSpeed = originalVerticalSpeed;
            focusDist = 0f;
            //currentFocus.layer = 17;
            camBlended = false;
            camBlendDone = false;
            ReferenceTool.playerHolding.looking = false;
            ReferenceTool.playerMovement.enabled = true;
            ReferenceTool.playerLeftHand.bypassThrow = false;
            currentFocus.GetComponent<LookingObject>().focusingThis = false;
            currentFocus = null;
            focused = false;
        }

        BeautifySettings.settings.blurIntensity.Override(focusDist);

    }
    #endregion

    #region Hint Related
    /// <summary>
    /// put in "DataHolder.hints.blablabla" for the string
    /// </summary>
    /// <param name="hint"></param>
    public static void ShowHint(string hint)
    {
        bool instantiated = false;
        if (!instantiated)
        {
            if (currentHints.Count == 0 || !currentHints.Contains(hint))
            {
                GameObject instantiatedPanel = Instantiate(hintPanel, canvas);
                List<Image> imgs = new List<Image>();
                List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
                string[] parsed = hint.Split("\n");
                foreach (string s in parsed)
                {
                    GameObject instantiatedHintGroup = Instantiate(hintPref, instantiatedPanel.transform);
                    int buttonInt = s.IndexOf(" ");
                    string button = s.Substring(0, buttonInt);
                    string usage = s.Replace(button, "");
                    instantiatedHintGroup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = button;
                    imgs.Add(instantiatedHintGroup.transform.GetChild(0).GetComponent<Image>());
                    texts.Add(instantiatedHintGroup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>());
                    instantiatedHintGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = usage;
                    texts.Add(instantiatedHintGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>());
                }
                hintPanels.Add(instantiatedPanel);
                currentHints.Add(hint);
                instantiated = true;
            }
        }
    }
    
    public static void HideHint(string hintToHide)
    {
        bool hidden = false;
        if (hintPanels.Count != 0 && !hidden)
        {
            for (int i = 0; i < hintPanels.Count; i++)
            {
                if (currentHints[i] == hintToHide)
                {
                    Destroy(hintPanels[i]);
                    currentHints.Remove(hintToHide);
                    hintPanels.Remove(hintPanels[i]);
                    hidden = true;
                }
            }
        }
    }

    public static void HideHintExceptThis(string hintToExclude)
    {
        bool hidden = false;
        if (hintPanels.Count != 0 && !hidden)
        {
            for (int i = 0; i < hintPanels.Count; i++)
            {
                if (currentHints[i] != hintToExclude)
                {
                    Destroy(hintPanels[i]);
                    hintPanels.Remove(hintPanels[i]);
                    currentHints.Remove(currentHints[i]);
                    hidden = true;
                }
            }
        }
    }
    #endregion

    #region Tattoo Panel
    //public void TurnOnTattooPanel()
    //{
    //    tatCanvasAnim.Play("")
    //}

    //public void TurnOffTattooPanel()
    //{

    //}
    #endregion
}
