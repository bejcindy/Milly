using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loyi : NPCControl
{
    public bool overrideLoyi;

    public bool turnChar;
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
        talkable = true;
        agent.enabled = true;
        machine.ChangeState(machine.moveState);

    }

    public void LoyiAction1()
    {
        noLookInConvo = true;
        if (!turnChar)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 1f);
            turnChar = true;
        }
        
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
