using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hugo : NPCControl 
{
    protected override void Start()
    {
        base.Start();

        noMoveAfterTalk = true;
        noLookInConvo = true;
    }

    public void HugoAction1()
    {
        noMoveAfterTalk = true;
        currentDialogue.gameObject.SetActive((true));

    }
}
