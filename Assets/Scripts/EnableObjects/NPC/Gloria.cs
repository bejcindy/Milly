using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{

    protected override void Start()
    {
        base.Start();
        firstTalk = true;
    }

    public void GloriaAction1()
    {
        currentDialogue = dialogues[0];
    }
}
