using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Beautify.Universal;
using PixelCrushers.DialogueSystem;

public class SMHAdjustTest : MonoBehaviour
{
    public VolumeProfile monoProfile;
    public float lerpDuration;
    bool afterLutDiaDone;

    ColorAdjustments colorAdjust;

    [Button]
    void ResetToDefault()
    {
        BeautifySettings.settings.lutIntensity.Override(0);
        //BeautifySettings.settings.lut.Override(false);
        //BeautifySettings.settings.lutTexture.Override(null);
        BeautifySettings.settings.vignettingBlink.value = 0;
        if (monoProfile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = -100;
        }
    }
    

    public IEnumerator LerpToPizzaColor(Texture2D pizzaLUT)
    {        
        float t = 0;
        BeautifySettings.UnloadBeautify();
        BeautifySettings.settings.lut.Override(true);
        BeautifySettings.settings.lutTexture.Override(pizzaLUT);
        while (t < lerpDuration)
        {
            
            float intensity = Mathf.Lerp(0, 1, t / lerpDuration);
            
            BeautifySettings.settings.lutIntensity.Override(intensity);
            if (monoProfile.TryGet<ColorAdjustments>(out colorAdjust))
            {
                colorAdjust.saturation.value = Mathf.Lerp(-100, 0, t / lerpDuration);
            }

            t += Time.deltaTime;
            Debug.Log(intensity);
            yield return null;
        }

        BeautifySettings.settings.lutIntensity.Override(1);
        if (monoProfile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = 0f;
        }

        yield break;
    }

    public IEnumerator LerpToDefaultColor()
    {
        float t = 0;

        while (t < lerpDuration)
        {
            float intensity = Mathf.Lerp(1, 0, t / lerpDuration);

            BeautifySettings.settings.lutIntensity.Override(intensity);

            if (monoProfile.TryGet<ColorAdjustments>(out colorAdjust))
            {
                colorAdjust.saturation.value = Mathf.Lerp(0, -100, t / lerpDuration);
            }

            t += Time.deltaTime;
            yield return null;
        }
        BeautifySettings.settings.lutIntensity.Override(0);
        BeautifySettings.settings.lut.Override(false);
        BeautifySettings.settings.lutTexture.Override(null);
        if (monoProfile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.saturation.value = -100f;
        }
        if (!afterLutDiaDone)
        {
            afterLutDiaDone = true;
            DialogueManager.StartConversation("Pizza/AfterLut");
        }

        yield break;
    }

    private void OnDisable()
    {
        ResetToDefault();
    }
}
