using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Charles : NPCControl
{
    public bool charlesSmoking;
    public bool charlesOpenDoor;

    protected override void Start()
    {
        base.Start();
        firstTalk = true;

    }
    protected override void Update()
    {
        base.Update();
    }

    public void CharlesAction1()
    {
        
        

    }

    public void CharlesAction2()
    {
        noTalkStage = true;
        if(charlesOpenDoor)
            destObjects[_counter - 1].GetComponent<Door>().NPCOpenDoor();

    }

    public void CharlesAction3()
    {
        noTalkStage = false;
    }

    public void FinishSmoking()
    {
        Debug.Log("finishing smoking");
        //machine.ResetAnimTrigger("Special1");
        machine.SetAnimatorTrigger("Finish");
    }


}
