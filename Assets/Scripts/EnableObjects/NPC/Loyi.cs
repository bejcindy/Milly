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
}
