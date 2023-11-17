using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : FixedCameraObject
{
    public bool toiletUsed;
    public int useCount;

    float useTime;
    DialogueSystemTrigger toiletDialogue;

    protected override void Update()
    {
        base.Update();
        if(isInteracting)
            CheckDialogue();
        else if(useCount == 2)
        {
            this.enabled = false;
        }
    }

    public void AddCount()
    {
        useCount++;
        DialogueLua.SetVariable("Bathroom/ToiletCount", useCount);
    }

    public void CheckDialogue()
    {
        useTime += Time.deltaTime;
        if (useTime >= 5f)
        {
            useTime = 0;
            DialogueManager.StartConversation("Bathroom/Toilet");
            StartCoroutine(UnfixPlayer());
        }
    }
}
