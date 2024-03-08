using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCActivateObject : LivableObject
{

    DialogueSystemTrigger dialogue;

    protected override void Update()
    {
        base.Update();
        if (activated && dialogue != null)
        {
            dialogue.enabled = true;
            player.GetComponent<Renderer>().enabled = false;
        }
    }
}
