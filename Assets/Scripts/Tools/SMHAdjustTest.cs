using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Beautify.Universal;

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
        BeautifySettings.settings.lutIntensity.Override(0);
        BeautifySettings.settings.lut.Override(false);
        BeautifySettings.settings.lutTexture.Override(null);
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


    public IEnumerator LerpToPizzaColor(Texture2D pizzaLUT)
    {
        float t = 0;
        BeautifySettings.settings.lut.Override(true);
        BeautifySettings.settings.lutTexture.Override(pizzaLUT);
        while (t < lerpDuration)
        {
            
            float intensity = Mathf.Lerp(0, 1, t / lerpDuration);
            
            BeautifySettings.settings.lutIntensity.Override(intensity);
            
            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.lutIntensity.Override(1);

        yield break;
    }

    public IEnumerator LerpToDefaultColor()
    {
        float t = 0;

        while (t < lerpDuration)
        {
            float intensity = Mathf.Lerp(1, 0, t / lerpDuration);

            BeautifySettings.settings.lutIntensity.Override(intensity);

            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.lutIntensity.Override(0);
        BeautifySettings.settings.lut.Override(false);
        BeautifySettings.settings.lutTexture.Override(null);
        yield break;
    }

    private void OnDisable()
    {
        ResetToDefault();
    }
}
