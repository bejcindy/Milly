using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zayne : NPCControl
{
    protected override void Start()
    {
        base.Start();
        talkable = true;
        noMoveAfterTalk = true;
    }


}
