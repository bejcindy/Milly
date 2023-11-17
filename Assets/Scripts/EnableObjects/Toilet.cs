using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : FixedCameraObject
{
    public bool toiletUsed;
    public int useCount;

    bool addCount;
    bool diaDone;
    float useTime;
    DialogueSystemTrigger toiletDialogue;

    protected override void Update()
    {
        base.Update();
        if (isInteracting)
        {
            if (!addCount)
                AddCount();
            CheckDialogue();
        }

        else if(useCount == 2)
        {
            addCount = false;
            this.enabled = false;
        }
        else
        {
            addCount = false;
        }
    }

    public void AddCount()
    {
        addCount = true;
        useCount++;
        DialogueLua.SetVariable("Bathroom/ToiletCount", useCount);
        diaDone = false;
    }

    public void CheckDialogue()
    {
        if (!diaDone)
        {
            DialogueManager.StartConversation("Bathroom/Toilet");
            diaDone = true;
        }
    }

    private void OnDisable()
    {
        interactable = false;
    }
}
