using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{

    protected override void Start()
    {
        base.Start();
        overrideNoControl = true;
        talkable = true;
    }
     
    public void GloriaAction1()
    {
        stopIdleAfterConvo = true;
    }

    public void GloriaAction2()
    {
        noTalkStage = true;
        gameObject.SetActive(false);
    }

}
