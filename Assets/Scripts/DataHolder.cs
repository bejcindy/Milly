using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using TMPro;
using System;

[Serializable]
public class HintTexts
{
    [TextArea]
    public string throwHint, smokeHint, lookHint, drinkHint, talkHint, scrollHint;
}

public class DataHolder : MonoBehaviour
{
    static float minBlur = .1f;
    static float maxBlur = .75f;
    static float focusDist = .75f;
    static DepthOfField dof;
    static GameObject postProcessingVolume;

    public static bool focusing;
    public static bool focused;
    static Volume v;

    public static GameObject currentFocus;

    static CinemachineVirtualCamera focusCinemachine;
    static CinemachineVirtualCamera playerCinemachine;
    public static CinemachineBrain playerBrain;
    public static bool camBlended, camBlendDone;

    static Transform originalPlayerCmFollow;

    [SerializeField]
    [InspectorName("Hints")]
    HintTexts hintsReference;
    public static HintTexts hints;

    static GameObject hintPanel;
    static GameObject dotImage;
    static TextMeshProUGUI hintTMP;

    // Start is called before the first frame update
    void Start()
    {
        focusCinemachine = GameObject.Find("FocusCinemachine").GetComponent<CinemachineVirtualCamera>();
        playerCinemachine = GameObject.Find("PlayerCinemachine").GetComponent<CinemachineVirtualCamera>();
        playerBrain = Camera.main.GetComponent<CinemachineBrain>();
        originalPlayerCmFollow = playerCinemachine.Follow;
        postProcessingVolume = GameObject.Find("GlowVolume");
        v = postProcessingVolume.GetComponent<Volume>();
        hintPanel = GameObject.Find("HintPanel");
        dotImage = hintPanel.transform.GetChild(0).gameObject;
        hintTMP = hintPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        hintPanel.SetActive(false);
        hints = hintsReference;
        //focusCinemachine.Priority = playerCinemachine.Priority + 1;
        //focusCinemachine.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!focusing && focused)
        {
            Unfocus();
        }
    }

    #region Focusing and Unfocusing
    public static void FocusOnThis(float fadeInterval,float matColorVal)
    {
        if (currentFocus)
        {
            currentFocus.layer = 12;
            //focused = true;
            //focusCinemachine.gameObject.SetActive(true);
            focusCinemachine.Priority = playerCinemachine.Priority + 1;
            focusCinemachine.LookAt = currentFocus.transform;
            //Cursor.lockState = CursorLockMode.Locked;

            playerCinemachine.LookAt= currentFocus.transform;
            if(playerBrain.IsBlending)
                camBlended = true;
            if (camBlended && !playerBrain.IsBlending)
                camBlendDone = true;
            //playerCinemachine.gameObject.SetActive(false);
            //playerCinemachine.Follow = focusCinemachine.transform;
            //Debug.Log("focusing");
            if (camBlendDone)
            {
                focused = true;
                if (focusDist > .1f)
                {
                    //focusDist -= 0.1f * fadeInterval * Time.deltaTime;
                    //0.75f,0.1f
                    float speed = Mathf.Lerp(0.75f, 0.001f, Mathf.InverseLerp(1, 0, matColorVal));
                    focusDist = speed;
                    focusing = true;
                }
                else
                {
                    focusDist = .001f;
                    //currentFocus = null;
                    focusing = false;
                    Debug.Log("false1");
                    currentFocus.GetComponent<LookingObject>().focusingThis = false;
                    //playerCinemachine.Follow = focusCinemachine.transform;
                }

                //Debug.Log("focus dist" + focusDist);
                //postProcessingVolume.GetComponent<DepthOfField>().focusDistance.value = focusDist;
                //Volume v = postProcessingVolume.GetComponent<Volume>();
                if (v.profile.TryGet<DepthOfField>(out dof))
                {
                    dof.focusDistance.value = focusDist;
                }
            }
        }
        else
        {
            focusing = false;
            //Debug.Log("false2");
        }
        //playerCam.m_Lens.FieldOfView = 120;
    }

    public static void Unfocus()
    {
        if (!focusing)
        {
            focusCinemachine.Priority = 1;
            //focusCinemachine.gameObject.SetActive(false);
            focusCinemachine.LookAt = null;
            //playerCinemachine.gameObject.SetActive(true);
            playerCinemachine.ForceCameraPosition(playerCinemachine.transform.position, focusCinemachine.transform.rotation);
            //playerCinemachine.Follow = originalPlayerCmFollow;
            playerCinemachine.LookAt = null;
            //focusCinemachine.GetCinemachineComponent<CinemachinePOV>().
            //Cursor.lockState = CursorLockMode.None;

            if (focusDist < .75f)
            {
                currentFocus.layer = 13;
                focusDist += .5f * Time.deltaTime;
            }
            else
            {
                focusDist = .75f;
                //changeThis = false;
                currentFocus.layer = 0;
                currentFocus = null;
                focused = false;
                camBlended = false;
                camBlendDone = false;
                //playerCinemachine.Follow = originalPlayerCmFollow;
                //if (matColorVal <= 0)
                //    go.GetComponent<LookingObject>().enabled = false;
            }

            //postProcessingVolume.GetComponent<DepthOfField>().focusDistance.value = focusDist;
            //Volume v = postProcessingVolume.GetComponent<Volume>();
            if (v.profile.TryGet<DepthOfField>(out dof))
            {
                dof.focusDistance.value = focusDist;
            }
        }
        //else
        //{
        //    currentFocus.layer = 0;
        //    currentFocus = null;
        //}
            

    }
    #endregion

    /// <summary>
    /// put in "DataHolder.hints.blablabla" for the string
    /// </summary>
    /// <param name="hint"></param>
    public static void ShowHint(string hint)
    {
        hintTMP.text = hint;
        hintPanel.SetActive(true);
        dotImage.SetActive(true);
        hintTMP.gameObject.SetActive(true);
    }

    public static void HideHint()
    {
        hintPanel.SetActive(false);
        hintTMP.text = null;
    }
}
