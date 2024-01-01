using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBreathing : MonoBehaviour
{

    public AnimationCurve verticalCurve, horizontalCurve;
    public float amplitude, timeElapsed, frequency, rotationalAmplitude, phaseShift, horizontalAmplitude, motionSpeed;
    public float multiplier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        Vector3 pos = new Vector3(horizontalCurve.Evaluate(timeElapsed) * horizontalAmplitude * motionSpeed, verticalCurve.Evaluate(timeElapsed * frequency) * amplitude, transform.position.z); 
        transform.position = pos;

        Quaternion rot = Quaternion.Euler(verticalCurve.Evaluate(timeElapsed * frequency + phaseShift) * rotationalAmplitude, 0, 0);
        transform.rotation = rot;
    }
}
