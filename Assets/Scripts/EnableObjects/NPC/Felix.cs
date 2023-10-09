using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Felix : NPCControl
{

    public BuildingGroupController hintOne;

    protected override void Start()
    {
        base.Start();
        questTriggered = true;
        firstTalk = true;

    }
    public void FelixAction1()
    {
        noLookInConvo = true;
        currentDialogue = dialogues[1];

    }

    public void FelixAction2()
    {
        noTalkStage = true;
    }

    public void FelixStandUp()
    {
        machine.SetAnimatorTrigger("StandUp");
    }

    public void ActivateHintOne()
    {
        hintOne.activateAll = true;
    }

}
