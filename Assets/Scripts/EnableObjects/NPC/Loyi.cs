using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loyi : NPCControl
{
    protected override void Start()
    {
        base.Start();
        mainNPC = true;
        overrideNoControl = true;
    }

    public void ExitIzaCutsceneStage()
    {
        firstTalk = true;
        machine.ChangeState(machine.moveState);
        agent.enabled = true;
    }

    public void LoyiAction1()
    {
        noLookInConvo = true;
    }

    public void ExitCutscene()
    {
        inCutscene = false;
    }
}
