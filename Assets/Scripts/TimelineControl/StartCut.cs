using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricFogAndMist2;

public class StartCut : MonoBehaviour
{
    public VolumetricFogProfile fog;
    public bool fadingFog = false;
    public float fogFadeSpeed;

    public bool turnOnLights;
    public Light[] lights;
    public GameObject logo;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera windowCam;
    public float[] intensities;
    // Start is called before the first frame update
    void Start()
    {
        playerCam.m_Priority = 10;
        windowCam.m_Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingFog)
        {
            if (fog.albedo.a > 0)
                fog.albedo.a -= Time.deltaTime * fogFadeSpeed;
            else
            {
                fog.albedo.a = 0;
                fadingFog = false;
            }

        }

        if (turnOnLights)
        {
            TurnOnLights();
        }
    }

    public void FadeOutFog()
    {
        fadingFog = true;
        turnOnLights = true;
    }

    public void TurnOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].intensity < intensities[i])
            {
                lights[i].intensity += Time.deltaTime * fogFadeSpeed;
            }
            else
            {
                lights[i].intensity = intensities[i];
            }
        }
    }
}
