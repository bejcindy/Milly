using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loyi : NPCControl
{
    public bool overrideLoyi;
    protected override void Start()
    {
        base.Start();
        mainNPC = true;
        overrideNoControl = true;
        if (overrideLoyi)
        {
            ExitIzaCutsceneStage();
        }
    }
     
    public void ExitIzaCutsceneStage()
    {
        firstTalk = true;
        agent.enabled = true;
        machine.ChangeState(machine.moveState);


    }

    public void LoyiAction1()
    {
        noLookInConvo = false;
    }

    public void LoyiAction2()
    {
        noLookInConvo = false;
    }

    public void ExitCutscene()
    {
        inCutscene = false;
    }
}
