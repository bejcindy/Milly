using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loyi : NPCControl
{
    public bool overrideLoyi;
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

    }

    public void MoveLoyiToVinyl()
    {
        _counter = 2;
        StopIdle();
        phone.SetActive(false);
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
        phone.SetActive(true);
        allowLookPlayer = false;

    }

    public void LoyiAction3()
    {
        phone.SetActive(false);

    }

    public void LoyiAction4()
    {
        talkable = false;
    }

    public void ExitCutscene()
    {
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
        if(_counter == 3)
        {
            DialogueLua.SetVariable("Vinyl/LoyiInBF", true);
            MainQuestState.demoProgress++;
        }
    }
}
