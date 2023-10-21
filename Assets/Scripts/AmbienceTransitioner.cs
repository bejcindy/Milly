using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AmbienceTransitioner : MonoBehaviour
{
    public EventReference thisPlays, fadeTo;
    FMOD.Studio.EventInstance currentAmb, fadeToAmb;
    public float fadeSpeed;
    public float currentVol;
    public bool fadeIn, fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        currentAmb = RuntimeManager.CreateInstance(thisPlays);
        fadeToAmb = RuntimeManager.CreateInstance(fadeTo);
        currentAmb.start();
        fadeToAmb.start();
        currentAmb.setVolume(currentVol);
        fadeToAmb.setVolume(0);
    }

    private void Update()
    {
        if (fadeOut)
        {
            if (currentVol > 0)
            {
                currentVol -= Time.deltaTime * fadeSpeed;
            }
            else
            {
                currentVol = 0;
                fadeOut = false;
            }
        }
        if (fadeIn)
        {
            if (currentVol < 1)
            {
                currentVol += Time.deltaTime * fadeSpeed;
            }
            else
            {
                currentVol = 1;
                fadeIn = false;
            }
        }
        currentAmb.setVolume(currentVol);
        fadeToAmb.setVolume(1 - currentVol);
        if (currentVol == 1)
        {
            fadeToAmb.setPaused(true);
        }
        else
        {
            fadeToAmb.setPaused(false);
        }
        if (currentVol == 0)
        {
            currentAmb.setPaused(true);
        }
        else
        {
            currentAmb.setPaused(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeOut = true;
        }
    }

}
