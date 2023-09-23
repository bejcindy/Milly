using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxShift : MonoBehaviour
{
    public Color dawn, night;
    public Material skyboxMaterial;
    public static float skyboxProgress;
    public float shiftSpeed;

    [SerializeField]
    float currentProgress;
    [SerializeField]
    float actualProgress;

    float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentProgress != skyboxProgress)
        {
            if (actualProgress != skyboxProgress)
            {
                t += Time.deltaTime;
                actualProgress = Mathf.Lerp(currentProgress, skyboxProgress, t * shiftSpeed);
                Color lerpedColor = Color.Lerp(dawn, night, actualProgress);
                skyboxMaterial.SetColor("_TintColor", lerpedColor);
            }
            else
            {
                currentProgress = skyboxProgress;
                t = 0;
            }
        }
        
    }
}
