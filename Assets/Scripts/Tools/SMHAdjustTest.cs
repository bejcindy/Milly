using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SMHAdjustTest : MonoBehaviour
{
    public VolumeProfile profile;
    public float lerpDuration;

    [Foldout("Default Values (Don't Touch)")]
    public Vector4 defaultShadow;
    public Vector4 defaultMidtone;
    public Vector4 defaultHighlight;
    public float defaultVignette;

    [Foldout("Current Values")]
    public Vector4 currentShadow;
    public Vector4 currentMidtone;
    public Vector4 currentHighlight;
    public float currentVignette;

    ShadowsMidtonesHighlights shadowMidHigh;
    Vignette vignette;

    [Button]
    void ResetToDefault()
    {
        if (profile.TryGet<ShadowsMidtonesHighlights>(out shadowMidHigh))
        {
            shadowMidHigh.shadows.value = defaultShadow;
            shadowMidHigh.midtones.value = defaultMidtone;
            shadowMidHigh.highlights.value = defaultHighlight;
        }
        if(profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = defaultVignette;
        }
    }

    [Button]
    void CurrentValues()
    {
        if (profile.TryGet<ShadowsMidtonesHighlights>(out shadowMidHigh))
        {
            currentShadow = shadowMidHigh.shadows.value;
            currentMidtone = shadowMidHigh.midtones.value;
            currentHighlight = shadowMidHigh.highlights.value;
        }
        if (profile.TryGet<Vignette>(out vignette))
        {
            currentVignette = vignette.intensity.value;
        }
    }


    public IEnumerator LerpToPizzaColor(Vector4 targetShadow,Vector4 targetMidtone,Vector4 targetHighlight,float targetVignette)
    {
        float t = 0;
        while (t < lerpDuration)
        {
            if (profile.TryGet<ShadowsMidtonesHighlights>(out shadowMidHigh))
            {
                shadowMidHigh.shadows.value = Vector4.Lerp(defaultShadow, targetShadow, t / lerpDuration);
                shadowMidHigh.midtones.value = Vector4.Lerp(defaultMidtone, targetMidtone, t / lerpDuration);
                shadowMidHigh.highlights.value = Vector4.Lerp(defaultHighlight, targetHighlight, t / lerpDuration);
            }
            if (profile.TryGet<Vignette>(out vignette))
            {
                vignette.intensity.value = Mathf.Lerp(defaultVignette, targetVignette, t / lerpDuration);
            }
            t += Time.deltaTime;
            yield return null;
        }
        shadowMidHigh.shadows.value = targetShadow;
        shadowMidHigh.midtones.value = targetMidtone;
        shadowMidHigh.highlights.value = targetHighlight;
        vignette.intensity.value = targetVignette;

        yield return new WaitForSeconds(30f);

        StartCoroutine(LerpToDefaultColor());

        yield break;
    }

    public IEnumerator LerpToDefaultColor()
    {
        float t = 0;
        Vector4 shadow = shadowMidHigh.shadows.value;
        Vector4 midtone = shadowMidHigh.midtones.value;
        Vector4 highlight = shadowMidHigh.highlights.value;
        float intensity = vignette.intensity.value;

        while (t < lerpDuration)
        {
            if (profile.TryGet<ShadowsMidtonesHighlights>(out shadowMidHigh))
            {
                shadowMidHigh.shadows.value = Vector4.Lerp(shadow, defaultShadow, t / lerpDuration);
                shadowMidHigh.midtones.value = Vector4.Lerp(midtone, defaultMidtone, t / lerpDuration);
                shadowMidHigh.highlights.value = Vector4.Lerp(highlight, defaultHighlight, t / lerpDuration);
            }
            if (profile.TryGet<Vignette>(out vignette))
            {
                vignette.intensity.value = Mathf.Lerp(intensity, defaultVignette, t / lerpDuration);
            }
            t += Time.deltaTime;
            yield return null;
        }
        shadowMidHigh.shadows.value = defaultShadow;
        shadowMidHigh.midtones.value = defaultMidtone;
        shadowMidHigh.highlights.value = defaultHighlight;
        vignette.intensity.value = defaultVignette;
        yield break;
    }

    private void OnDisable()
    {
        ResetToDefault();
    }
}
