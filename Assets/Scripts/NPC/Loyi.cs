using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loyi : NPCControl
{
    public bool overrideLoyi;
    public bool mainStarted;
    public bool turnChar;
    public GameObject phone;
    protected override void Start()
    {
        base.Start();
        overrideNoControl = true;
        if (overrideLoyi)
        {
            ExitIzaCutsceneStage();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (mainStarted)
        {
            StopIdle();
            remainInAnim = false;
            noMoveAfterTalk = false;
            phone.SetActive(false);
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

        noLookInConvo = true;
    }

    public void LoyiAction2()
    {
        phone.SetActive(true);
        allowLookPlayer = false;
        remainInAnim = true;
        noMoveAfterTalk = true;
    }

    public void LoyiAction3()
    {

    }

    public void ExitCutscene()
    {
    }
}
