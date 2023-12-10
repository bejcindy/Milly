using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : FixedCameraObject 
{
    public float sitTime;
    public bool npcBound;
    public NPCControl npcControl;
    public bool thoughtDone;
    
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
            if (!npcBound)
            {
                GetComponent<DialogueSystemTrigger>().enabled = true;
            }
            else
            {
                if(npcControl.gameObject.name == "Kelvin")
                    DialogueLua.SetVariable("NPC/Kelvin/PlayerAtRock", true);
                npcControl.npcActivated = true;
            }

            thoughtDone = true;
        }
    }
}
