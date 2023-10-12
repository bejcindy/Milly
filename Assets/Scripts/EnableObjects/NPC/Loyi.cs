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
        firstTalk = true;
        inCutscene = true;
    }

    public void ExitIzaCutsceneStage()
    {
        machine.ChangeState(machine.moveState);
    }

    public void LoyiAction1()
    {

    }

    public void ExitCutscene()
    {
        inCutscene = false;
    }
}
