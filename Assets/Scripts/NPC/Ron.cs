using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ron : NPCControl
{
    protected override void Start()
    {
        base.Start();

        noLookInConvo = true;

    }

    public void RonAction1()
    {
        talkable = true;
        noLookInConvo = false;
        remainInAnim = false;
    }

    public void RonAction2()
    {
        noLookInConvo = true;
        noTalkStage = true;
    }
}
