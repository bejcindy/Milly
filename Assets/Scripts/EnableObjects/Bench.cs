using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : FixedCameraObject 
{
    public float sitTime;
    public bool thoughtDone;
    float sitTimeVal;

    protected override void Start()
    {
        base.Start();
        sitTimeVal = sitTime;
    }

    protected override void Update()
    {
        base.Update();
        if (isInteracting && !thoughtDone)
        {
            DetectSitThought();
        }
    }

    void DetectSitThought()
    {
        if (sitTime > 0)
        {
            sitTime -= Time.deltaTime;
        }
        else
        {
            GetComponent<DialogueSystemTrigger>().enabled = true;
            thoughtDone = true;
        }
    }
}
