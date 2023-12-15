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

[Serializable]
public class HintTexts
{
    [TextArea]
    public string throwHint, smokeHint, lookHint, drinkHint, kickHint, scrollHint, sitHint, cigHint, chopHint, pickFoodHint, eatHint,powderHint,tabHint,tableDrinkHint, drinkAndThrowHint, tattooViewHint, pizzaHint;
}

public class DataHolder : MonoBehaviour
{
    static float minBlur = .1f;
    static float maxBlur = .75f;
    static float focusDist = .75f;
    static DepthOfField dof;
    static DepthOfField colorDof;
    static GameObject postProcessingVolume;
    static GameObject chromaticVolume;

    public static bool focusing;
    public static bool focused;
    static Volume v,cv;

    public static GameObject currentFocus;

    public static CinemachineVirtualCamera focusCinemachine;
    static CinemachineVirtualCamera playerCinemachine;
    public static CinemachineBrain playerBrain;
    public static bool camBlended, camBlendDone;

    static Transform originalPlayerCmFollow;

    [SerializeField]
    [InspectorName("Hints")]
    HintTexts hintsReference;
    public static HintTexts hints;
    public GameObject hintPanelPrefab;
    public GameObject hintPrefab;
    public Transform canvasRef;
    static Transform canvas;
    static GameObject hintPanel;
    static GameObject hintPref;

    //static TextMeshProUGUI hintTMP;
    static PlayerHolding playerHolding;
    static PlayerLeftHand playerLeftHand;
    static PlayerMovement playerMovement;
    public static CinemachinePOV pov;
    static float originalVerticalSpeed, originalHorizontalSpeed;
    //static string currentHint;
    static List<string> currentHints;
    static List<GameObject> hintPanels;
    public static bool canMakeSound;
    float beginningAudioCoolDownTimer;
    bool hintOff,turnedOffHint;

    // Start is called before the first frame update
    void Start()
    {
        //reset public static variables
        focusing = false;
        focused = false;
        currentFocus = null;
        camBlended = false;
        camBlendDone = false;
        canMakeSound = false;

        focusCinemachine = GameObject.Find("FocusCinemachine").GetComponent<CinemachineVirtualCamera>();
        playerCinemachine = GameObject.Find("PlayerCinemachine").GetComponent<CinemachineVirtualCamera>();
        playerBrain = Camera.main.GetComponent<CinemachineBrain>();
        originalPlayerCmFollow = playerCinemachine.Follow;
        postProcessingVolume = GameObject.Find("MonoVolume");
        chromaticVolume = GameObject.Find("ChromeVolume");
        v = postProcessingVolume.GetComponent<Volume>();
        cv = chromaticVolume.GetComponent<Volume>();

        hintPanel = hintPanelPrefab;
        hintPref = hintPrefab;
        hints = hintsReference;
        currentHints = new List<string>();
        hintPanels = new List<GameObject>();
        canvas = canvasRef;

        playerHolding = GameObject.Find("Player").GetComponent<PlayerHolding>();
        playerLeftHand = GameObject.Find("Player").GetComponent<PlayerLeftHand>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        pov = playerCinemachine.GetCinemachineComponent<CinemachinePOV>();
        originalHorizontalSpeed = pov.m_HorizontalAxis.m_MaxSpeed;
        originalVerticalSpeed = pov.m_VerticalAxis.m_MaxSpeed;
    }

    // Update is called once per frame
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
    }

    #region Focusing and Unfocusing
    public static void FocusOnThis(float matColorVal)
    {
        playerLeftHand.bypassThrow = true;

        if (playerBrain.IsBlending)
            camBlended = true;
        if (camBlended && !playerBrain.IsBlending)
            camBlendDone = true;

        if (camBlendDone)
        {
            if (focusDist > .1f)
            {
                float speed = Mathf.Lerp(0.75f, 0.001f, Mathf.InverseLerp(1, 0, matColorVal));
                focusDist = speed;
            }
            else
            {
                focusDist = .001f;
                focused = true;
            }


            if (v.profile.TryGet<DepthOfField>(out dof))
            {
                dof.focusDistance.value = focusDist;
            }
            if (cv.profile.TryGet<DepthOfField>(out colorDof))
            {
                colorDof.focusDistance.value = focusDist;
            }
        }
        

    }

    public static void Unfocus()
    {
        playerCinemachine.LookAt = null;
        focusCinemachine.Priority = 1;
        focusCinemachine.LookAt = null;
        playerCinemachine.ForceCameraPosition(playerCinemachine.transform.position, focusCinemachine.transform.rotation);


        if (focusDist < .75f)
        {
            currentFocus.layer = 13;
            focusDist += .5f * Time.deltaTime;
        }
        else
        {
            pov.m_HorizontalAxis.m_MaxSpeed = originalHorizontalSpeed;
            pov.m_VerticalAxis.m_MaxSpeed = originalVerticalSpeed;
            focusDist = .75f;
            currentFocus.layer = 17;
            camBlended = false;
            camBlendDone = false;
            playerHolding.looking = false;
            playerMovement.enabled = true;
            playerLeftHand.bypassThrow = false;

            currentFocus = null;
            focused = false;
        }

        if (v.profile.TryGet<DepthOfField>(out dof))
        {
            dof.focusDistance.value = focusDist;
        }
        if (cv.profile.TryGet<DepthOfField>(out colorDof))
        {
            colorDof.focusDistance.value = focusDist;
        }

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
                //instantiatedPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(25, 200 - currentHints.Count * 200, 0);
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
                //instantiatedPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hint;
                //instantiatedPanel.SetActive(true);
                hintPanels.Add(instantiatedPanel);
                currentHints.Add(hint);
                instantiated = true;
            }
        }
        //string[] parsed = hints.throwHint.Split("\n");
        //foreach (string s in parsed)
        //{
        //    int buttonInt = s.IndexOf(" ");
        //    string button = s.Substring(0, buttonInt);
        //    string usage = s.Replace(button, "");
        //}

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
        //if (hintToHide == currentHint)
        //{
        //    hintPanel.SetActive(false);
        //    hintTMP.text = null;
        //}
    }
    #endregion
}
