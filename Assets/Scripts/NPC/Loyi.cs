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
        if (!turnChar)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 1f);
            turnChar = true;
        }
        
    }

    public void LoyiAction2()
    {
        allowLookPlayer = false;
    }

    public void ExitCutscene()
    {
    }
}
